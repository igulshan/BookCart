using BookCartWeb.Data;
using BookCartWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BookCartWeb.Controllers
{
	public class CategoryController : Controller
	{
		private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
			_db = db;
        }
        public IActionResult Index()
		{
			List<Category> ls = _db.Categories.OrderBy(x=>x.DisplayOrder).ToList();
			return View(ls);
		}


		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(Category obj)
		{
			if(obj.Name == obj.DisplayOrder.ToString())
			{
				ModelState.AddModelError("Name", "Name & Display Order can't be same");
			}
			if (ModelState.IsValid)
			{
				_db.Categories.Add(obj);
				_db.SaveChanges();
				TempData["success"] = "Category Successfully Created";
				return RedirectToAction("Index");
			}
			return View();
			
		}

		public IActionResult Edit(int? Id)
		{
			if(Id==null || Id == 0)
			{
				return NotFound();
			}

			Category? selectedCategory = _db.Categories.FirstOrDefault(x => x.Id == Id);
			if(selectedCategory is null)
			{
				return NotFound();
			}
			return View(selectedCategory);
		}

		[HttpPost]
		public IActionResult Edit(Category obj)
		{
			if (obj.Name == obj.DisplayOrder.ToString())
			{
				ModelState.AddModelError("Name", "Name & Display Order can't be same");
			}
			if (ModelState.IsValid)
			{
				_db.Categories.Update(obj);
				_db.SaveChanges();
				TempData["success"] = "Category Successfully Updated";
				return RedirectToAction("Index");
			}
			return View();

		}

		public IActionResult Delete(int? Id)
		{
			if (Id is null || Id == 0)
			{
				return NotFound();
			}
			Category? delCategory = _db.Categories.Find(Id);
			if(delCategory is null)
			{
				return NotFound();
			}
			return View(delCategory);
		}

		[HttpPost , ActionName("Delete")]
		public IActionResult DeleteCategory(int? Id)
		{
			if(Id is null || Id == 0)
			{
				return NotFound();
			}
			Category? delCategory = _db.Categories.Find(Id);
			if (delCategory is not null)
			{
				_db.Categories.Remove(delCategory);
				_db.SaveChanges();
				TempData["success"] = "Category Successfully Deleted";
			}
			return RedirectToAction("Index");
		}
	}
}
