using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using Pharmacy.Data;
using Pharmacy.Models;

namespace Pharmacy.Areas.Admin.Pages.Items
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
        public IActionResult OnPost(Item model)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                var uniqeFileName = "";
                string uploadFolder = Path.Combine(_hostEnvironment.WebRootPath, "Images/Item");


                if (Response.HttpContext.Request.Form.Files.Count() > 0)
                {

                    string ext = Path.GetExtension(Response.HttpContext.Request.Form.Files[0].FileName);

                    uniqeFileName = Guid.NewGuid().ToString("N") + ext;

                    string uploadedImagePath = Path.Combine(uploadFolder, uniqeFileName);

                    using (FileStream fileStream = new FileStream(uploadedImagePath, FileMode.Create))
                    {
                        Response.HttpContext.Request.Form.Files[0].CopyTo(fileStream);
                    }
                    model.ItemPic = uniqeFileName;
                }
                _context.Items.Add(model);
                _context.SaveChanges();



                if (Response.HttpContext.Request.Form.Files.Count() > 1 && model.ItemId>0)
                {
                    for (int i = 1; i < Response.HttpContext.Request.Form.Files.Count(); i++)
                    {
                        var ins = new ItemImage();
                        string ext = Path.GetExtension(Response.HttpContext.Request.Form.Files[i].FileName);

                        uniqeFileName = Guid.NewGuid().ToString("N") + ext;

                        string uploadedImagePath = Path.Combine(uploadFolder, uniqeFileName);

                        using (FileStream fileStream = new FileStream(uploadedImagePath, FileMode.Create))
                        {
                            Response.HttpContext.Request.Form.Files[i].CopyTo(fileStream);
                        }
                        ins.ImageUrl = uniqeFileName;
                        ins.ItemId = model.ItemId;
                        _context.ItemImages.Add(ins);

                    }
                    _context.SaveChanges();
                }

                _toastNotification.AddSuccessToastMessage("Item Added successfully");






            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");
            }
          
            return Redirect("./Index");

        }
    }
}
