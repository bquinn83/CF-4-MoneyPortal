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
using CsQuery.ExtensionMethods.Internal;
using Microsoft.AspNet.Identity;
using MoneyPortal.Models;

namespace MoneyPortal.Controllers
{
    public class InvitationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // POST: Invitations/Create
        [HttpPost]
        [Authorize(Roles = "Owner")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> InviteAsync(string RecipientEmails, string PersonalMessage)
        {
            if (RecipientEmails.IsNullOrEmpty())
                return RedirectToAction("Main", "Dashboard", new { ErrorMessage = "Recipients field must have at least 1 email." });

            var userId = User.Identity.GetUserId();
            var householdId = db.Users.Find(userId).HouseholdId;

            var recipients = new List<string>(RecipientEmails.Split(','));
            var invitesSent = 0;
            foreach (var email in recipients)
            {
                if (new EmailAddressAttribute().IsValid(email.Trim()))
                {
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

                    await SendInvitation(invitation.Id);
                    invitesSent++;
                }
            }

            return RedirectToAction("Main", "Dashboard", new { Message = $"You have sent { invitesSent } invitations.", success = (invitesSent > 0) });
        }

        private async Task SendInvitation(int invitationId)
        {
            var invitation = db.Invitations.Find(invitationId);
            var userId = User.Identity.GetUserId();
            var household = db.Households.Where(h => h.OwnerId == userId).FirstOrDefault();
            var callbackUrl = Url.Action("ConfirmInvitation", "Invitations", new { email = invitation.RecipientEmail, code = invitation.Code }, protocol: Request.Url.Scheme);

            var mail = new EmailModel()
            {
                FromName = "Money Portal",
                FromEmail = WebConfigurationManager.AppSettings["OutlookFrom"],
                ToEmail = invitation.RecipientEmail,
                Subject = "You've been Invited to join a Money Portal Household!",
                Body =  $"<p>{ household.Owner.FullName } has invited you to their Money Portal Household. </p>" +
                        $"<p>\"{ invitation.PersonalMessage }\"</p>" +
                        $"<p>Please click <a href=\"{callbackUrl}\">here</a> to join the household!</p>"
            };
            await mail.Launch();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
