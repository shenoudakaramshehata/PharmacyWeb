using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Pharmacy.Data;
using Pharmacy.Models;

namespace Pharmacy.Areas.Admin.Pages.Customers
{
    public class OrderDetailsModel : PageModel
    {
        private PharmacyContext _context;
        private readonly IToastNotification _toastNotification;
        public OrderDetailsModel(PharmacyContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }
        [BindProperty]
        public Order order { get; set; }
        [BindProperty]
        public PaymentMethod paymentMethod { get; set; }
        [BindProperty]
        public List<OrderItem> orderItem { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                order = await _context.Orders
               .Include(o => o.Customer)
               .Include(o=>o.Delivery)
               .FirstOrDefaultAsync(m => m.OrderId == id);
                if (order==null)
                {
                    return Redirect("../Error");
                }
                orderItem = _context.OrderItems.Include(s => s.Item).Where(s => s.OrderId == id).ToList();
                paymentMethod = _context.PaymentMethods.FirstOrDefault(c => c.PaymentMethodId == order.PaymentMethodId);
            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");

            }




            return Page();
        }
    }
}
