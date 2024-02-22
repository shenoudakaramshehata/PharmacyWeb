using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using Pharmacy.Data;
using Pharmacy.Models;

namespace Pharmacy.Areas.Admin.Pages.Collections
{
    public class AddModel : PageModel
    {
        private PharmacyContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;

        public AddModel(PharmacyContext context, IWebHostEnvironment hostEnvironment, IToastNotification toastNotification)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;

        }

        [BindProperty(SupportsGet = true)]
        public List<Item> items { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool ArLang { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Text { get; set; }
       
        public void OnGet()
        {
            try
            {
                var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
                var BrowserCulture = locale.RequestCulture.UICulture.ToString();
                if (BrowserCulture == "en-US")
                {
                    ArLang = false;
                    Text = "ItemTlEn";
                }
                else
                {
                    ArLang = true;
                    Text = "ItemTlAr";
                }

                items = _context.Items.ToList();
            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");

            }


        }
        public IActionResult OnPost(Collection model)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            try
            {
                string Ids = Request.Form["ItemIds"];
                List<int> ItemsIds = new List<int>();
                List<CollectionItem> collectionItemLst = new List<CollectionItem>();
                ItemsIds = Ids.Split(',').Select(Int32.Parse).ToList();
                _context.Collections.Add(model);
                _context.SaveChanges();

                if (model.CollectionId>0)
                {
                    foreach (var item in ItemsIds)
                    {
                        var rec = new CollectionItem();
                        rec.CollectionId = model.CollectionId;
                        rec.ItemId = item;
                        collectionItemLst.Add(rec);

                    }
                    _context.CollectionItems.AddRange(collectionItemLst);


                    _context.SaveChanges();

                }
                _toastNotification.AddSuccessToastMessage("Collection Added successfully");

            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");
            }
          
            return Redirect("./Index");

        }
    }
}
