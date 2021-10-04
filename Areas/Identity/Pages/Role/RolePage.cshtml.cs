using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoDotNet5.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DemoDotNet5.Areas.Identity.Pages.Role
{
    public class RolePageModel : PageModel
    {
        protected readonly RoleManager<IdentityRole> _roleManager;
        protected readonly EShopDbContext _db;
        public RolePageModel(RoleManager<IdentityRole> roleManager, EShopDbContext eshopDbContext)
        {
            _roleManager = roleManager;
            _db = eshopDbContext;
        }
        public void OnGet()
        {
            
        }
    }
}
