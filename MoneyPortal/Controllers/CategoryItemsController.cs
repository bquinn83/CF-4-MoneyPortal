using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MoneyPortal.Models;

namespace MoneyPortal.Controllers
{
    public class CategoryItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CategoryItems
        public ActionResult Index()
        {
            var categoryItems = db.CategoryItems.Include(c => c.Category);
            return View(categoryItems.ToList());
        }

        // GET: CategoryItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryItem categoryItem = db.CategoryItems.Find(id);
            if (categoryItem == null)
            {
                return HttpNotFound();
            }
            return View(categoryItem);
        }

        // GET: CategoryItems/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        // POST: CategoryItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CategoryId,Name")] CategoryItem categoryItem)
        {
            if (ModelState.IsValid)
            {
                db.CategoryItems.Add(categoryItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", categoryItem.CategoryId);
            return View(categoryItem);
        }

        // GET: CategoryItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryItem categoryItem = db.CategoryItems.Find(id);
            if (categoryItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", categoryItem.CategoryId);
            return View(categoryItem);
        }

        // POST: CategoryItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CategoryId,Name")] CategoryItem categoryItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(categoryItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", categoryItem.CategoryId);
            return View(categoryItem);
        }

        // GET: CategoryItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryItem categoryItem = db.CategoryItems.Find(id);
            if (categoryItem == null)
            {
                return HttpNotFound();
            }
            return View(categoryItem);
        }

        // POST: CategoryItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CategoryItem categoryItem = db.CategoryItems.Find(id);
            db.CategoryItems.Remove(categoryItem);
            db.SaveChanges();
            return RedirectToAction("Index");
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
