using Aptech_Final_E_project.Context;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Aptech_Final_E_project.Controllers
{
   
    public class FeedBackController : Controller
    {
        JameProjectEntities2 dbobj = new JameProjectEntities2();
        // GET: FeedBack
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FeedBacks(FeedBack model)
        {
            var userId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                var obj = new FeedBack()
                {
                    User_id = userId,
                    Contents= model.Contents,
                    Rating=model.Rating,
                    datePosted=DateTime.Now
                    
            };
                dbobj.FeedBack.Add(obj);
                dbobj.SaveChanges();
                return RedirectToAction("Index", "Home");

            }
            return View(model);
        }
        public ActionResult feedback()
        {
            var feed = dbobj.FeedBack.ToList();


            return View(feed);



        }
    }
}