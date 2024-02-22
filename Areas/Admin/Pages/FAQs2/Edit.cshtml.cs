using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Pharmacy.Data;
using Pharmacy.Models;

namespace Pharmacy.Areas.Admin.Pages.FAQs2
{
    public class EditModel : PageModel
    {
        private readonly PharmacyContext _context;
        public EditModel(PharmacyContext context)
        {
            _context = context;
        }
        [BindProperty]
        public FAQ faq { get; set; }
        public void OnGet(int id)
        {
            faq = _context.FAQs.Find(id);
        }
        public async Task<IActionResult> OnPost(int id)
        {
            FAQ newfAQ = await _context.FAQs.FindAsync(id);

            newfAQ.AnswerAr = faq.AnswerAr;
            newfAQ.QuestionAr = faq.QuestionAr;
            newfAQ.QuestionEn = faq.QuestionEn;
            newfAQ.AnswerEn = faq.AnswerEn;

            _context.SaveChanges();

            return RedirectToPage("Index");

        }
    }
}
