using DemoDotNet5.Data;
using DemoDotNet5.Models;
using DemoDotNet5.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoDotNet5.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;

namespace DemoDotNet5.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {

        private readonly EShopDbContext _db;
        public CustomerController(EShopDbContext db)
        {
            _db = db;
        }
        public IActionResult Index(string SearchString)
        {
            var data = from c in _db.Customers
                       select new CustomerViewModel
                       {
                           Id = c.Id,
                           Username = c.Username,
                           Password = c.Password,
                           Fullname = c.Fullname,
                           Email = c.Email,
                           Address = c.Address,
                           CreatedAt = c.CreatedAt,
                           UserAdd = c.UserAdd
                           
                       };
            if (!string.IsNullOrEmpty(SearchString))
            {
                return View(data.Where(a => a.Username.Contains(SearchString)).ToList());
            }
            return View(data.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CustomerViewModel obj)
        {
            if (ModelState.IsValid)
            {
               
                var Customer = new Customer
                {
                    Fullname = obj.Fullname,
                    Username = obj.Username,
                    Password = obj.Password,
                    Email = obj.Email,
                    Address = obj.Address,
                    CreatedAt = DateTime.Now.ToString("dd/MM/yyyy"),
                    UserAdd = obj.UserAdd

                };
                _db.Customers.Add(Customer);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
           
        }

        public IActionResult Edit(CustomerViewModel obj, int id)
        {
            var data = _db.Customers.Find(id);
            obj.Username = data.Username;
            obj.Password = data.Password;
            obj.Fullname = data.Fullname;
            obj.Email = data.Email;
            obj.Address = data.Address;
            obj.CreatedAt = data.CreatedAt;
            obj.UserAdd = data.UserAdd;
            return View(obj);
        }

        [HttpPost]
        public IActionResult EditPost(CustomerViewModel obj,int id)
        {
            if (ModelState.IsValid)
            {
                var data = _db.Customers.Find(id);
                 data.Username = obj.Username;
                 data.Password = obj.Password;
                 data.Fullname = obj.Fullname;
                 data.Email = obj.Email;
                 data.Address = obj.Address;
                 data.UserAdd = obj.UserAdd;
                 data.CreatedAt = obj.CreatedAt;
                _db.Customers.Update(data);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
            
        }

        public IActionResult Delete(int id)
        {
            var obj = _db.Customers.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Customers.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
