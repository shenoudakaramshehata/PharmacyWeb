using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Pharmacy.Data;
using Pharmacy.Models;

namespace Pharmacy.Areas.Admin.Pages.Orders
{
    public class OrderDetailsModel : PageModel
    {
        private PharmacyContext _context;
        private readonly IToastNotification _toastNotification;
        private readonly IEmailSender _emailSender;
        public static int orderId = 0;

        public OrderDetailsModel(PharmacyContext context, IToastNotification toastNotification, IEmailSender emailSender)
        {
            _context = context;
            _toastNotification = toastNotification;
            _emailSender = emailSender;
            order = new Order();
        }
        [BindProperty]
        public Order order { get; set; }
        public PaymentMethod paymentMethod { get; set; }
        public List<OrderItem> orderItem { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                if (!_context.Orders.Any(c => c.OrderId == id))
                {
                    return Redirect("../Error");
                }
                order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Delivery)
                .FirstOrDefaultAsync(m => m.OrderId == id);
                orderItem = _context.OrderItems.Include(s => s.Item).Where(s => s.OrderId == id).ToList();
                paymentMethod = _context.PaymentMethods.FirstOrDefault(c => c.PaymentMethodId == order.PaymentMethodId);

            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something went wrong");
            }
            return Page();

        }
        public async Task <IActionResult> OnPost(int? id, int?deliveryId)
        {
            if (id != null && deliveryId != null)
            {
                
                try
                {
                    order = _context.Orders.Include(o => o.Customer)
                .Include(o => o.Delivery)
                .FirstOrDefault(m => m.OrderId == id);
                    orderItem = _context.OrderItems.Include(s => s.Item).Where(s => s.OrderId == id).ToList();
                    paymentMethod = _context.PaymentMethods.FirstOrDefault(c => c.PaymentMethodId == order.PaymentMethodId);
                    
                    order.DeliveryId = deliveryId;
                    var delivery = _context.Deliveries.Find(deliveryId);
                    var Updatedorder = _context.Orders.Attach(order);
                    Updatedorder.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
                    var callbackUrl = Url.Page(
                        "/OrderBill",
                        pageHandler: null,
                        values: new { orderId = order.OrderId },
                        "http",
                        "codewarenet-001-site11.dtempurl.com"
                        );
                    if (delivery != null)
                    {
                        await _emailSender.SendEmailAsync(delivery.Email, "Delivery Order",
                      $"Go to Order Information to get customer information <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Order Information</a>.");
                    }
                                     
                    _toastNotification.AddSuccessToastMessage("Delivery Added Successfully and Email sent Successfully ");

                }
                catch (Exception)
                {
                    _toastNotification.AddErrorToastMessage("Something went wrong");
                }
                return Page();
                }
            _toastNotification.AddErrorToastMessage("Something went wrong");
            return Page();
        }


        public async Task<IActionResult> OnPostCashOrder(int id)
        {
            try
            {
                order = order = await _context.Orders
                               .Include(o => o.Customer).Include(o => o.Delivery)
                               .FirstOrDefaultAsync(m => m.OrderId == id);
                orderItem = _context.OrderItems.Include(s => s.Item).Where(s => s.OrderId == id).ToList();
                paymentMethod = _context.PaymentMethods.FirstOrDefault(c => c.PaymentMethodId == order.PaymentMethodId);
                if (order == null)
                {
                    _toastNotification.AddErrorToastMessage("Something went wrong");
                }

                order.ispaid = true;
                _context.Attach(order).State = EntityState.Modified;
                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");
            }
            return Page();
        }
    }
    }

