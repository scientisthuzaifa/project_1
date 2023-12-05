using Aptech_Final_E_project.Context;
using Aptech_Final_E_project.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Aptech_Final_E_project.Controllers
{
    [Authorize] // Add this attribute to restrict access to authenticated users
    public class HomeController : Controller
    {
        private JameProjectEntities2 db = new JameProjectEntities2();
        private ApplicationUserManager _userManager;

        public HomeController()
        {
        }
        public bool UserHasActivesubscription(string userId)
        {
            var userSubscription = db.Subscription.Where(s => s.user_id == userId && s.Subscription_Status == "Active").ToList();
            return userSubscription.Any();
        }
        public HomeController(ApplicationUserManager userManager)
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

        // Home/Index action
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (User.IsInRole("admin"))
            {
               


                return RedirectToAction("Index", "Admin");
            }

            var userId = User.Identity.GetUserId();
            var announcements = db.Announcement.ToList();
            var recipes = db.Recipes.ToList();
            var FAQS = db.FAQ.ToList();
            var Tip=db.Tips.ToList();
            
            bool hasActiveSubscription = UserHasActivesubscription(userId);
            
            var model = new ModelClass
            {
                Announcements = announcements,
                Receipes = recipes,
                UserHasActivesubscription = hasActiveSubscription,// Add this property to the model
                FAQs = FAQS,
                tips=Tip
                
            };

            return View(model);
        }


        // Home/Register action
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        // Home/Login action
        [AllowAnonymous]
        public ActionResult Login()
        {
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Subscription(Subscription model)
        {
            var userId = User.Identity.GetUserId();
            var userIsSubscribed = db.Subscription.FirstOrDefault(s => s.user_id == userId);

            if (userIsSubscribed != null)
            {
                if (userIsSubscribed.Subscription_Status == "Deactive")
                {
                    userIsSubscribed.SubscriptionType = model.SubscriptionType;
                    userIsSubscribed.Subscription_Status = "Active";
                    userIsSubscribed.Subscription_Date = System.DateTime.Now;
                    db.SaveChanges(); // Save changes to update the existing subscription
                }
                TempData["Subscription"] = "You are already subscribed.";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var user = new Subscription()
                    {
                        user_id = userId,
                        SubscriptionType = model.SubscriptionType,
                        Subscription_Status = "Active",
                        Subscription_Date = System.DateTime.Now
                    };

                    db.Subscription.Add(user);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index", "Home");
        }



        public ActionResult feedback()
        {
            var feed = db.FeedBack.ToList();


            return View(feed);



        }
        [AllowAnonymous]
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
        public ActionResult Context()
        {
            return View();
        }

        JameProjectEntities2 dbobj = new JameProjectEntities2();

        [HttpPost]
        public ActionResult Context(contestDB model)
        {
            var userid = User.Identity.GetUserId();

            // Check if a record with the same ID already exists
            bool isExistingForm = dbobj.contestDB.Any(item => item.userid == userid && item.IsActive == true);

            if (isExistingForm)
            {


                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                contestDB obj = new contestDB()
                {
                    userid = userid,
                    Title = model.Title,
                    Ingradient = model.Ingradient,
                    CookingProcess = model.CookingProcess,
                    IsActive = true
                };

                dbobj.contestDB.Add(obj);
                dbobj.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            // If ModelState.IsValid is false, return the view with the model to display validation errors
            return View(model);
        }
        public ActionResult show()
        {
            var list = dbobj.contestDB.ToList();
            return View(list);
        }
        public ActionResult Winner()
        {
            var list = dbobj.Winner.ToList();
            return View(list);
           
        }

        [HttpPost]
        public ActionResult Winner(Winner model,int id)
        {

            bool isExistingForm = dbobj.Winner.Any(item => item.UserId == model.UserId);


            if (isExistingForm)
            {


               return RedirectToAction("Index", "Home");
           }
           
            
            if (ModelState.IsValid)
            {

                Winner obj = new Winner
                {
                    UserId = model.UserId,
                    Contestid = id
                };

               
                db.Winner.Add(obj);
                db.SaveChanges();

               
                return RedirectToAction("Index","Admin"); 
            }
            return View(model);
        }
       
   
        public ActionResult Delete(int id)
        {
            Winner winner=db.Winner.Find(id);
            db.Winner.Remove(winner);
            db.SaveChanges();

            return RedirectToAction("Winner","Home");
        }
       
        public ActionResult WinnerAnnounce()
        {

            var list = dbobj.Winner.ToList();
            return View(list);
        }
        public ActionResult ViewSubscription()
        {
            var list = dbobj.Subscription.ToList();
            return View(list);
        }
        [HttpPost]
        public ActionResult ViewSubscription( int id)
        {
            var subscription = dbobj.Subscription.FirstOrDefault(s => s.Id == id);

            if (subscription != null && subscription.Subscription_Status == "Active")
            {
                subscription.Subscription_Status = "Deactive";
                dbobj.SaveChanges();
                return RedirectToAction("ViewSubscription", "Home");
            }
            return View();
            //else
            //{
            //    if (subscription != null && subscription.Subscription_Status == "Deactive")
            //    {
            //        subscription.Subscription_Status = "Active";
            //        dbobj.SaveChanges();
            //    }

            //    // Redirect back to the view with the updated data
            //    return RedirectToAction("ViewSubscription", "Home");
            //}
        }
           
          
            }

        }

    

