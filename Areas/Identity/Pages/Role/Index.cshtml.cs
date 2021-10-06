using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoDotNet5.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DemoDotNet5.Areas.Identity.Pages.Role
{
    [Authorize(Policy = "Admin")]
    public class IndexModel : PageModel 
    {   
        private readonly RoleManager<IdentityRole> _roleManager;
        public IndexModel(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        // Tạo ra thuộc tính roles có kiểu là danh sách các IdentityRole
        public List<IdentityRole> roles { set; get; }
        // Phương thức bất đồng bộ OnGet thực hiện lấy tên của các Roles và sắp xếp theo tên
        public async Task<IActionResult> OnGet()
        {
            roles = await _roleManager.Roles.OrderByDescending(r=>r.Name).ToListAsync();
            return Page();
        }
        
    }
}
