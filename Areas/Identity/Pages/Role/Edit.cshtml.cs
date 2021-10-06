using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DemoDotNet5.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DemoDotNet5.Areas.Identity.Pages.Role
{
    [Authorize(Policy="Admin")]
    public class EditModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public EditModel(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public class InputModel
        {
            [Display(Name = "Role Name")]
            [Required]
            [StringLength(256,MinimumLength =3)]
            public string Name { get; set; }
        }
        [BindProperty]
        public InputModel Input { set; get; }
        public async Task<IActionResult> OnGet(string roleId)
        {
            if (roleId == null) return NotFound();
            var role = await _roleManager.FindByIdAsync(roleId);
            if(role != null)
            {
                Input = new InputModel()
                {
                    Name = role.Name
                };
            }
            return Page();

        }
        public async Task<IActionResult> OnPostAsync(string roleId)
        {
            if (roleId == null) return NotFound();
            var role = await _roleManager.FindByIdAsync(roleId);

            if (!ModelState.IsValid)
            {
                return Page();
            }
            role.Name = Input.Name;
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToPage("./Index");
            }
            return Page();
        }
    }
}
