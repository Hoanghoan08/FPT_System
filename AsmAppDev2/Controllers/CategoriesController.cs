using AsmAppDev2.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace AsmAppDev2.Controllers
{
	public class CategoriesController : Controller
	{
		private ApplicationDbContext _context;

		public CategoriesController()
		{
			_context = new ApplicationDbContext();
		}
		// GET: Categories
		[HttpGet]
		public ActionResult Index(string searchString)
		{
			var category = _context.Categories.ToList();

			if (!String.IsNullOrEmpty(searchString))
			{
				category = category.FindAll(s => s.Name.Contains(searchString));
			}

			return View(category);
		}

		////GET: View Categories Details
		//public ActionResult Details(int? id)
		//{
		//	if (id == null)
		//	{
		//		return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		//	}
		//	Category category = _context.Categories.Find(id);
		//	if (category == null)
		//	{
		//		return HttpNotFound();
		//	}
		//	return View(category);
		//}

		//GET: Create 
		[HttpGet]
		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Create(Category category)
		{
			if (!ModelState.IsValid)
			{
				return RedirectToAction("Create");
			}
			var check = _context.Categories.Any(
				c => c.Name.Contains(category.Name));
			if (check)
			{
				ModelState.AddModelError("Name", "Already Exists.");
				return View("Create");
			}
			_context.Categories.Add(category);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}

		//GET: Edit
		[HttpGet]
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Category category = _context.Categories.Find(id);
			if (category == null)
			{
				return HttpNotFound();
			}
			return View();
		}

		[HttpPost]
		public ActionResult Edit(Category category)
		{

			if (ModelState.IsValid)
			{
				_context.Entry(category).State = EntityState.Modified;
				_context.SaveChanges();
				return RedirectToAction("Index");
			}
			var check = _context.Categories.Any(
				c => c.Name.Contains(category.Name));

			if (check)
			{
				ModelState.AddModelError("Name", "Already Exists.");
				return View("Index");
			}
			category.Name = category.Name;
			category.Description = category.Description;
			return View(category);
		}

		//GET: Delete
		[HttpGet]
		public ActionResult Delete(int id)
		{
			var categoryInDb = _context.Categories.SingleOrDefault(c => c.ID == id);

			if (categoryInDb == null)
			{
				return HttpNotFound();
			}

			_context.Categories.Remove(categoryInDb);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}