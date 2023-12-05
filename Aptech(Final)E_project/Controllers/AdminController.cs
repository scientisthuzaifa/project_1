using Aptech_Final_E_project.Context;
using Aptech_Final_E_project.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Aptech_Final_E_project.Controllers
{
    public class AdminController : Controller
    {
        private JameProjectEntities2 db = new JameProjectEntities2();

        private ApplicationUserManager _userManager;

        public AdminController()
        {
        }

        public AdminController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Admin
        [Authorize(Roles ="admin")]
        public ActionResult Index()

        { 
            //var img = db.Image_User.ToList();
            //    var mod = new myclass
            //    {
            //        image_Users = img,
            //    };
                
            return View();
        }
        // Home/UpdateProfile GET action
        public async Task<ActionResult> UpdateProfile()
        {
            var userId = User.Identity.GetUserId();
            ApplicationUser user = await UserManager.FindByIdAsync(userId);

            return View(user);
        }


        // Home/UpdateProfile POST action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateProfile(ApplicationUser model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var user = await UserManager.FindByIdAsync(userId);

                // Update user properties with values from the model
                user.UserName = model.UserName;
                user.U_Name = model.U_Name;
                user.Email = model.Email;
                user.P_Number = model.P_Number;


                // Add other properties as needed

                var result = await UserManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    var identity = UserManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    var authenticationManager = HttpContext.GetOwinContext().Authentication;
                    authenticationManager.SignIn(new AuthenticationProperties { IsPersistent = false }, identity);
                    // Redirect to a success page or update the user's profile and return to the profile page.
                    return RedirectToAction("Index", "Home"); // Assuming you have a "Profile" action in your "AccountController"
                }

                // If there are errors, add them to the ModelState and return to the update page.

            }

            // If we got this far, something failed, redisplay the form.
            return View(model);
        }
        public ActionResult ActivateForm()
        {
            // Set a session variable or update a database flag to activate the form
            Session["IsFormActivated"] = true;
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult DeactivateForm()
        {
            // Set a session variable or update a database flag to deactivate the form
            Session["IsFormActivated"] = false;
            return RedirectToAction("Index", "Admin");
        }
        public ActionResult ActivateWinner()
        {
            // Set a session variable or update a database flag to activate the form
            Session["IsFormActivated"] = true;
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult DeactivateWinner()
        {
            // Set a session variable or update a database flag to deactivate the form
            Session["IsFormActivated"] = false;
            return RedirectToAction("Index", "Admin");
        }
        public ActionResult WinnerAnnounce()
        {

            return View();
        }
    }
}