using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pharmacy.Data;
using Pharmacy.Models;

namespace Pharmacy.Areas.Admin.Pages.newOrder
{
    public class DetailsModel : PageModel
    {
        private readonly PharmacyContext _context;

        [BindProperty]
        public Order order { get; set; }
        public DetailsModel(PharmacyContext context)
        {
            _context = context;
        }
        public void OnGet(int? id)
        {
            order = _context.Orders.Find(id);
        }
    }
}
