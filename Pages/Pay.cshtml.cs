using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Pharmacy.Data;
using Pharmacy.Models;

namespace Pharmacy.Pages
{
    public class PayModel : PageModel
    {
        private PharmacyContext _context;
        public Order order { get; set; }

        public List<OrderItem> orderItem { get; set; }
        public PaymentMethod paymentMethod { get; set; }
        private readonly IEmailSender _emailSender;
        public HttpClient httpClient { get; set; }

        public PayModel(PharmacyContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
            httpClient = new HttpClient();
        }
        public async Task<IActionResult> OnGetAsync(string Values)
        {
            if (Values != null)
            {
                var model = JsonConvert.DeserializeObject<Order>(Values);
                
                foreach (var orderItem in model.OrderItem)
                {
                    var item = _context.Items.Find(orderItem.ItemId);
                    if (item.Stock <= 0 || item.Stock == null)
                    {
                        return RedirectToPage("SomethingwentError", new { Message = item.ItemTlEn + " is Out of Stock" });

                    }
                    if (item.Stock < orderItem.Qty)
                    {
                        return RedirectToPage("SomethingwentError", new { Message = item.ItemTlEn + " Quantity less than Stock Quantity" });
                    }

                    item.Stock -= orderItem.Qty;
                    var UpdatedItem = _context.Items.Attach(item);
                    UpdatedItem.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }
                model.OrderDate = DateTime.Now;
                if (_context.Orders.ToList().Count()==0)
                {
                    model.OrderSerial = Convert.ToString(1);
                }
                else
                {
                    var maxserial = _context.Orders.ToList().Max(e => Convert.ToInt64(e.OrderSerial));
                    model.OrderSerial = Convert.ToString(maxserial + 1);
                }
                model.Closed = false;
                model.ispaid = false;
                model.DeliveryId = 1;
                try
                {
                    _context.Orders.Add(model);
                    _context.SaveChanges();

                }
                catch (Exception)
                {
                    return RedirectToPage("SomethingwentError");
                }
                var Customer = _context.Customers.Find(model.CustomerId);

                if (model.PaymentMethodId == 2)
                {
                    var requesturl = "https://api.upayments.com/test-payment";
                    var fields = new
                    {
                        merchant_id = "1201",
                        username = "test",
                        password = "test",
                        order_id = model.OrderId,
                        total_price = model.Total,
                        test_mode = 0,
                        CstFName = Customer.CustomerNameEn,
                        CstEmail = Customer.CustomerEmail,
                        CstMobile = Customer.CustomerPhone,
                        api_key = "jtest123",
                        //success_url =/* "https://localhost:44354/success"*/,
                        success_url = "http://codewarenet-001-site17.dtempurl.com/success",
                        //error_url = /*"https://localhost:44354/failed"*/
                        error_url = "http://codewarenet-001-site17.dtempurl.com/failed"

                    };
                    var content = new StringContent(JsonConvert.SerializeObject(fields), Encoding.UTF8, "application/json");
                    var task = httpClient.PostAsync(requesturl, content);
                    var result = await task.Result.Content.ReadAsStringAsync();
                    var paymenturl = JsonConvert.DeserializeObject<paymenturl>(result);
                    if (paymenturl.status == "success")
                    {
                        return Redirect(paymenturl.paymentURL);
                    }
                    else
                    {
                        return RedirectToPage("SomethingwentError", new { Message = paymenturl.error_msg });
                    }
                }


                if (model.PaymentMethodId == 1)
                {
                    if (model.CustomerId != null)
                    {
                        var carts = _context.Carts.Where(e => e.CustomerId == model.CustomerId);
                        _context.Carts.RemoveRange(carts);
                        _context.SaveChanges();
                    }
                    var callbackUrl = Url.Page(
                    "/OrderBill",
                    pageHandler: null,
                    values: new { orderId = model.OrderId },
                    "http",
                    "codewarenet-001-site17.dtempurl.com"
                    );
                    if (Customer != null)
                    {
                        try
                        {
                            await _emailSender.SendEmailAsync(Customer.CustomerEmail, "Your Order Bill",
                      $"Go to your Bill Information page by clicking on this link <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Bill Information</a>.");
                        }

                        catch (Exception)
                        {
                            return RedirectToPage("SomethingwentError");
                        }
                    }
                    return RedirectToPage("Thankyou");
                }
            }

            return RedirectToPage("SomethingwentError");
        }


    }
}
