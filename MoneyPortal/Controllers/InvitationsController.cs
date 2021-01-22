using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Management;
using System.Web.Mvc;
using CsQuery.ExtensionMethods;
using CsQuery.ExtensionMethods.Internal;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

using MoneyPortal.Classes;
using MoneyPortal.Models;
using MoneyPortal.Services;

namespace MoneyPortal.Controllers
{
    [Authorize]
    public class InvitationsController : Controller
    {
        private UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));


        private ApplicationDbContext db = new ApplicationDbContext();

        //POST: Invitations/CheckInvitation
        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = ("Personal"))]
        public JsonResult CheckInvitation(string code)
        {
            code = code.Trim();
            var Gcode = new Guid();
            try
            {
                Gcode = Guid.Parse(code);
            }
            catch(Exception ex)
            {
                return Json(false);
            }
            var invitation = db.Invitations.Where(i => i.Code == Gcode).First();
            var userEmail = db.Users.Find(User.Identity.GetUserId()).Email;

            if (invitation == null)
            {
                return Json(false);
            }
            else if (invitation.RecipientEmail == userEmail)
            {
                return Json(code);
            }
            return Json(false);

        }

        //GET: Invitations/Join
        [Authorize(Roles = ("Personal"))]
        public async Task<ActionResult> JoinHousehold(string code)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var invitation = db.Invitations.Where(i => i.RecipientEmail == user.Email && i.Valid == true).First();
            try
            {
                if (Guid.Parse(code) != invitation.Code)
                    return RedirectToAction("Main", "Dashboard");
            }
            catch
            {
                return RedirectToAction("Main", "Dashboard");
            }
            invitation.Valid = false;
            db.Entry(invitation).State = EntityState.Modified;

            user.HouseholdId = invitation.HouseholdId;
            db.Entry(user).State = EntityState.Modified;
            
            db.SaveChanges();

            userManager.RemoveFromRoles(user.Id, "Personal");
            userManager.AddToRole(user.Id, "Member");

            await UserAuthorization.RefreshAuthentication(HttpContext, user);

            return RedirectToAction("Main", "Dashboard");
        }

        // POST: Invitations/Create
        [HttpPost]
        [Authorize(Roles = "Owner")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Invite(string RecipientEmails, string PersonalMessage)
        {
            if (RecipientEmails.IsNullOrEmpty())
                return Json(false);

            var userId = User.Identity.GetUserId();
            var householdId = db.Users.Find(userId).HouseholdId;

            var recipients = new List<string>(RecipientEmails.Split(','));

            var result = new Dictionary<string, List<string>>();
            result.Add("Success", new List<string>());
            result.Add("Failed", new List<string>());
            foreach (var email in recipients)
            {
                ////////// DEMO ONLY ////////////
                if(email.Trim() == "rebuild")
                {
                    db.Users.Where(u => u.Demo == true).ForEach(u => u.HouseholdId = 1);
                    db.SaveChanges();
                    continue;
                }
                /////////////////////////////////
                if (new EmailAddressAttribute().IsValid(email.Trim()))
                {
                    //check for valid invitation, and set invalid
                    var oldInvitations = db.Invitations.Where(i => i.RecipientEmail == email.Trim() && i.Valid == true);
                    foreach (var inv in oldInvitations)
                    {
                        inv.Valid = false;
                        db.Entry(inv).State = EntityState.Modified;
                    }
                    //create invitation
                    var invitation = new Invitation
                    {
                        HouseholdId = (int)householdId,
                        Created = DateTime.Now,
                        RecipientEmail = email.Trim(),
                        PersonalMessage = PersonalMessage,
                        Valid = true,
                        Code = Guid.NewGuid()
                    };
                    db.Invitations.Add(invitation);
                    db.SaveChanges();
                    //check if email is already a user, send proper invitation body
                    if (db.Users.Any(u => u.Email == email.Trim()))
                    {
                        await SendInvitation(invitation.Id, InviteBody.CurrentUser);
                    }
                    else
                    {
                        await SendInvitation(invitation.Id, InviteBody.NonRegisteredEmail);
                    }
                    result["Success"].Add(email);
                }
                else
                {
                    result["Failed"].Add(email);
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private async Task SendInvitation(int invitationId, InviteBody bodyType)
        {
            var invitation = db.Invitations.Find(invitationId);
            var userId = User.Identity.GetUserId();
            var household = db.Households.Where(h => h.OwnerId == userId).FirstOrDefault();
            var callbackUrl = Url.Action("Register", "Account", null, protocol: Request.Url.Scheme);

            if (InviteBody.CurrentUser == bodyType)
            {
                //must implement a join to household for current users:
                callbackUrl = Url.Action("Login", "Account", null, protocol: Request.Url.Scheme); //, new { email = invitation.RecipientEmail, code = invitation.Code }, protocol: Request.Url.Scheme);
            }
            else if (InviteBody.NonRegisteredEmail == bodyType)
            {
                callbackUrl = Url.Action("ConfirmInvitation", "Account", new { email = invitation.RecipientEmail, code = invitation.Code }, protocol: Request.Url.Scheme);
            }

            //MailGun API EmailSender
            var message = $"<p>{ household.Owner.FullName } has invited you to their Money Portal Household. </p>" +
                            $"<p>\"{ invitation.PersonalMessage }\"</p>";
            var sender = new EmailSender();
            await sender.Execute("You've been invited!", "mp-invitation", invitation.RecipientEmail, new InvitationTemplate(callbackUrl, message, invitation.Code.ToString()));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private enum InviteBody
        {
            CurrentUser,
            NonRegisteredEmail
        }
    }
}