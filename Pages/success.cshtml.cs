using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DevExtreme.AspNet.Mvc.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MimeKit;
using Pharmacy.Data;
using Pharmacy.Models;
using Pharmacy.Services;
using Pharmacy.ViewModels;

namespace Pharmacy.Pages
{
    public class successModel : PageModel
    {
        private PharmacyContext _context;
        public Order order { get; set; }

        private readonly IEmailSender _emailSender;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
        public InvoiceVm invoiceVm { get; set; }
        public Customer Customer { get; set; }
        public Delivery delivery { get; set; }

        private readonly IRazorPartialToStringRenderer _renderer;
        public successModel(IRazorPartialToStringRenderer renderer, Microsoft.AspNetCore.Hosting.IHostingEnvironment env, PharmacyContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
            _env = env;
            _renderer = renderer;
        }
        public async Task<IActionResult> OnGetAsync(string payment_type, string PaymentID, string Result, int OrderID, DateTime? PostDate, string TranID,
        string Ref, string TrackID, string Auth)
        {
            if (OrderID != 0)
            {
                order = _context.Orders.Include(e => e.OrderItem).ThenInclude(e => e.Item).FirstOrDefault(e => e.OrderId == OrderID);

                order.ispaid = true;
                order.payment_type = payment_type;
                order.PaymentID = PaymentID;
                order.Result = Result;
                order.OrderId = OrderID;
                order.PostDate = PostDate;
                order.TranID = TranID;
                order.Ref = Ref;
                order.TrackID = TrackID;
                order.Auth = Auth;
                var UpdatedOrder = _context.Orders.Attach(order);
                UpdatedOrder.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                foreach (var orderItem in order.OrderItem)
                {
                    var item = _context.Items.Find(orderItem.ItemId);
                    item.Stock -= orderItem.Qty;
                    var UpdatedItem = _context.Items.Attach(item);
                    UpdatedItem.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();

                }
                Customer = _context.Customers.Find(order.CustomerId);
                if (order.CustomerId != null)
                {
                    var carts = _context.Carts.Where(e => e.CustomerId == order.CustomerId);
                    _context.Carts.RemoveRange(carts);
                    _context.SaveChanges();
                }
                delivery = _context.Deliveries.Find(order.DeliveryId);





                //var callbackUrl = Url.Page(
                //    "/OrderBill",
                //    pageHandler: null,
                //    values: new { orderId = order.OrderId },
                //    "http",
                //    "codewarenet-001-site17.dtempurl.com"
                //    );
                if (delivery != null)
                {
                    try
                    {
                        var webRoot = _env.WebRootPath;

                        var pathToFile = _env.WebRootPath
                               + Path.DirectorySeparatorChar.ToString()
                               + "Templates"
                               + Path.DirectorySeparatorChar.ToString()
                               + "EmailTemplate"
                               + Path.DirectorySeparatorChar.ToString()
                               + "EmailTemplate.html";
                        var builder = new BodyBuilder();
                        using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                        {

                            builder.HtmlBody = SourceReader.ReadToEnd();

                        }
                        //string messageBody = string.Format(builder.HtmlBody,
                        //   shoppingCost,
                        //   order.OrderDiscount,
                        //   order.OrderNet,
                        //   Customer.CustomerName,
                        //   order.OrderTotal,
                        //   order.OrderSerial

                        //   );

                        invoiceVm = _context.Orders.Include(a => a.Customer).Where(e => e.OrderId == order.OrderId).Select(i => new InvoiceVm
                        {
                            OrderId = i.OrderId,
                            OrderDate = i.OrderDate.Value.Date.Year + " , " + i.OrderDate.Value.Date.Month + " , " + i.OrderDate.Value.Date.Day,
                            OrderTime = i.OrderDate.Value.TimeOfDay.Hours + " : " + i.OrderDate.Value.TimeOfDay.Minutes,
                            CusName = _context.Customers.Where(e => e.CustomerId == i.CustomerId).FirstOrDefault().CustomerNameEn,
                            CusAddress = i.Customer.CustomerAddress,
                            NetOrder = i.Total.Value + i.DeliveryCost,
                            OrderTotal = i.Total.Value,
                            //InvoiceNumber = i.OrderSerial.Value,
                            WebSite = $"{this.Request.Scheme}://{this.Request.Host}",
                            SuppEmail = _context.Configurations.FirstOrDefault().Email,
                            ConntactNumber = _context.Configurations.FirstOrDefault().Phone,
                            Facebook = _context.Configurations.FirstOrDefault().Facebook,
                            whatsapp = _context.Configurations.FirstOrDefault().WhatsApp,
                            Instgram = _context.Configurations.FirstOrDefault().Instgram,
                            ShippingCost = i.DeliveryCost,
                            ShippingAddress = i.Customer.CustomerAddress,
                            //Address = i.CustomerAddress.Address,
                            ShippingAddressPhone = i.Customer.CustomerPhone,
                            PaymentTit = _context.PaymentMethods.Where(e => e.PaymentMethodId == i.PaymentMethodId).FirstOrDefault().PaymentMethodTlEn,
                            //currencyName = _context.Currency.Where(e => e.CurrencyId == i.Country.CurrencyId).FirstOrDefault().CurrencyTlen

                        }).FirstOrDefault();
                        if (invoiceVm == null)
                        {
                            return RedirectToPage("SomethingwentError");
                        }
                        else
                        {
                            var orderItemVm = _context.OrderItems.Include(e => e.Item).Where(e => e.OrderId == invoiceVm.OrderId).Select(i => new OrderItemVm
                            {
                                ItemImage = i.Item.ItemPic,
                                ItemPrice = i.ItemPrice.Value,
                                ItemQuantity = i.Qty.Value,
                                ItemTitleEn = i.Item.ItemTlEn,
                                Total = i.Total.Value,
                                ItemDis = i.Item.Description
                            }).ToList();
                            invoiceVm.orderItemVms = orderItemVm;
                        }



                        var messageBody = await _renderer.RenderPartialToStringAsync("_Invoice", invoiceVm);

                        await _emailSender.SendEmailAsync(delivery.Email, "Order Details", messageBody);
                        //  await _emailSender.SendEmailAsync(delivery.Email, "Delivery Order",
                        //$"Go to Order Information to get customer information <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Order Information</a>.");

                    }
                    catch (Exception)
                    {
                        return RedirectToPage("SomethingwentError");
                    }
                }
                if (Customer != null)
                {
                    try
                    {
                        var webRoot = _env.WebRootPath;

                        var pathToFile = _env.WebRootPath
                               + Path.DirectorySeparatorChar.ToString()
                               + "Templates"
                               + Path.DirectorySeparatorChar.ToString()
                               + "EmailTemplate"
                               + Path.DirectorySeparatorChar.ToString()
                               + "EmailTemplate.html";
                        var builder = new BodyBuilder();
                        using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                        {

                            builder.HtmlBody = SourceReader.ReadToEnd();

                        }
                        //string messageBody = string.Format(builder.HtmlBody,
                        //   shoppingCost,
                        //   order.OrderDiscount,
                        //   order.OrderNet,
                        //   Customer.CustomerName,
                        //   order.OrderTotal,
                        //   order.OrderSerial

                        //   );

                        invoiceVm = _context.Orders.Include(a => a.Customer).Where(e => e.OrderId == order.OrderId).Select(i => new InvoiceVm
                        {
                            OrderId = i.OrderId,
                            OrderDate = i.OrderDate.Value.Date.Year + " , " + i.OrderDate.Value.Date.Month + " , " + i.OrderDate.Value.Date.Day,
                            OrderTime = i.OrderDate.Value.TimeOfDay.Hours + " : " + i.OrderDate.Value.TimeOfDay.Minutes,
                            CusName = _context.Customers.Where(e => e.CustomerId == i.CustomerId).FirstOrDefault().CustomerNameEn,
                            CusAddress = i.Customer.CustomerAddress,
                            NetOrder = i.Total.Value + i.DeliveryCost,
                            OrderTotal = i.Total.Value,
                            //InvoiceNumber = i.OrderSerial.Value,
                            WebSite = $"{this.Request.Scheme}://{this.Request.Host}",
                            SuppEmail = _context.Configurations.FirstOrDefault().Email,
                            ConntactNumber = _context.Configurations.FirstOrDefault().Phone,
                            Facebook = _context.Configurations.FirstOrDefault().Facebook,
                            whatsapp = _context.Configurations.FirstOrDefault().WhatsApp,
                            Instgram = _context.Configurations.FirstOrDefault().Instgram,
                            ShippingCost = i.DeliveryCost,
                            ShippingAddress = i.Customer.CustomerAddress,
                            //Address = i.CustomerAddress.Address,
                            ShippingAddressPhone = i.Customer.CustomerPhone,
                            PaymentTit = _context.PaymentMethods.Where(e => e.PaymentMethodId == i.PaymentMethodId).FirstOrDefault().PaymentMethodTlEn,
                            //currencyName = _context.Currency.Where(e => e.CurrencyId == i.Country.CurrencyId).FirstOrDefault().CurrencyTlen

                        }).FirstOrDefault();
                        if (invoiceVm == null)
                        {
                            return RedirectToPage("SomethingwentError");
                        }
                        else
                        {
                            var orderItemVm = _context.OrderItems.Include(e => e.Item).Where(e => e.OrderId == invoiceVm.OrderId).Select(i => new OrderItemVm
                            {
                                ItemImage = i.Item.ItemPic,
                                ItemPrice = i.ItemPrice.Value,
                                ItemQuantity = i.Qty.Value,
                                ItemTitleEn = i.Item.ItemTlEn,
                                Total = i.Total.Value,
                                ItemDis = i.Item.Description
                            }).ToList();
                            invoiceVm.orderItemVms = orderItemVm;
                        }



                        var messageBody = await _renderer.RenderPartialToStringAsync("_Invoice", invoiceVm);

                        await _emailSender.SendEmailAsync(Customer.CustomerEmail, "Order Details", messageBody);
                    }

                    catch (Exception)
                    {
                        return RedirectToPage("SomethingwentError");
                    }
                }
            }
            return Page();

        }
    }
}
