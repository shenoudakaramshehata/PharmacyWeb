using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Data;
using Pharmacy.Models;

namespace Pharmacy.Areas.Admin.Pages.Newsletter22
{
    public class EditModel : PageModel
    {
        private readonly PharmacyContext _context;
        public EditModel(PharmacyContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Newsletter newsletter { get; set; }
        public void OnGet(int? id)
        {
            newsletter = _context.Newsletters.Find(id);
        }

        public async Task<IActionResult> OnPost(int id)
        {
            Newsletter newl= _context.Newsletters.Find(id);

            newl.Email = newsletter.Email;
            newl.Date = newsletter.Date;
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}

