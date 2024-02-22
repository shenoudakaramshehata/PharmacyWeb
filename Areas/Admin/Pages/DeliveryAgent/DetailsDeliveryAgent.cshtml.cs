using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Pharmacy.Data;
using Pharmacy.Models;


namespace Pharmacy.Areas.Admin.Pages.DeliveryAgent
{
    public class DetailsDeliveryAgentModel : PageModel
    {
        public Delivery delivery { get; set; }

        private PharmacyContext _context;
        public List<Order>orders { get; set; }

        private readonly IToastNotification _toastNotification;
        public DetailsDeliveryAgentModel(PharmacyContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {

            try
            {
                delivery = await _context.Deliveries.FirstOrDefaultAsync(m => m.DeliveryId == id);

                if (delivery == null)
                {
                    return Redirect("../Error");
                }
                orders = _context.Orders.Where(e => e.DeliveryId == id).ToList();
            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");
            }



            return Page();
        }
    }
}
