using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DemoDotNet5.Areas.Identity.Data;
using DemoDotNet5.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DemoDotNet5.Areas.Identity.Pages.User
{
    [Authorize(Policy = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _useManager;
        private readonly SignInManager<ApplicationUser> _signManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CreateModel(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            _useManager = userManager;
            _signManager = signInManager;
            _roleManager = roleManager;
        }
        public SelectList allRoles { get; set; }
        public ApplicationUser user { get; set; }
        [BindProperty]
        public string[] RoleNames { get; set; }
       
        public async Task<IActionResult> OnGetAsync(string id)
        {
            user = await _useManager.FindByIdAsync(id);
            RoleNames = (await _useManager.GetRolesAsync(user)).ToArray<string>();
            List<string> roleNames =  _roleManager.Roles.Select(r => r.Name).ToList();
            allRoles = new SelectList(roleNames);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            // Tìm user theo id
            user = await _useManager.FindByIdAsync(id);
            // Lấy tất cả những role mà user đang có và tạo thành 1 mảng
            var oldRoleNames = (await _useManager.GetRolesAsync(user)).ToArray();
            // Lấy tất cả những role cần phải xóa (có trong oldRoleNames mà không có trong RoleName)
            var deleteRoles = oldRoleNames.Where(r => !RoleNames.Contains(r));
            // Lấy tất cả những role có trong role Name mà không có ở trong oldRoleNames
            // tức là những role cần thêm vào
            var addRoles = RoleNames.Where(r => !oldRoleNames.Contains(r));
            
            var resultDelete = await _useManager.RemoveFromRolesAsync(user, deleteRoles);
            var resultAdd = await _useManager.AddToRolesAsync(user, addRoles);

            // Nạp lại danh sách các role
            List<string> roleNames = _roleManager.Roles.Select(r => r.Name).ToList();
            allRoles = new SelectList(roleNames);

            return RedirectToPage("./Index");
        }
    }
}
