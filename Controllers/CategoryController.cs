using DemoDotNet5.Data;
using DemoDotNet5.Models;
using DemoDotNet5.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoDotNet5.Controllers
{
    public class CategoryController : Controller
    {
        private readonly EShopDbContext _db;

        public CategoryController(EShopDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var data = from c in _db.Categories
                       select new CategoryViewModel
                       {
                           Id = c.Id,
                           Name = c.Name,
                           Desc = c.Desc,
                       };

            return View(data.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryViewModel obj)
        {
            if (ModelState.IsValid)
            {
                var Category = new Category
                {
                    Name = obj.Name,
                    Desc = obj.Desc

                };
                _db.Categories.Add(Category);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int id)
        {
            var obj = _db.Categories.Find(id);
            return View(obj);
        }

        [HttpPost]
        public IActionResult EditPost(Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View();
        }

        public IActionResult Delete(int id)
        {
            var obj = _db.Categories.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
