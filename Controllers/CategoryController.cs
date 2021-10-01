using DemoDotNet5.Data;
using DemoDotNet5.Models;
using DemoDotNet5.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoDotNet5.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly EShopDbContext _db;

        public CategoryController(EShopDbContext db)
        {
            _db = db;

        }
        public IActionResult Index(string SearchString)
        {
            var data = from c in _db.Categories
                       select new CategoryViewModel
                       {
                           Id = c.Id,
                           Name = c.Name,
                           Desc = c.Desc,
                       };
            // Nếu chuỗi SearchString có giá trị thỳ lấy những phần tử có name chứa chuỗi Searchstring
            if (!string.IsNullOrEmpty(SearchString))
            {
              return View(data.Where(a => a.Name.Contains(SearchString)).ToList());
            }
            // Không thỳ trả về data.ToList();
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

        public IActionResult Edit(CategoryViewModel obj, int id)
        {

            var data = _db.Categories.Find(id);
            obj.Name = data.Name;
            obj.Desc = data.Desc;
            return View(obj);
        }

        [HttpPost]
        public IActionResult EditPost(CategoryViewModel obj, int id)
        {
            if (ModelState.IsValid)
            {
                var data = _db.Categories.Find(id);
                data.Name = obj.Name;
                data.Desc = obj.Desc;
                _db.Categories.Update(data);
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

        //public IActionResult SearchString(string SearchString)
        //{
        //    var data = from c in _db.Categories
        //               select new CategoryViewModel
        //               {
        //                   Id = c.Id,
        //                   Name = c.Name,
        //                   Desc = c.Desc,
        //               };
        //    if (!string.IsNullOrEmpty(SearchString))
        //    {
        //        data.Where(a => a.Name.Contains(SearchString)).ToList();
        //    }
        //    else
        //    {
        //        data.ToList();
        //    }
        //    return RedirectToAction("Index");
        //}
    }
}
