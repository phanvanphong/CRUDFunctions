using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoDotNet5.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DemoDotNet5.Areas.Identity.Pages.Role
{
    public class IndexModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public IndexModel(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public List<IdentityRole> roles { set; get; }
        public async Task<IActionResult> OnGet()
        {
            roles = await _roleManager.Roles.ToListAsync();
            return Page();
        }
        public void OnPost() => RedirectToPage();
    }
}
