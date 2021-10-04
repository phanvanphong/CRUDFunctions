using AutoMapper;
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
        private readonly IMapper _mapper;

        public CategoryController(EShopDbContext db,IMapper mapper)
        {
            _db = db;
            _mapper = mapper;

        }
        ////public IActionResult Index(string SearchString)
        ////{

        ////    var data = from c in _db.Categories
        ////               select new CategoryViewModel
        ////               {
        ////                   Id = c.Id,
        ////                   Name = c.Name,
        ////                   Desc = c.Desc,
        ////               };
        ////    // Nếu chuỗi SearchString có giá trị thỳ lấy những phần tử có name chứa chuỗi Searchstring
        ////    if (!string.IsNullOrEmpty(SearchString))
        ////    {
        ////        return View(data.Where(a => a.Name.Contains(SearchString)).ToList());
        ////    }
        ////    // Không thỳ trả về data.ToList();
        ////    return View(data.ToList());
        ////}

        public IActionResult Index(string SearchString)
        {
            var tbl_category = _db.Categories;
            var categoryViewModel = _mapper.Map<List<CategoryViewModel>>(tbl_category);
            if (!string.IsNullOrEmpty(SearchString))
            {
                var searchCategory = tbl_category.Where(a => a.Name.Contains(SearchString));
                var searchCategoryViewModel = _mapper.Map<List<CategoryViewModel>>(searchCategory);
                return View(searchCategoryViewModel);
            };
            return View(categoryViewModel);

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
                var category = _mapper.Map<Category>(obj);
                _db.Categories.Add(category);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int id)
        {
            var category_data = _db.Categories.Find(id);
            var categoryViewModel = _mapper.Map<CategoryViewModel>(category_data);
            return View(categoryViewModel);
        }

        [HttpPost]
        public IActionResult EditPost(CategoryViewModel obj)
        {
            if (ModelState.IsValid)
            {
                var category = _mapper.Map<Category>(obj);
                _db.Categories.Update(category);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int id)
        {
            var category_data = _db.Categories.Find(id);
            var category = _mapper.Map<Category>(category_data);
            _db.Categories.Remove(category);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
