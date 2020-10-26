using AsmAppDev2.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;


namespace AsmAppDev2.Controllers
{
    public class TraineeUsersController : Controller
    {
        private ApplicationDbContext _context;
        public TraineeUsersController()
        {
            _context = new ApplicationDbContext();
        }



        // GET: Trainees
        [HttpGet]
        public ActionResult Index(string searchString)
        {
            var trainee = _context.TraineeUsers.Include(tp => tp.Trainee);
            if (!String.IsNullOrEmpty(searchString))
            {
                trainee = trainee.Where(
                        s => s.Trainee.UserName.Contains(searchString) ||
                        s.Trainee.Email.Contains(searchString));
                return View(trainee);
            }
            
            if (User.IsInRole("Staff"))
            {
                var assign = _context.TraineeUsers.Include(a => a.Trainee).ToList();
                return View(assign);
            }
            if (User.IsInRole("Trainee"))
            {
                var traineeId = User.Identity.GetUserId();
                var traineeVM = _context.TraineeUsers.Where(te => te.TraineeID == traineeId).ToList();
                return View(traineeVM);
            }
            return View("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult Create()
        {
            //Get Account Trainee
            var userInDb = (from r in _context.Roles where r.Name.Contains("Trainee") select r).FirstOrDefault();
            var users = _context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(userInDb.Id)).ToList();
            var traineeUser = _context.TraineeUsers.ToList();
            var traineeInfo = new TraineeUser
            {
                Trainees = users,

            };
            return View(traineeInfo);
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        public ActionResult Create(TraineeUser traineeUser)
        {
            var traineeInfo = new TraineeUser
            {
                TraineeID = traineeUser.TraineeID,
                Full_Name = traineeUser.Full_Name,
                Education = traineeUser.Education,
                Programming_Language = traineeUser.Programming_Language,
                Experience_Details = traineeUser.Experience_Details,
                Department = traineeUser.Department,
                Phone = traineeUser.Phone
            };
            _context.TraineeUsers.Add(traineeInfo);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult Edit(int id)
        {
            var traineeInDb = _context.TraineeUsers.SingleOrDefault(te => te.ID == id);
            if (traineeInDb == null)
            {
                return HttpNotFound();
            }
            return View(traineeInDb);
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        public ActionResult Edit(TraineeUser traineeUser)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var traineeInDb = _context.TraineeUsers.SingleOrDefault(te => te.ID == traineeUser.ID);
            if (traineeInDb == null)
            {
                return HttpNotFound();
            }
            traineeInDb.TraineeID = traineeUser.TraineeID;
            traineeInDb.Full_Name = traineeUser.Full_Name;
            traineeInDb.Education = traineeUser.Education;
            traineeInDb.Programming_Language = traineeUser.Programming_Language;
            traineeInDb.Experience_Details = traineeUser.Experience_Details;
            traineeInDb.Department = traineeUser.Department;
            traineeInDb.Phone = traineeUser.Phone;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult Delete(int id)
        {
            var traineeInDb = _context.TraineeUsers.SingleOrDefault(te => te.ID == id);
            if (traineeInDb == null)
            {
                return HttpNotFound();
            }
            _context.TraineeUsers.Remove(traineeInDb);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}