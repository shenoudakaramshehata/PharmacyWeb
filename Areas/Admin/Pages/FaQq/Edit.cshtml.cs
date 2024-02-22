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
    public class EditModel : PageModel
    {
        private readonly PharmacyContext _context;
        public EditModel(PharmacyContext context)
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
            FAQ newfAQ =await _context.FAQs.FindAsync(id);

            newfAQ.AnswerAr = fAQ.AnswerAr;
            newfAQ.QuestionAr = fAQ.QuestionAr;
            newfAQ.QuestionEn = fAQ.QuestionEn;
            newfAQ.AnswerEn = fAQ.AnswerEn;

            _context.SaveChanges();

            return RedirectToPage("Index");

        }

    }
}
