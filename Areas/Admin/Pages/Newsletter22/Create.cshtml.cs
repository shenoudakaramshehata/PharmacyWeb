using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pharmacy.Data;
using Pharmacy.Models;

namespace Pharmacy.Areas.Admin.Pages.Newsletter22
{
    public class CreateModel : PageModel
    {
        private readonly PharmacyContext _context;
        public CreateModel(PharmacyContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Newsletter newsletter { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            _context.Newsletters.Add(newsletter);

            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}
