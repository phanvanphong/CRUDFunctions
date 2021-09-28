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
    public class CustomerController : Controller
    {

        private readonly EShopDbContext _db;
        public CustomerController(EShopDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var data = from c in _db.Customers
                       select new CustomerViewModel
                       {
                           Id = c.Id,
                           Username = c.Username,
                           Password = c.Password,
                           Fullname = c.Fullname,
                           Email = c.Email,
                           Address = c.Address
                       };
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
                    Address = obj.Address
                };
                _db.Customers.Add(Customer);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
           
        }

        public IActionResult Edit(int id)
        {
            var obj = _db.Customers.Find(id);
            return View(obj);
        }

        [HttpPost]
        public IActionResult Edit(Customer obj)
        {
            if (ModelState.IsValid)
            {
                _db.Customers.Update(obj);
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
