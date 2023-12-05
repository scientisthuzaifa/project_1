using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Aptech_Final_E_project.Context;
using Microsoft.AspNet.Identity;

namespace Aptech_Final_E_project.Controllers
{
    public class RecipesController : Controller
    {
        private JameProjectEntities2 db = new JameProjectEntities2();

        // GET: Recipes
        public ActionResult Index()
        {
         

            return View(db.Recipes.ToList());
        }

        // GET: Recipes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipes recipes = db.Recipes.Find(id);
            if (recipes == null)
            {
                return HttpNotFound();
            }
            return View(recipes);
        }

        // GET: Recipes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Ingredients,CookingProcess,Isfree,Images")] Recipes recipes, HttpPostedFileBase ImageFiles)
        {
            if (ModelState.IsValid)
            {
                // Generate a unique file name using the original file name, timestamp, and extension
                string fileName = Path.GetFileNameWithoutExtension(ImageFiles.FileName);
                string extension = Path.GetExtension(ImageFiles.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;

                // Combine the file name with the server's path to the image folder
                string imagePath = Path.Combine(Server.MapPath("~/All_Image/"), fileName);

                // Save the uploaded image to the specified path
                ImageFiles.SaveAs(imagePath);

                // Update the model's Image property with the relative image path
                recipes.Images = "~/All_Image/" + fileName;
                db.Recipes.Add(recipes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(recipes);
        }

        // GET: Recipes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipes recipes = db.Recipes.Find(id);
            if (recipes == null)
            {
                return HttpNotFound();
            }
            return View(recipes);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Ingredients,CookingProcess,Isfree,Images")] Recipes recipes, HttpPostedFileBase imageUpload)
        {
            if (ModelState.IsValid)
            {
                if (imageUpload != null)
                {
                    recipes.Images = imageUpload.FileName;
                    string Path = Server.MapPath("~/All_Image/" + imageUpload.FileName);
                    imageUpload.SaveAs(Path);
                }
                db.Entry(recipes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(recipes);
        }

        // GET: Recipes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipes recipes = db.Recipes.Find(id);
            if (recipes == null)
            {
                return HttpNotFound();
            }
            return View(recipes);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Recipes recipes = db.Recipes.Find(id);
            db.Recipes.Remove(recipes);
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
