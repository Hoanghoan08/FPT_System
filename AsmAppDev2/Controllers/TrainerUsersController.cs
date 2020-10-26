using AsmAppDev2.Models;
using AsmAppDev2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace AsmAppDev2.Controllers
{
	public class TrainerUsersController : Controller
	{
		private ApplicationDbContext _context;
		public TrainerUsersController()
		{
			_context = new ApplicationDbContext();
		}

		// GET: Trainees
		[HttpGet]
		public ActionResult Index()
		{
			if (User.IsInRole("Staff"))
			{
				var assign = _context.TrainerUsers.Include(a => a.Trainer).ToList();
				return View(assign);
			}
			if (User.IsInRole("Trainer"))
			{
				var trainerId = User.Identity.GetUserId();
				var trainerVM = _context.TrainerUsers.Where(te => te.TrainerID == trainerId).ToList();
				return View(trainerVM);
			}
			return View("Index");
		}

		[HttpGet]
		[Authorize(Roles = "Staff")]
		public ActionResult Create()
		{
			//Get Account Trainer
			var userInDb = (from r in _context.Roles where r.Name.Contains("Trainer") select r).FirstOrDefault();
			var users = _context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(userInDb.Id)).ToList();
			var trainerUser = _context.TrainerUsers.ToList();

			var trainerInfo = new TrainerUser
			{
				Trainers = users,

			};
			return View(trainerInfo);
		}

		[HttpPost]
		[Authorize(Roles = "Staff")]
		public ActionResult Create(TrainerUser trainerUser)
		{
			var trainerInfo = new TrainerUser
			{
				TrainerID = trainerUser.TrainerID,
				Full_Name = trainerUser.Full_Name,
				Working_Place = trainerUser.Working_Place,
				Phone = trainerUser.Phone
			};

			_context.TrainerUsers.Add(trainerInfo);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}



		[HttpGet]
		[Authorize(Roles = "Staff")]
		public ActionResult Edit(int id)
		{
			var trainerInDb = _context.TrainerUsers.SingleOrDefault(tn => tn.ID == id);
			if (trainerInDb == null)
			{
				return HttpNotFound();
			}
			return View(trainerInDb);
		}

		[HttpPost]
		[Authorize(Roles = "Staff")]
		public ActionResult Edit(TrainerUser trainerUser)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}
			var trainerInDb = _context.TrainerUsers.SingleOrDefault(tn => tn.ID == trainerUser.ID);
			if (trainerInDb == null)
			{
				return HttpNotFound();
			}
			trainerInDb.TrainerID = trainerUser.TrainerID;
			trainerInDb.Full_Name = trainerUser.Full_Name;
			trainerInDb.Working_Place = trainerUser.Working_Place;
			trainerInDb.Phone = trainerUser.Phone;

			_context.SaveChanges();
			return RedirectToAction("Index");
		}



		[HttpGet]
		[Authorize(Roles = "Staff")]
		public ActionResult Delete(int id)
		{
			var trainerInDb = _context.TrainerUsers.SingleOrDefault(tn => tn.ID == id);
			if (trainerInDb == null)
			{
				return HttpNotFound();
			}
			_context.TrainerUsers.Remove(trainerInDb);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}