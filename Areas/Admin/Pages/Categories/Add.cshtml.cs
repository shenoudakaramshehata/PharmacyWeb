using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pharmacy.Data;
using Microsoft.AspNetCore.Localization;
using NToastNotify;

namespace Pharmacy.Areas.Admin.Pages.Categories
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

        public void OnGet()
        {
          
        }
        public IActionResult OnPost(Models.Category model)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                var uniqeFileName = "";

                if (Response.HttpContext.Request.Form.Files.Count() > 0)
                {
                    string uploadFolder = Path.Combine(_hostEnvironment.WebRootPath, "Images/Category");
                   
                     string ext = Path.GetExtension(Response.HttpContext.Request.Form.Files[0].FileName);

                    uniqeFileName = Guid.NewGuid().ToString("N") +ext;

                    string uploadedImagePath = Path.Combine(uploadFolder, uniqeFileName);

                    using (FileStream fileStream = new FileStream(uploadedImagePath, FileMode.Create))
                    {
                        Response.HttpContext.Request.Form.Files[0].CopyTo(fileStream);
                    }
                    model.CategoryPic = uniqeFileName;
                }
            

                _context.Categories.Add(model);
                _context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Category Added successfully");

            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");
            }
          
            return Redirect("./Index");

        }
    }
}
