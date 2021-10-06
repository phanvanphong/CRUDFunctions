using AutoMapper;
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
   [Authorize(Policy ="Product")]
    public class ProductController : Controller
    {
        private readonly EShopDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMapper _mapper;

        public ProductController(EShopDbContext db,IWebHostEnvironment hostEnvironment, IMapper mapper)
        {
            _db = db;
            _hostEnvironment = hostEnvironment;
            _mapper = mapper;
        }

        public IActionResult Index(string search)
        {
            var products = from p in _db.Products
                       join c in _db.Categories on p.CategoryId equals c.Id
                       select new ProductViewModel
                       {
                           Id = p.Id,
                           Name = p.Name,
                           ImageName = p.Image,
                           Price = p.Price,
                           CategoryName = c.Name
                       };
            if (!string.IsNullOrEmpty(search))
            {
                var searchProducts = products.Where(a => a.Name.Contains(search));
                return View(searchProducts.ToList());
            }
            return View(products.ToList());
        }


        public IActionResult Create() {
            var categories = _db.Categories;
            var categoryViewModel = _mapper.Map<List<CategoryViewModel>>(categories);
            ViewBag.cats = categoryViewModel;
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


        public IActionResult Edit(int id)
        {
            var categories = _db.Categories;
            var categoryViewModel = _mapper.Map<List<CategoryViewModel>>(categories);
            ViewBag.cats = categoryViewModel;

            var product = _db.Products.Find(id);
            var productViewModel = _mapper.Map<ProductViewModel>(product);
            return View(productViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditPost(ProductViewModel obj, int id)
        {
            var categories = _db.Categories;
            var categoryViewModel = _mapper.Map<List<CategoryViewModel>>(categories);
            ViewBag.categories = categoryViewModel;

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
            var product = _db.Products.Find(id);
            var productViewModel = _mapper.Map<Product>(product);
            _db.Products.Remove(productViewModel);
            _db.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
