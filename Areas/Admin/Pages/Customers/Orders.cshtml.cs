using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Pharmacy.Data;
using Pharmacy.Models;

namespace Pharmacy.Areas.Admin.Pages.Customers
{
    public class OrdersModel : PageModel
    {
        private PharmacyContext _context;
        private readonly IToastNotification _toastNotification;
        public OrdersModel(PharmacyContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }
        [BindProperty]
        public List<Order> orderlist { get; set; }
       [BindProperty]
        public string customerName { get; set; }
       [BindProperty]
        public int customerId { get; set; }
       
        

        public async Task<IActionResult> OnGetAsync(int id)
        {

            try
            {
                var model = _context.Customers.FirstOrDefault(c => c.CustomerId == id);
                if (model == null)
                {
                    return Redirect("../Error");

                }


                orderlist = await _context.Orders.Where(m => m.CustomerId == id).ToListAsync();
                var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
                var BrowserCulture = locale.RequestCulture.UICulture.ToString();
                customerId = id;
                if (BrowserCulture == "en-US")
                    customerName = model.CustomerNameAr;
                else
                    customerName = model.CustomerNameEn;

            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");

            }


            return Page();

           
        }
    }
}
