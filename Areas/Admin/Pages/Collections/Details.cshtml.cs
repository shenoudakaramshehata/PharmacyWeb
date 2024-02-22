using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Pharmacy.Data;
using Pharmacy.Models;

namespace Pharmacy.Areas.Admin.Pages.Collections
{
    public class DetailsModel : PageModel
    {

        private PharmacyContext _context;


        private readonly IToastNotification _toastNotification;
        public DetailsModel(PharmacyContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }
        [BindProperty]
        public Collection collection { get; set; }




        [BindProperty(SupportsGet = true)]
        public List<int> itemsSelected { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<Item> items { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public String Text { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            
            try
            {
                var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
                var BrowserCulture = locale.RequestCulture.UICulture.ToString();
                if (BrowserCulture == "en-US")
                {
                   
                    Text = "ItemTlEn";
                }
                else
                {
                   
                    Text = "ItemTlAr";
                }
                collection = await _context.Collections.FirstOrDefaultAsync(m => m.CollectionId == id);
                items = _context.Items.ToList();

                if (collection == null)
                {
                    return Redirect("../Error");
                }
                itemsSelected = _context.CollectionItems.Where(c => c.CollectionId == id).Select(c => c.ItemId).ToList();

            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");
            }


            return Page();
        }


    }
}
