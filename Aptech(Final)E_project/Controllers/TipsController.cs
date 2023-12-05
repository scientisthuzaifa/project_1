using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Aptech_Final_E_project.Context;

namespace Aptech_Final_E_project.Controllers
{
    public class TipsController : Controller
    {
        private JameProjectEntities2 db = new JameProjectEntities2();

        // GET: Tips
        public ActionResult Index()
        {
            return View(db.Tips.ToList());
        }

        // GET: Tips/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tips tips = db.Tips.Find(id);
            if (tips == null)
            {
                return HttpNotFound();
            }
            return View(tips);
        }

        // GET: Tips/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tips/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Description,DatePosted")] Tips tips)
        {
            if (ModelState.IsValid)
            {
                db.Tips.Add(tips);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tips);
        }

        // GET: Tips/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tips tips = db.Tips.Find(id);
            if (tips == null)
            {
                return HttpNotFound();
            }
            return View(tips);
        }

        // POST: Tips/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Description,DatePosted")] Tips tips)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tips).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tips);
        }

        // GET: Tips/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tips tips = db.Tips.Find(id);
            if (tips == null)
            {
                return HttpNotFound();
            }
            return View(tips);
        }

        // POST: Tips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tips tips = db.Tips.Find(id);
            db.Tips.Remove(tips);
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
