using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using Pharmacy.Data;
using Pharmacy.Models;

namespace Pharmacy.Areas.Admin.Pages.Homeslider22
{
    public class CreateModel : PageModel
    {
             private PharmacyContext _context;
            private readonly IWebHostEnvironment _hostEnvironment;
            private readonly IToastNotification _toastNotification;

            public CreateModel(PharmacyContext context, IWebHostEnvironment hostEnvironment, IToastNotification toastNotification)
            {
                _context = context;
                _hostEnvironment = hostEnvironment;
                _toastNotification = toastNotification;

            }
            public void OnGet()
            {
            }
            public IActionResult OnPost(HomeSlider model)
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }
                try
                {
                    if (model.HomeSliderTypeId == 1)
                    {
                        model.HomeSliderEntityId = Request.Form["CategoryId"];
                    }
                    if (model.HomeSliderTypeId == 2)
                    {
                        model.HomeSliderEntityId = Request.Form["ItemId"];
                    }

                    if (model.HomeSliderTypeId == 3)
                    {
                        if (model.HomeSliderEntityId == null || model.HomeSliderEntityId == "")
                        {
                            ModelState.AddModelError("Validation", "enter link");
                            return Page();
                        }

                    }
                    var uniqeFileName = "";

                    if (Response.HttpContext.Request.Form.Files.Count() > 0)
                    {
                        string uploadFolder = Path.Combine(_hostEnvironment.WebRootPath, "Images/Slider");
                        string ext = Path.GetExtension(Response.HttpContext.Request.Form.Files[0].FileName);
                        uniqeFileName = Guid.NewGuid().ToString("N") + ext;
                        string uploadedImagePath = Path.Combine(uploadFolder, uniqeFileName);
                        using (FileStream fileStream = new FileStream(uploadedImagePath, FileMode.Create))
                        {
                            Response.HttpContext.Request.Form.Files[0].CopyTo(fileStream);
                        }
                        model.HomeSliderPic = uniqeFileName;
                    }
                    _context.HomeSliders.Add(model);
                    _context.SaveChanges();
                    _toastNotification.AddSuccessToastMessage("Home Slider Added successfully");

                }
                catch (Exception)
                {

                    _toastNotification.AddErrorToastMessage("Something went wrong");
                }

                return Redirect("./Index");

            }
        }
    }
