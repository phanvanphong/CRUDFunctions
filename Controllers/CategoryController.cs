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
    [Authorize(Policy ="Manage")]
    public class CategoryController : Controller
    {
        private readonly EShopDbContext _db;
        private readonly IMapper _mapper;

        public CategoryController(EShopDbContext db,IMapper mapper)
        {
            _db = db;
            _mapper = mapper;

        }
 
        public IActionResult Index(string search)
        {
            Object categories = _db.Categories;
            if (!string.IsNullOrEmpty(search))
            {
                 categories = _db.Categories.Where(a => a.Name.Contains(search));
            };
            var categoryViewModel = _mapper.Map<List<CategoryViewModel>>(categories);
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
            var category = _db.Categories.Find(id);
            var categoryViewModel = _mapper.Map<CategoryViewModel>(category);
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
            var category = _db.Categories.Find(id);
            var categoryViewModel = _mapper.Map<Category>(category);
            _db.Categories.Remove(categoryViewModel);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
