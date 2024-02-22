using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NToastNotify;
using Pharmacy.Data;
using Pharmacy.Models;

namespace Pharmacy.Areas.Admin.Pages.DeliveryAgent
{
    public class DeleteDeliveryAgentModel : PageModel
    {
        private PharmacyContext _context;
        private readonly IToastNotification _toastNotification;

        public DeleteDeliveryAgentModel(PharmacyContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;

        }
        [BindProperty]
        public Delivery delivery { get; set; }


        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                delivery = await _context.Deliveries.FirstOrDefaultAsync(m => m.DeliveryId == id);
                if (delivery == null)
                {
                    return Redirect("../Error");
                }
            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");
            }



            return Page();
        }




        public async Task<IActionResult> OnPostAsync(int id)
        {
            try
            {

                delivery = await _context.Deliveries.FindAsync(id);
                if (delivery != null)
                {
                    if (_context.Orders.Any(c => c.OrderId == id))
                    {
                        _toastNotification.AddErrorToastMessage("You cannot delete this delivery agent");
                        return Page();

                    }
                    _context.Deliveries.Remove(delivery);
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("Delivery Agent Deleted successfully");
                }
                else
                    return Redirect("../Error");
            }
            catch (Exception)

            {
                _toastNotification.AddErrorToastMessage("Something went wrong");

                return Page();

            }

            return RedirectToPage("DeliveryAgentIndex");
        }
    }
}
