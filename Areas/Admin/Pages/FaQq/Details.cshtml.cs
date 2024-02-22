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
    public class DetailsModel : PageModel
    {
        private readonly PharmacyContext _context;
        public DetailsModel(PharmacyContext context)
        {
            _context = context;
        }
        [BindProperty]
        public FAQ fAQ { get; set; }
        public void OnGet(int id)
        {
            fAQ = _context.FAQs.Find(id);
        }
    }
}
