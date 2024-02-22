using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pharmacy.Data;
using Pharmacy.Models;

namespace Pharmacy.Areas.Admin.Pages.FaQq
{
    public class DeleteModel : PageModel
    {
        private readonly PharmacyContext _context;
        public DeleteModel(PharmacyContext context)
        {
            _context = context;
        }
        [BindProperty]
        public FAQ fAQ { get; set; }
        public void OnGet(int id)
        {
            fAQ = _context.FAQs.Find(id);
        }
        public async Task<IActionResult> OnPost(int id)
        {
            fAQ = _context.FAQs.Find(id);

            _context.FAQs.Remove(fAQ);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
