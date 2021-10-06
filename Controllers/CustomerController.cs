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
using AutoMapper;

namespace DemoDotNet5.Controllers
{
    [Authorize(Policy = "Manage")]
    public class CustomerController : Controller
    {

        private readonly EShopDbContext _db;
        private readonly IMapper _mapper;
        public CustomerController(EShopDbContext db,IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public IActionResult Index(string search)
        {
            Object customers = _db.Customers;
            if (!string.IsNullOrEmpty(search))
            {
                customers = _db.Customers.Where(a => a.Username.Contains(search));
            };
            var customerViewModel = _mapper.Map<List<CustomerViewModel>>(customers);
            return View(customerViewModel);
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
                var customer = _mapper.Map<Customer>(obj);
                _db.Customers.Add(customer);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
           
        }

        public IActionResult Edit(int id)
        {
            var customer = _db.Customers.Find(id);
            var customerViewModel = _mapper.Map<CustomerViewModel>(customer);
            return View(customerViewModel);
        }

        [HttpPost]
        public IActionResult EditPost(CustomerViewModel obj)
        {
            if (ModelState.IsValid)
            {
                var customer = _mapper.Map<Customer>(obj);
                _db.Customers.Update(customer);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
            
        }

        public IActionResult Delete(int id)
        {
            var customer = _db.Customers.Find(id);
            var customerViewModel = _mapper.Map<Customer>(customer);
            _db.Customers.Remove(customerViewModel);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
