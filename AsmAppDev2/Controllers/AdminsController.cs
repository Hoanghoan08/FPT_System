using AsmAppDev2.Models;
using AsmAppDev2.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Linq;
using System.Web.Mvc;

namespace AsmAppDev2.Controllers
{
    // This file contains the DELETE and EDIT for the Admin Role
    public class AdminsController : Controller
    {
        //Create bridges between models and databases
        private ApplicationDbContext _context;

        public AdminsController()
        {
            _context = new ApplicationDbContext();
        }

        //Get: Manage user
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var useradmin = (from user in _context.Users
                             select new
                             /*FROM-IN: xác định nguồn dữ liệu truy vấn (User) và biến phạm vi (user)
                             Nguồn dữ liệu tập hợp những phần tử thuộc kiểu lớp triển khai giao diện IEnumrable
                                                Tham chiếu đến từng phần tử trong Users*/
                             /*SELECT: chỉ ra các dữ liệu được xuất ra từ nguồn */
                             {
                                 UserId = user.Id,
                                 Username = user.UserName,
                                 Emailaddress = user.Email,
                                 Phonenumber = user.PhoneNumber,
                                 RoleName = (from userRole in user.Roles // lấy từ user.role
                                                 // nối role với Roles trong context bằng roleid
                                             join role in _context.Roles
                                             //ON: chỉ ra sự ràng buộc giữa các phần tử
                                             on userRole.RoleId
                                             //EQUALS: chỉ ra căn cứ vs ràng buộc (userRole.RoleId ~~ role.Id)
                                             equals role.Id
                                             // chọn name Role đưa ra list
                                             select role.Name).ToList()
                             }
                             ).ToList().Select(p => new UserInRoles()    // ViewModel
                             {
                                 UserId = p.UserId,
                                 Username = p.Username,
                                 Email = p.Emailaddress,
                                 setPhone = p.Phonenumber,
                                 Role = string.Join(",", p.RoleName)  //  nối list roleName bởi dấu ,

                             }
                                                );
            return View(useradmin);
        }



        //Edit admin role
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            // Find and assign the Id value in the Users table to userInDb
            var userInDb = _context.Users.SingleOrDefault(u => u.Id == id);
            if (userInDb == null)
            {
                return HttpNotFound();
            }

            return View(userInDb);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(ApplicationUser user)
        {
            // Check the value of Id
            if (!ModelState.IsValid)
            {
                return View();
            }
            var usernameExist = _context.Users.Any(u => u.UserName.Contains(user.UserName));
            //  var EmailIsExist = _context.Users.Any(u => u.Email.Contains(user.Email));
            if (usernameExist)
            {
                ModelState.AddModelError("UserName", "Username existed");
                return View();
            }
            //  else if (EmailIsExist)
            //  {
            //      ModelState.AddModelError("Email", " Email existed");
            //      return View();
            // }
            var userInDb = _context.Users.SingleOrDefault(u => u.Id == user.Id);

            if (userInDb == null)
            {
                return HttpNotFound();
            }
            userInDb.UserName = user.UserName;
            userInDb.Email = user.Email;
            userInDb.PhoneNumber = user.PhoneNumber;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        //Delete admin role
        [HttpGet]
        [Authorize (Roles ="Admin")]
        public ActionResult Delete(string id)
        {
            // Find and assign the Id value in the Users table to accountInDb
            var accountInDb = _context.Users.SingleOrDefault(p => p.Id == id);
            if (accountInDb == null)
            {
                return HttpNotFound();
            }
            _context.Users.Remove(accountInDb);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }




        //Reset pass for account to pass default
        [Authorize(Roles = "Admin")]
        public ActionResult ResetPass(string id)
        {
            var accountInDb = _context.Users.SingleOrDefault(p => p.Id == id);

            if (accountInDb == null)
            {
                return HttpNotFound();
            }

      
            var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            userId = accountInDb.Id;
            if (userId != null)
            {

                UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>());


                userManager.RemovePassword(userId);
                String newPassword = "abc123@";
                userManager.AddPassword(userId, newPassword);
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
