using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DemoDotNet5.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DemoDotNet5.Areas.Identity.Pages.Role
{
    public class CreateModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public CreateModel(RoleManager<IdentityRole> roleManager)
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
        public InputModel Input { set; get; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var newRole = new IdentityRole(Input.Name);
            var result = await _roleManager.CreateAsync(newRole);
            if (result.Succeeded)
            {
                return RedirectToAction("./Index");
            }
            return Page();
        }
    }
}
