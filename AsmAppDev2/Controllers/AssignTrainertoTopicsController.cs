using AsmAppDev2.Models;
using AsmAppDev2.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace AsmAppDev2.Controllers
{
    public class AssignTrainertoTopicsController : Controller
    {
        private ApplicationDbContext _context;

        public AssignTrainertoTopicsController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: AssignTrainertoTopics
        [HttpGet]
        public ActionResult Index()
        {
            if (User.IsInRole("Staff"))
            {
                var assign = _context.AssignTrainertoTopics.Include(a => a.Topic).Include(a => a.Trainer).ToList();
                return View(assign);
            }
            if (User.IsInRole("Trainer"))
            {
                var trainerId = User.Identity.GetUserId();
                var trainerVM = _context.AssignTrainertoTopics.Where(te => te.TrainerID == trainerId).Include(te => te.Topic).ToList();
                return View(trainerVM);
            }
            return View("Index");
        }

        //GET: Trainer and Topic
        [HttpGet]
        [Authorize(Roles = "Staff, Trainer")]
        public ActionResult Create()
        {
            var roleInDb = (from r in _context.Roles where r.Name.Contains("Trainer") select r).FirstOrDefault();
            var users = _context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(roleInDb.Id)).ToList();
            var topics = _context.Topics.ToList();
            var viewModel = new AssignTrainertoTopicViewModel
            {
                Topics = topics,
                Trainers = users,
                AssignTrainertoTopic = new AssignTrainertoTopic()
            };
            return View(viewModel);
        }


        [HttpPost]
        [Authorize(Roles = "Staff, Trainer")]
        public ActionResult Create(AssignTrainertoTopicViewModel assign)
        {
            var trainer = (from tn in _context.Roles where tn.Name.Contains("Trainer") select tn).FirstOrDefault();
            var trainerUser = _context.Users.Where(u => u.Roles.Select(us => us.RoleId).Contains(trainer.Id)).ToList();
            var topic = _context.Topics.ToList();
            if (ModelState.IsValid)
            {
                _context.AssignTrainertoTopics.Add(assign.AssignTrainertoTopic);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            var trainertopicVM = new AssignTrainertoTopicViewModel()
            {
                Topics = topic,
                Trainers = trainerUser,
                AssignTrainertoTopic = new AssignTrainertoTopic()
            };
            return View(trainertopicVM);
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult Delete(int id)
        {
            var assignInDb = _context.AssignTrainertoTopics.SingleOrDefault(a => a.ID == id);
            if (assignInDb == null)
            {
                return HttpNotFound();
            }

            _context.AssignTrainertoTopics.Remove(assignInDb);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}