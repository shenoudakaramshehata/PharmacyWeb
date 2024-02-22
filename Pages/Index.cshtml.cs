using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Pharmacy.Data;
using Pharmacy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharmacy.Pages
{
    public class IndexModel : PageModel


    {
        private readonly ILogger<IndexModel> _logger;
        private readonly PharmacyContext _context;

        public IndexModel(PharmacyContext context, ILogger<IndexModel> logger)
        {
            _logger = logger;
            _context = context;
        }
        [BindProperty]
        public Newsletter newsletter { get; set; }
        [BindProperty]
        public ContactUs contactUs { get; set; }

        public void OnGet()
        {

        }

        public IActionResult OnPostAddNewsletter()

        {
            if (!ModelState.IsValid)
            {
                return Redirect("./Index");
            }
            try
            {
                newsletter.Date = DateTime.Now;
                _context.Newsletters.Add(newsletter);
                _context.SaveChanges();
            }
            catch (Exception)
            {

                return Redirect("./Index");
            }

            return Redirect("./Index");

        }
        public IActionResult OnPostAddContactUs()
        {
            if (!ModelState.IsValid)
            {
                return Redirect("./Index");
            }
            try
            {
               
                _context.ContactUs.Add(contactUs);
                _context.SaveChanges();
            }
            catch (Exception)
            {

                return Redirect("./Index");

            }

            return Redirect("./Index");

        }
  
    }
}
