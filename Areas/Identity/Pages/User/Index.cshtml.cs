using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoDotNet5.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DemoDotNet5.Areas.Identity.Pages.User
{
    [Authorize(Policy = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _useManager;
        public IndexModel(UserManager<ApplicationUser> userManager)
        {
            _useManager = userManager;
        }
        // Tạo lớp UserAndRole kế thừa từ lớp ApplicationUser và tạo thêm thuộc tính mới là RollNames
        // để lấy tên Role hiển thị ra danh sách
        public class UserAndRole : ApplicationUser
        {
            public string RollNames { get; set; }
        }
        public List<UserAndRole> users { set; get; }

        public async Task<IActionResult> OnGet()
        {
          var query =  _useManager.Users.OrderBy(u => u.UserName);
          var query1 = query.Select(u => new UserAndRole()
            {
                Id = u.Id,
                UserName = u.UserName
            });

            users =  query1.ToList();
            foreach(var user in users)
            {
                var roles = await _useManager.GetRolesAsync(user);
                user.RollNames = string.Join(",", roles);
            }
            return Page();
        }
        
      
    }
}
