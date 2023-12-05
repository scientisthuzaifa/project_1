using Aptech_Final_E_project.Context;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Aptech_Final_E_project.Controllers
{
    public class AdminImagesController : Controller
    {
        private JameProjectEntities2 db = new JameProjectEntities2();
        // GET: AdminImages
        public ActionResult Index()
        {
           
      return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,User_id,Image")] Image_User image_User, HttpPostedFileBase ImageFiles)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();

                // Check if an image with the same User_id exists
                var existingImage = db.Image_User.SingleOrDefault(i => i.User_id == userId);

                if (existingImage != null)
                {
                    // An image already exists for this user, update it
                    string oldImagePath = Server.MapPath(existingImage.Image);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath); // Delete the old image
                    }

                    // Generate a unique file name for the new image
                    string fileName = Path.GetFileNameWithoutExtension(ImageFiles.FileName);
                    string extension = Path.GetExtension(ImageFiles.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;

                    // Combine the file name with the server's path to the image folder
                    string imagePath = Path.Combine(Server.MapPath("~/All_Image/"), fileName);

                    // Save the uploaded image to the specified path
                    ImageFiles.SaveAs(imagePath);

                    // Update the existing image record with the new information
                    existingImage.Image = "~/All_Image/" + fileName;

                    db.Entry(existingImage).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    // No image exists for this user, create a new record
                    string fileName = Path.GetFileNameWithoutExtension(ImageFiles.FileName);
                    string extension = Path.GetExtension(ImageFiles.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;

                    string imagePath = Path.Combine(Server.MapPath("~/All_Image/"), fileName);
                    ImageFiles.SaveAs(imagePath);

                    image_User.User_id = userId;
                    image_User.Image = "~/All_Image/" + fileName;

                    db.Image_User.Add(image_User);
                    db.SaveChanges();
                }

                return RedirectToAction("Index","Admin");
            }

            ViewBag.User_id = new SelectList(db.AspNetUsers, "Id", "U_Name", image_User.User_id);
            return View(image_User);
        }

    }
}