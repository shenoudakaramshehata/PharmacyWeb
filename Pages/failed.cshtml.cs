using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pharmacy.Data;
using Pharmacy.Models;

namespace Pharmacy.Pages
{
    public class failedModel : PageModel
    {
        private PharmacyContext _context;
        public Order order { get; set; }

        private readonly IEmailSender _emailSender;

        public failedModel(PharmacyContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }
        public void OnGet(string payment_type, string PaymentID, string Result, int OrderID, DateTime? PostDate, string TranID,
        string Ref, string TrackID, string Auth)
        {
            if (OrderID != 0)
            {
                order = _context.Orders.FirstOrDefault(e => e.OrderId == OrderID);
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
        }
    }
}
