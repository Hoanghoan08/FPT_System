using AsmAppDev2.Models;
using AsmAppDev2.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace AsmAppDev2.Controllers
{
    public class AssignTraineetoCoursesController : Controller
    {
        private ApplicationDbContext _context;
        public AssignTraineetoCoursesController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpGet]
        // GET: AssignTraineetoCourses
        public ActionResult Index()
        {
            if (User.IsInRole("Staff"))
            {
                var assign = _context.AssignTraineetoCourses.Include(a => a.Course).Include(a => a.Trainee).ToList();
                return View(assign);
            }
            if (User.IsInRole("Trainee"))
            {
                var traineeId = User.Identity.GetUserId();
                var traineeVM = _context.AssignTraineetoCourses.Where(te => te.TraineeID == traineeId).Include(te => te.Course).ToList();
                return View(traineeVM);
            }
            return View();
        }

        //GET: Trainee and Course
        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult Create()
        {
            var traineeInDb = (from r in _context.Roles where r.Name.Contains("Trainee") select r).FirstOrDefault();
            var user = _context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(traineeInDb.Id)).ToList();
            var courses = _context.Courses.ToList();
            var viewModel = new AssignTraineetoCourseViewModel
            {
                Courses = courses,
                Trainees = user,
                AssignTraineetoCourse = new AssignTraineetoCourse()
            };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        public ActionResult Create(AssignTraineetoCourseViewModel assign)
        {
            var trainee = (from te in _context.Roles where te.Name.Contains("Trainee") select te).FirstOrDefault();
            var traineeUser = _context.Users.Where(u => u.Roles.Select(us => us.RoleId).Contains(trainee.Id)).ToList();

            var course = _context.Courses.ToList();

            if (ModelState.IsValid)
            {
                _context.AssignTraineetoCourses.Add(assign.AssignTraineetoCourse);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            var traineecourseVM = new AssignTraineetoCourseViewModel()
            {
                Courses = course,
                Trainees = traineeUser,
                AssignTraineetoCourse = new AssignTraineetoCourse()
            };
            return View(traineecourseVM);
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult Delete(int id)
        {
            var assignInDb = _context.AssignTraineetoCourses.SingleOrDefault(a => a.ID == id);
            if (assignInDb == null)
            {
                return HttpNotFound();
            }

            _context.AssignTraineetoCourses.Remove(assignInDb);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}