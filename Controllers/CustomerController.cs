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
    [Authorize]
    public class CustomerController : Controller
    {

        private readonly EShopDbContext _db;
        private readonly IMapper _mapper;
        public CustomerController(EShopDbContext db,IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        //public IActionResult Index(string SearchString)
        //{

        //    var data = from c in _db.Customers
        //               select new Customer
        //               {
        //                   Id = c.Id,
        //                   Username = c.Username,
        //                   Password = c.Password,
        //                   Fullname = c.Fullname,
        //                   Address = c.Address,
        //                   UserAdd = c.UserAdd,
        //                   CreatedAt = c.CreatedAt
        //               };
        //    // Nếu chuỗi SearchString có giá trị thỳ lấy những phần tử có name chứa chuỗi Searchstring
        //    if (!string.IsNullOrEmpty(SearchString))
        //    {
        //        return View(data.Where(a => a.Username.Contains(SearchString)).ToList());
        //    }
        //    // Không thỳ trả về data.ToList();
        //    return View(data.ToList());
        //}
        public IActionResult Index(string SearchString)
        {
            var tbl_customer = _db.Customers;
            var customerViewModel = _mapper.Map<List<CustomerViewModel>>(tbl_customer);
            if (!string.IsNullOrEmpty(SearchString))
            {
                var searchCustomer = tbl_customer.Where(a => a.Username.Contains(SearchString));
                var searchCustomerViewModel = _mapper.Map<List<CustomerViewModel>>(searchCustomer);
                return View(searchCustomerViewModel);
            };
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
            var tbl_customer = _db.Customers.Find(id);
            var customerViewModel = _mapper.Map<CustomerViewModel>(tbl_customer);
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
            var tbl_customer = _db.Customers.Find(id);
            var customer = _mapper.Map<Customer>(tbl_customer);
            _db.Customers.Remove(customer);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
