using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Data;
using Pharmacy.Models;
using Pharmacy.Reports;

namespace Pharmacy.Areas.Admin.Pages.DeliveryOrders
{
    public class InvoiceModel : PageModel
    {
        public PharmacyContext _context { get; }
        public InvoiceModel(PharmacyContext context)
        {
            _context = context;
        }
        public rptNewInvoice Report { get; set; }
        
        public IActionResult OnGet(int id)
            {
            Report = new rptNewInvoice();
            var Invoice = _context.OrderItems.Include(a => a.Item).Include(a => a.Order).ThenInclude(a => a.Customer).Where(e => e.OrderId == id).Select(a => new InvoiceVM
            {
                customername = a.Order.Customer.CustomerNameEn,
                deliveryName = a.Order.Delivery.Title,
                ItemDescription = a.Item.Description,
                ItemPrice = a.ItemPrice,
                ItemTitle = a.Item.ItemTlEn,
                ItemTotal = a.Total,
                OrderSerial = a.Order.OrderSerial
                , Total = a.Order.Total,
                OrderDate = a.Order.OrderDate,
                PaymentTitle = _context.PaymentMethods.Where(c => c.PaymentMethodId == a.Order.PaymentMethodId).FirstOrDefault().PaymentMethodTlEn,
                Qty=a.Qty, 
                CustomerEmail=a.Order.Customer.CustomerEmail,CustomerPhone=a.Order.Customer.CustomerPhone
                ,invoicedate=DateTime.Now
                ,CustomerAddress=a.Order.Customer.CustomerAddress
            }).ToList();
            Report.DataSource = Invoice;
            return Page();

        }
    }
}
