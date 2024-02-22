using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NToastNotify;
using Pharmacy.Data;
using Pharmacy.Models;

namespace Pharmacy.Areas.Admin.Pages.Collections2
{
    public class DeleteModel : PageModel
    {
        private PharmacyContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;

        public DeleteModel(PharmacyContext context, IWebHostEnvironment hostEnvironment, IToastNotification toastNotification)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
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

        public async Task<IActionResult> OnPostAsync(int id)
        {
            try
            {
                collection = await _context.Collections.FindAsync(id);
                if (collection == null)
                {
                    return Redirect("../Error");
                }
                var Lst = _context.CollectionItems.Where(c => c.CollectionId == collection.CollectionId).ToList();
                if (Lst.Count > 0)
                {
                    _context.CollectionItems.RemoveRange(Lst);
                    _context.SaveChanges();
                }
                _context.Collections.Remove(collection);
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("collection Deleted successfully");
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something went wrong");
                return Page();
            }
            return RedirectToPage("./Index");
        }
    }
}
