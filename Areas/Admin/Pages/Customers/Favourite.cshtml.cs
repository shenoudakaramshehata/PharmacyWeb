using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using Pharmacy.Data;

namespace Pharmacy.Areas.Admin.Pages.Customers
{
    public class FavouriteModel : PageModel
    {
        private PharmacyContext _context;


        private readonly IToastNotification _toastNotification;
        public FavouriteModel(PharmacyContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

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
                var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
                var BrowserCulture = locale.RequestCulture.UICulture.ToString();
                customerId = id;
                if (BrowserCulture == "en-US")
                    customerName = model.CustomerNameEn; 
                else
                    customerName = model.CustomerNameAr;

            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");

            }
            


            return Page();


        }
    }
}
