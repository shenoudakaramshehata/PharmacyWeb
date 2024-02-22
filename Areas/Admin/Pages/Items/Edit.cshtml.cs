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

namespace Pharmacy.Areas.Admin.Pages.Items
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
        public Item item { get; set; }


        public async Task<IActionResult> OnGetAsync(int id)
        {

            try
            {


                item = await _context.Items.FirstOrDefaultAsync(m => m.ItemId == id);
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
                var model = _context.Items.Where(c => c.ItemId == id).FirstOrDefault();
                if (model == null)
                {
                    return Redirect("../Error");

                }

                var uniqeFileName = "";
                string uploadFolder = Path.Combine(_hostEnvironment.WebRootPath, "Images/Item");

                for (int i = 0; i < Response.HttpContext.Request.Form.Files.Count(); i++)
                {

                    string ext = Path.GetExtension(Response.HttpContext.Request.Form.Files[i].FileName);
                    uniqeFileName = Guid.NewGuid().ToString("N") + ext;
                    string uploadedImagePath = Path.Combine(uploadFolder, uniqeFileName);
                    using (FileStream fileStream = new FileStream(uploadedImagePath, FileMode.Create))
                    {
                        Response.HttpContext.Request.Form.Files[i].CopyTo(fileStream);

                    }
                    if (HttpContext.Request.Form.Files[i].Name == "MainImage")
                    {
                        var ImagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/Item/" + model.ItemPic);
                        if (System.IO.File.Exists(ImagePath))
                        {
                            System.IO.File.Delete(ImagePath);
                        }
                        model.ItemPic = uniqeFileName;
                    }
                    else
                    {
                        var ins = new ItemImage();
                        ins.ImageUrl = uniqeFileName;
                        ins.ItemId = model.ItemId;
                        _context.ItemImages.Add(ins);

                    }

                }
                _context.SaveChanges();

                model.IsActive = item.IsActive;
                model.ItemTlAr = item.ItemTlAr;
                model.ItemTlEn = item.ItemTlEn;
                model.BrandId = item.BrandId;
                model.CategoryId = item.CategoryId;
                model.Price = item.Price;
                model.Description = item.Description;
                model.Remarks = item.Remarks;
                model.Stock = item.Stock;


                _context.Attach(model).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("Item Edited successfully");

            }
            catch (DbUpdateConcurrencyException)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");

            }

            return RedirectToPage("./Index");
        }
    }
}
