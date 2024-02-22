using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NToastNotify;
using Pharmacy.Data;
using Pharmacy.Models;

namespace Pharmacy.Areas.Admin.Pages.Brands
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
        public Brand brand { get; set; }


        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                brand = await _context.Brands.FirstOrDefaultAsync(m => m.BrandId == id);
                if (brand == null)
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
            try
            {
               
                brand = await _context.Brands.FindAsync(id);
                if (brand != null)
                {
                    if (_context.Items.Any(c => c.BrandId == id))
                    {
                        _toastNotification.AddErrorToastMessage("You cannot delete this Brand");
                        return Page();

                    }
                    _context.Brands.Remove(brand);
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("Brand Deleted successfully");
                    var ImagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/Brand/" + brand.BrandPic);
                    if (System.IO.File.Exists(ImagePath))
                    {
                        System.IO.File.Delete(ImagePath);
                    }
                }
                else
                    return Redirect("../Error");
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
