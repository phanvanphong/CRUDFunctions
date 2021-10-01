using DemoDotNet5.Data;
using DemoDotNet5.Models;
using DemoDotNet5.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DemoDotNet5.Controllers
{
   [Authorize]
    public class ProductController : Controller
    {
        private readonly EShopDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(EShopDbContext db,IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            this._hostEnvironment = hostEnvironment;
        }


        public IActionResult Index(string SearchString)
        {
            var data = from p in _db.Products
                       join c in _db.Categories on p.CategoryId equals c.Id
                       select new ProductViewModel
                       {
                           Id = p.Id,
                           Name = p.Name,
                           ImageName = p.Image,
                           Price = p.Price,
                           CategoryName = c.Name
                       };
            if (!string.IsNullOrEmpty(SearchString))
            {
                return View(data.Where(a => a.Name.Contains(SearchString)).ToList());
            }
            return View(data.ToList());
        }


        public IActionResult Create() {
            ViewBag.cats = _db.Categories.ToList();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductViewModel obj)
        {
            if (ModelState.IsValid)
            {   
                string fileName = null;
                if (obj.FileUpload != null)
                {
                    // Tạo filename với chuỗi ký tự sinh ngẫu nhiên + tên fileimage
                    fileName = Guid.NewGuid().ToString() + "-" + obj.FileUpload.FileName;
                    // Tạo đường dẫn lưu trữ ảnh
                    var filePath = Path.Combine(_hostEnvironment.WebRootPath, "images", fileName);
                    // Tạo file lưu trữ và di chuyển ảnh đến thư mục lưu trữ
                    using var fileStream = new FileStream(filePath, FileMode.Create);
                    obj.FileUpload.CopyTo(fileStream);
                }
                var Product = new Product
                {
                    Name = obj.Name,
                    Price = obj.Price,
                    Image = fileName,
                    CategoryId = obj.CategoryId
                };
                _db.Products.Add(Product);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }


        public IActionResult Edit(ProductViewModel obj, int id)
        {
            ViewBag.cats = _db.Categories.ToList();
            if (ModelState.IsValid)
            {
                var data = _db.Products.Find(id);
                obj.Name = data.Name;
                obj.Price = data.Price;
                obj.ImageName = data.Image;
                obj.CategoryId = data.CategoryId;
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditPost(ProductViewModel obj, int id)
        {
            ViewBag.cats = _db.Categories.ToList();
            var data = _db.Products.Find(id);
            if (ModelState.IsValid)
            {
                // Lấy tên hình ảnh
                string fileName = data.Image;
                // Nếu FileEdit != null tức là người dùng chọn image mới để update
                if (obj.FileEdit != null)
                {
                    // Tiến hành lấy thông tin ảnh mới
                    fileName = Guid.NewGuid().ToString() + "-" + obj.FileEdit.FileName;
                    var filePath = Path.Combine(_hostEnvironment.WebRootPath, "images", fileName);
                    using var fileStream = new FileStream(filePath, FileMode.Create);
                    obj.FileEdit.CopyTo(fileStream);
                    data.Image = fileName;
                }
                    // Nếu không thỳ vẫn lấy image cũ
                else
                {
                    data.Image = fileName;
                }
                data.Name = obj.Name;
                data.Price = obj.Price;
                data.CategoryId = obj.CategoryId;
                _db.Products.Update(data);
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
