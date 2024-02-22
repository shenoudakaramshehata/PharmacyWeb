using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using Pharmacy.Data;
using Pharmacy.Models;

namespace Pharmacy.Areas.Admin.Pages.DeliveryOrders
{
    public class DeliveryListModel : PageModel
    {
        private readonly PharmacyContext _context;
        private readonly IToastNotification toastNotification;
        UserManager<ApplicationUser> UserManger;
        public List<Order> orders;
        public bool ArLang { get; set; }

        public DeliveryListModel(PharmacyContext context, IToastNotification toastNotification, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.toastNotification = toastNotification;
            UserManger = userManager;
        }
        public async Task <IActionResult> OnGetAsync()
        {
            try
            {
                var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
                var BrowserCulture = locale.RequestCulture.UICulture.ToString();
                if (BrowserCulture == "en-US")
                {
                    ArLang = false;
                }
                else
                {
                    ArLang = true;
                }
                var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await UserManger.FindByIdAsync(userid);

                var roles = await UserManger.GetRolesAsync(user);
                if(roles[0]== "Delivery")
                {
                    orders = _context.Orders.Where(e => e.DeliveryId == user.EntityId).ToList();
                }
                else
                {
                    
                    return RedirectToPage("../NotFound");

                }
            }
            catch (Exception)
            {
                orders = new List<Order>();

                toastNotification.AddErrorToastMessage("Something went wrong");

            }
            return Page();
        }
    }
}
