using System;
using System.Collections.Generic;
using System.IO;
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

namespace Pharmacy.Areas.Admin.Pages.Brands
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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {

                var model = _context.Brands.Where(c => c.BrandId == id).FirstOrDefault();
                if (model == null)
                {
                    return Redirect("../Error");
                }
                var uniqeFileName = "";

                if (Response.HttpContext.Request.Form.Files.Count() > 0)
                {

                    var ImagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/Brand/" + model.BrandPic);
                    if (System.IO.File.Exists(ImagePath))
                    {
                        System.IO.File.Delete(ImagePath);
                    }
                    string uploadFolder = Path.Combine(_hostEnvironment.WebRootPath, "Images/Brand");

                    string ext = Path.GetExtension(Response.HttpContext.Request.Form.Files[0].FileName);

                    uniqeFileName = Guid.NewGuid().ToString("N") + ext;

                    string uploadedImagePath = Path.Combine(uploadFolder, uniqeFileName);

                    using (FileStream fileStream = new FileStream(uploadedImagePath, FileMode.Create))
                    {
                        Response.HttpContext.Request.Form.Files[0].CopyTo(fileStream);
                    }
                    model.BrandPic = uniqeFileName;
                }
                model.BrandSort = brand.BrandSort;
                model.IsActive = brand.IsActive;
                model.BrandTlAr = brand.BrandTlAr;
                model.BrandTlEn = brand.BrandTlEn;
                _context.Attach(model).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("Brand Edited successfully");


            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");

            }

            return RedirectToPage("./Index");
        }

    }
}
