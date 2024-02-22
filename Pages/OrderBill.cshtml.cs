using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Data;
using Pharmacy.Models;

namespace Pharmacy.Pages
{
    [Authorize]
    public class OrderBillModel : PageModel
    {
        private PharmacyContext _context;
        public Order order { get; set; }
        UserManager<ApplicationUser> UserManger;

        public List<OrderItem> orderItem { get; set; }
        public PaymentMethod paymentMethod { get; set; }

        public OrderBillModel(PharmacyContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            UserManger = userManager;

        }
        public async Task<IActionResult> OnGetAsync(int orderId)
        {

            try
            {
                var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await UserManger.FindByIdAsync(userid);
                
                order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o=>o.Delivery)
                .FirstOrDefaultAsync(m => m.OrderId == orderId);
                if (user != null) { 
                if (user.EntityName == "Customer" && user.EntityId != 0)
                {
                    if (user.EntityId != order.CustomerId)
                    {
                        return RedirectToPage("NotFound");
                    }
                }
                else if(user.EntityName== "Delivery" && user.EntityId != 0)
                    {
                        if (user.EntityId != order.DeliveryId)
                        {
                            return RedirectToPage("NotFound");
                        }
                    }
                    else
                    {
                        return RedirectToPage("NotFound");
                    }
                }
                
                orderItem = _context.OrderItems.Include(s => s.Item).Where(s => s.OrderId == orderId).ToList();
                paymentMethod = _context.PaymentMethods.FirstOrDefault(c => c.PaymentMethodId == order.PaymentMethodId);

            }
            catch (Exception)
            {
                return Redirect("../Error");
            }
            return Page();

        }
    }
}
