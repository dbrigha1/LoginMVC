using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LoginMVC.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace LoginMVC.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
       // private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        

        public UsersController()
        {
        }

        public UsersController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
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
        // GET: Users
        public ActionResult Index()
        {
            return View(UserManager.Users.ToList());
        }

        //// GET: Users/Details/5
        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ApplicationUser applicationUser = db.Users.Find(id);
        //    if (applicationUser == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(applicationUser);
        //}

        //// GET: Users/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Users/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,UserRole,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Users.Add(applicationUser);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(applicationUser);
        //}

        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = UserManager.FindById(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            ApplicationDbContext context = new ApplicationDbContext();
            UserViewModel userModel = new UserViewModel
            {
                Id = applicationUser.Id,
                UserRole = applicationUser.UserRole,
                Email = applicationUser.Email,
               // PasswordHash = applicationUser.PasswordHash,
               // SecurityStamp = applicationUser.SecurityStamp,
                PhoneNumber = applicationUser.PhoneNumber,
                TwoFactorEnabled = applicationUser.TwoFactorEnabled,
                UserName = applicationUser.UserName,
                UserRoleList = context.Roles.Select(x=> new SelectListItem {Value = x.Name, Text = x.Name })
            };
            return View(userModel);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <ActionResult> Edit([Bind(Include = "Id,UserRole,UserRoleList,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] UserViewModel applicationUser)
        {
            if (ModelState.IsValid)
            {
                // Get the existing student from the db
                var user = await UserManager.FindByIdAsync(applicationUser.Id);

                // Id,UserRole,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName
                // Update it with the values from the view model
                // user.Id = applicationUser.Id;
                //  user.UserRole = applicationUser.UserRole;
                user.Email = applicationUser.Email;
                //user.EmailConfirmed = applicationUser.EmailConfirmed;
                //user.PasswordHash = applicationUser.PasswordHash;
                //user.SecurityStamp = applicationUser.SecurityStamp;
                user.PhoneNumber = applicationUser.PhoneNumber;
                //user.PhoneNumberConfirmed = applicationUser.PhoneNumberConfirmed;
                user.TwoFactorEnabled = applicationUser.TwoFactorEnabled;
                //user.LockoutEndDateUtc = applicationUser.LockoutEndDateUtc;
                //user.LockoutEnabled = applicationUser.LockoutEnabled;
                //user.AccessFailedCount = applicationUser.AccessFailedCount;
                user.UserName = applicationUser.UserName;
                //var selectedRole = applicationUser.UserRoleList.Where(x => x.Selected);
                await UserManager.RemoveFromRoleAsync(user.Id, user.UserRole);
                user.UserRole = applicationUser.UserRole;
                await UserManager.AddToRoleAsync(user.Id, user.UserRole);  
                
                var result = await UserManager.UpdateAsync(user);
                //db.Entry(applicationUser).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(applicationUser);
        }

        //// GET: Users/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ApplicationUser applicationUser = db.Users.Find(id);
        //    if (applicationUser == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(applicationUser);
        //}

        //// POST: Users/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(string id)
        //{
        //    ApplicationUser applicationUser = db.Users.Find(id);
        //    db.Users.Remove(applicationUser);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
