using AsmAppDev2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AsmAppDev2.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AsmAppDev2.Controllers
{
	public class StaffViewModelsController : Controller
	{
		ApplicationDbContext _context;
		public StaffViewModelsController()
		{
			_context = new ApplicationDbContext();
		}
		// GET: StaffViewModels
		public ActionResult Index()
		{
			var traineeRole = (from te in _context.Roles where te.Name.Contains("Trainee") select te).FirstOrDefault();
			var traineeUser = _context.Users.Where(u => u.Roles.Select(us => us.RoleId).Contains(traineeRole.Id)).ToList();
			var traineeUserVM = traineeUser.Select(user => new StaffViewModel
			{
				UserName = user.UserName,
				Email = user.Email,
				RoleName = "Trainee",
				UserID = user.Id
			}).ToList();

			var trainerRole = (from tn in _context.Roles where tn.Name.Contains("Trainer") select tn).FirstOrDefault();
			var trainerUser = _context.Users.Where(u => u.Roles.Select(us => us.RoleId).Contains(trainerRole.Id)).ToList();
			var trainerUserVM = trainerUser.Select(user => new StaffViewModel
			{
				UserName = user.UserName,
				Email = user.Email,
				RoleName = "Trainer",
				UserID = user.Id
			}).ToList();
			var staff = new StaffViewModel { Trainee = traineeUserVM, Trainer = trainerUserVM };
			return View(staff);
		}

		[HttpGet]
		[Authorize(Roles = "Staff")]
		public ActionResult Edit(string id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}
			var appUser = _context.Users.Find(id);
			if (appUser == null)
			{
				return HttpNotFound();
			}
			return View(appUser);
		}

		[HttpPost]
		[Authorize(Roles = "Staff")]
		public ActionResult Edit(ApplicationUser user)
		{
			var userInDb = _context.Users.Find(user.Id);

			if (userInDb == null)
			{
				return View(user);
			}

			if (ModelState.IsValid)
			{

				userInDb.PhoneNumber = user.PhoneNumber;
				userInDb.Email = user.Email;

				_context.Users.AddOrUpdate(userInDb);
				_context.SaveChanges();

				return RedirectToAction("Index");
			}
			return View(user);

		}

		[Authorize(Roles = "Staff")]
		public ActionResult Delete(string id)
		{
			var userInDb = _context.Users.SingleOrDefault(p => p.Id == id);

			if (userInDb == null)
			{
				return HttpNotFound();
			}
			_context.Users.Remove(userInDb);
			_context.SaveChanges();

			return RedirectToAction("Index");
		}
	}
}