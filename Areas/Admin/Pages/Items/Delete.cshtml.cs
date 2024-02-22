using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NToastNotify;
using Pharmacy.Data;
using Pharmacy.Models;

namespace Pharmacy.Areas.Admin.Pages.Items
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
        public Item item { get; set; }


        public async Task<IActionResult> OnGetAsync(int id)
        {
            
            try
            {
                item = await _context.Items.Include(c => c.Category).Include(c => c.Brand).FirstOrDefaultAsync(m => m.ItemId == id);
                if (item == null)
                {
                    return Redirect("../Error");
                }
            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");
            }



            return Page();
        }




        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
           
            try
            {
                item = await _context.Items.Include(c => c.Category).Include(c => c.Brand).FirstOrDefaultAsync(m => m.ItemId == id);

                if (item != null)
                {
                    if (_context.OrderItems.Any(c=>c.ItemId==id)||_context.Carts.Any(c=>c.ItemId==id))
                    {
                        _toastNotification.AddErrorToastMessage("You cannot delete this Item");
                        return Page();
                    }
                    var imgList = _context.ItemImages.Where(c => c.ItemId == id).ToList();
                    var colList = _context.CollectionItems.Where(c => c.ItemId == id).ToList();
                    if (colList.Count>0)
                    {
                        _context.CollectionItems.RemoveRange(colList);
                    } 
                    if (imgList.Count>0)
                    {
                        _context.ItemImages.RemoveRange(imgList);

                    }

                    _context.SaveChanges();
                    _context.Items.Remove(item);
                    await _context.SaveChangesAsync();
                    var ImagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/Item/" + item.ItemPic);
                    if (System.IO.File.Exists(ImagePath))
                    {
                        System.IO.File.Delete(ImagePath);
                    }
                    _toastNotification.AddSuccessToastMessage("Item Deleted successfully");

                }
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
