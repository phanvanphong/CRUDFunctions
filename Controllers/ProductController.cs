using DemoDotNet5.Data;
using DemoDotNet5.Models;
using DemoDotNet5.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
namespace DemoDotNet5.Controllers
{
    public class ProductController : Controller
    {
        private readonly EShopDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(EShopDbContext db,IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            this._hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            var data = from p in _db.Products
                       join c in _db.Categories on p.CategoryId equals c.Id
                       select new ProductViewModel
                       {
                           Id = p.Id,
                           Name = p.Name,
                           NameImage = p.Image,
                           Price = p.Price,
                           CategoryName = c.Name
                       };
            return View(data.ToList());
        }

        public IActionResult Create()
        {
            ViewBag.cats = _db.Categories.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductViewModel obj)
        {
            if (ModelState.IsValid)
            {
                var stringFileName = UploadFile(obj);
                var Product = new Product
                {
                    Name = obj.Name,
                    Price = obj.Price,
                    Image = stringFileName,
                    CategoryId = obj.CategoryId
                };
                _db.Products.Add(Product);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        // Hàm xử lý thêm hình ảnh
        private string UploadFile(ProductViewModel obj)
        {
            string fileName = null;
            if (obj.Image != null)
            {
                string uploadDir = Path.Combine(_hostEnvironment.WebRootPath, "images");
                fileName = Guid.NewGuid().ToString() + "-" + obj.Image.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using(var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    obj.Image.CopyTo(fileStream);
                }
            }
            return fileName;
        }

        public IActionResult Edit(int id)
        {
            ViewBag.cats = _db.Categories.ToList();
            var data = _db.Products.Find(id);
            return View(data);
        }

        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            ViewBag.cats = _db.Categories.ToList();
            if (ModelState.IsValid)
            {
                _db.Products.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Delete(int id)
        {
            var obj = _db.Products.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Products.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");

        }

    }
}
