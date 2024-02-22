using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pharmacy.Data;

namespace Pharmacy.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly PharmacyContext _context;
        public int revenues { get; set; }

        public IndexModel(PharmacyContext context)
        {
            _context = context;
        }
        
        public void OnGet()
        {
            var sum= _context.Orders.Select(c => c.Total).Sum();
            revenues= Convert.ToInt32(sum);


        }
    }
}
