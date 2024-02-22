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
    public class DeleteModel : PageModel
    {
        private readonly PharmacyContext _context;
        public DeleteModel(PharmacyContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Newsletter newsletter { get; set; }
        public void OnGet(int? id)
        {
            newsletter = _context.Newsletters.Find(id);
        }
        public async Task<IActionResult> OnPost(int? id)
        {
            newsletter = _context.Newsletters.Find(id);

            _context.Newsletters.Remove(newsletter);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}
