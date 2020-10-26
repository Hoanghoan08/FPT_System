using AsmAppDev2.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Web.Mvc;

namespace AsmAppDev2.Controllers
{
    // Create Role for Account
    public class RolesController : Controller
    {
        ApplicationDbContext context;

        public RolesController()
        {
            context = new ApplicationDbContext();
        }



        /// <summary>
        /// Get All Roles
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var Roles = context.Roles.ToList();
            return View(Roles);
        }

        /// <summary>
        /// Create  a New role
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            var Role = new IdentityRole();
            return View(Role);
        }

        /// <summary>
        /// Create a New Role
        /// </summary>
        /// <param name="Role"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(IdentityRole Role)
        {
            context.Roles.Add(Role);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}