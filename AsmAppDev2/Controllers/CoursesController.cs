using AsmAppDev2.Models;
using AsmAppDev2.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;


namespace AsmAppDev2.Controllers
{
    public class CoursesController : Controller
    {
        private ApplicationDbContext _context;

        public CoursesController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Courses
        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult Index(string searchString)
        {
            var courses = _context.Courses.Include(c => c.Category);

            if (!String.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(
                    s => s.Name.Contains(searchString) ||
                    s.Category.Name.Contains(searchString));
            }
            return View(courses);
        }

        //// GET: View Course Details
        //public ActionResult Details(int? id)
        //{
        //  if (id == null)
        //  {
        //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //  }
        //  var course = _context.Courses.Find(id);
        //  if (course == null)
        //  {
        //    return HttpNotFound();
        //  }
        //  return View(course);
        //}

        //GET: Create
        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult Create()
        {
            var viewModel = new CourseCategoryViewModel
            {
                Categories = _context.Categories.ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        public ActionResult Create(Course course)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var check = _context.Courses.Any(
                c => c.Name.Contains(course.Name));

            if (check)
            {
                ModelState.AddModelError("Name", "Already Exists.");
                return RedirectToAction("Index");
            }

            var create = new Course
            {
                Name = course.Name,
                Description = course.Description,
                CategoryID = course.CategoryID,
            };
            _context.Courses.Add(create);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        //GET: Edit
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var courseInDb = _context.Courses.SingleOrDefault(c => c.ID == id);
            if (courseInDb == null)
            {
                return HttpNotFound();
            }
            var viewModel = new CourseCategoryViewModel
            {
                Course = courseInDb,
                Categories = _context.Categories.ToList()
            };
            return View(viewModel);
        }

        [HttpPost]

        public ActionResult Edit(Course course)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var courseInDb = _context.Courses.SingleOrDefault(c => c.ID == course.ID);
            if (courseInDb == null)
            {
                return HttpNotFound();
            }
            var check = _context.Courses.Any(
                c => c.Name.Contains(course.Name));
            if (check)
            {
                ModelState.AddModelError("Name", "Already Exists.");
                return View();
            }
            courseInDb.Name = course.Name;
            courseInDb.Description = course.Description;
            courseInDb.CategoryID = course.CategoryID;

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        //GET: Delete
        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult Delete(int id)
        {
            var courseInDb = _context.Courses.SingleOrDefault(c => c.ID == id);

            if (courseInDb == null)
            {
                return HttpNotFound();
            }
            _context.Courses.Remove(courseInDb);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //  Course course = _context.Courses.Find(id);
        //  _context.Courses.Remove(course);
        //  _context.SaveChanges();
        //  return RedirectToAction("Index");
        //}
    }
}