using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Pharmacy.Data;
using Pharmacy.Models;

namespace Pharmacy.Areas.Admin.Pages.DeliveryOrders
{
    public class orderdetailsModel : PageModel
    {
        private PharmacyContext _context;
        private readonly IToastNotification _toastNotification;
        public orderdetailsModel(PharmacyContext context, IToastNotification toastNotification, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _toastNotification = toastNotification;
            UserManger = userManager;
        }
        [BindProperty]
        public Order order { get; set; }
        [BindProperty]
        public PaymentMethod paymentMethod { get; set; }
        [BindProperty]
        public List<OrderItem> orderItem { get; set; }
        UserManager<ApplicationUser> UserManger;
        
        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {

                if (!_context.Orders.Any(c => c.OrderId == id))
                {
                    return Redirect("../Error");
                }
                var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await UserManger.FindByIdAsync(userid);
                order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o=>o.Delivery)
                .FirstOrDefaultAsync(m => m.OrderId == id);
                orderItem = _context.OrderItems.Include(s => s.Item).Where(s => s.OrderId == id).ToList();
                paymentMethod = _context.PaymentMethods.FirstOrDefault(c => c.PaymentMethodId == order.PaymentMethodId);
               
                    if (user.EntityId != order.DeliveryId)
                    {
                        return RedirectToPage("../NotFound");
                    }
                
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something went wrong");
            }
            return Page();

        }
        public IActionResult OnPost(int id)
        {
           
            try
            {
                order = _context.Orders.Include(o => o.Customer)
                .Include(o => o.Delivery).FirstOrDefault(e => e.OrderId == id);
                orderItem = _context.OrderItems.Include(s => s.Item).Where(s => s.OrderId == id).ToList();
                paymentMethod = _context.PaymentMethods.FirstOrDefault(c => c.PaymentMethodId == order.PaymentMethodId);
                order.ispaid = true;
                var Updatedorder = _context.Orders.Attach(order);
                Updatedorder.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }
            catch(Exception e)
            {
                _toastNotification.AddErrorToastMessage("Something went wrong");
            }
            return Page();
        }
    }
}
