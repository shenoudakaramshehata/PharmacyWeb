using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pharmacy.Data;
using Pharmacy.Models;

namespace Pharmacy.Areas.Admin.Pages.FAQs2
{
    public class CreateModel : PageModel
    {
        private readonly PharmacyContext _context;

        [BindProperty]
        public FAQ fAQ { get; set; }
        public CreateModel(PharmacyContext context)
        {
            _context = context;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPost()
        {
            await _context.FAQs.AddAsync(fAQ);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
