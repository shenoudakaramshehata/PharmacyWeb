using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pharmacy.Data;
using Pharmacy.Models;
using Microsoft.EntityFrameworkCore;

namespace Pharmacy.Areas.Admin.Pages.Areas
{
    public class IndexModel : PageModel
    {


        private PharmacyContext _context;
        
        public IndexModel(PharmacyContext context)
        {
            _context = context;
            
        }
      
        public ActionResult OnGet()
        {
            return Page();
        }
    }
}
    