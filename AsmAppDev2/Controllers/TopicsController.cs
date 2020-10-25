using AsmAppDev2.Models;
using AsmAppDev2.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace AsmAppDev2.Controllers
{
    public class TopicsController : Controller
    {
        private ApplicationDbContext _context;
        public TopicsController()
        {
            _context = new ApplicationDbContext();
        }

        //GET: Topics
        [HttpGet]
        public ActionResult Index(string search)
        {
            var topics = _context.Topics.Include(t => t.Course);
            if (!String.IsNullOrEmpty(search))
            {
                topics = topics.Where(
                    s => s.Name.Contains(search) ||
                    s.Course.Name.Contains(search));
            }
            return View(topics);
        }

        //GET: Create
        [HttpGet]
        public ActionResult Create()
        {
            var viewModel = new CourseTopicViewModel
            {
                Courses = _context.Courses.ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(Topic topic)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var create = new Topic
            {
                Name = topic.Name,
                Description = topic.Description,
                CourseID = topic.CourseID,
            };
            _context.Topics.Add(create);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        //GET: Edit
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var topicInDb = _context.Topics.SingleOrDefault(t => t.ID == id);

            if (topicInDb == null)
            {
                return HttpNotFound();
            }
            var viewModel = new CourseTopicViewModel
            {
                Topic = topicInDb,
                Courses = _context.Courses.ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(Topic topic)
        {
            var topicInDb = _context.Topics.SingleOrDefault(t => t.ID == topic.ID);
            if (topicInDb == null)
            {
                return HttpNotFound();
            }
            var check = _context.Topics.Any(
                c => c.Name.Contains(topic.Name));

            if (check)
            {
                ModelState.AddModelError("Name", "Topic Name Already Exists.");
                return View();
            }
            topicInDb.Name = topic.Name;
            topicInDb.Description = topic.Description;
            topicInDb.CourseID = topic.CourseID;

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        //GET: Delete
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var topicInDb = _context.Topics.SingleOrDefault(t => t.ID == id);
            if (topicInDb == null)
            {
                return HttpNotFound();
            }
            _context.Topics.Remove(topicInDb);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}