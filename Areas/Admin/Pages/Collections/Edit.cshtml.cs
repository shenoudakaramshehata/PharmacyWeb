using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Pharmacy.Data;
using Pharmacy.Models;

namespace Pharmacy.Areas.Admin.Pages.Collections
{
    public class EditModel : PageModel
    {


        private PharmacyContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;

        public EditModel(PharmacyContext context, IWebHostEnvironment hostEnvironment, IToastNotification toastNotification)
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
        public List<Item> items{ get; set; }
        [BindProperty(SupportsGet = true)]
        public bool ArLang { get; set; }
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
                    ArLang = false;
                    Text = "ItemTlEn";
                }
                else
                {
                    ArLang = true;
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
                var model = _context.Collections.Where(c => c.CollectionId == id).FirstOrDefault();
                if (model==null)
                {
                    return Redirect("../Error");
                }
                string Ids = Request.Form["ItemIds"];
                List<int> ItemsIds = new List<int>();
                if (Ids!="")
                {
                    List<CollectionItem> collectionItemLst = new List<CollectionItem>();
                    ItemsIds = Ids.Split(',').Select(Int32.Parse).ToList();
                    var Lst = _context.CollectionItems.Where(c => c.CollectionId == model.CollectionId).ToList();
                    if (Lst.Count > 0)
                    {
                        _context.CollectionItems.RemoveRange(Lst);
                        _context.SaveChanges();
                    }

                    if (ItemsIds.Count > 0)
                    {
                        foreach (var item in ItemsIds)
                        {
                            var rec = new CollectionItem();
                            rec.CollectionId = id;
                            rec.ItemId = item;
                            collectionItemLst.Add(rec);

                        }
                        _context.CollectionItems.AddRange(collectionItemLst);
                        _context.SaveChanges();

                    }

                }

                model.CollectionSort = collection.CollectionSort;
                model.CollectionTlAr = collection.CollectionTlAr;
                model.CollectionTlEn = collection.CollectionTlEn;
                model.IsActive = collection.IsActive;
                _context.Attach(model).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("collection Edited successfully");

            }
            catch (Exception)
            {
                
               
                    _toastNotification.AddErrorToastMessage("Something went wrong");
               
            }

            return RedirectToPage("./Index");
        }

     
    }
}
