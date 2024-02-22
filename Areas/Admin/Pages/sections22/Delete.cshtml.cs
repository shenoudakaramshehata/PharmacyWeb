using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pharmacy.Data;
using Pharmacy.Models;

namespace Pharmacy.Areas.Admin.Pages.sections22
{
    public class DeleteModel : PageModel
    {

        private readonly PharmacyContext _context;
        public DeleteModel(PharmacyContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Section section { get; set; }
        public void OnGet(int? id)
        {
            section = _context.Sections.Find(id);
        }
        public async Task<IActionResult> OnPost(int id)
        {
            section = _context.Sections.Find(id);

            _context.Sections.Remove(section);

           await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
