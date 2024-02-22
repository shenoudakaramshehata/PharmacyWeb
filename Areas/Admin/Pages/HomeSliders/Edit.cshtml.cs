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

namespace Pharmacy.Areas.Admin.Pages.HomeSliders
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
        public HomeSlider homeSlider { get; set; }
        [BindProperty]
        public int? CategoryId { get; set; }
        public int? ItemId { get; set; }
        public string Externallink { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                homeSlider = _context.HomeSliders.Where(c => c.HomeSliderId == id).FirstOrDefault();
                if (homeSlider == null)
                {
                    return Redirect("../Error");
                }
                if (homeSlider.HomeSliderTypeId == 3)
                {
                    Externallink = homeSlider.HomeSliderEntityId;
                    //EntityId = 0;
                }
                if(homeSlider.HomeSliderTypeId == 2)
                {
                    if (homeSlider.HomeSliderEntityId != null && homeSlider.HomeSliderEntityId != "")
                    {
                        ItemId = int.Parse(homeSlider.HomeSliderEntityId);
                    }
                       
                }
                if(homeSlider.HomeSliderTypeId == 1)
                {
                    if(homeSlider.HomeSliderEntityId!=null && homeSlider.HomeSliderEntityId != "")
                    {
                        CategoryId = int.Parse(homeSlider.HomeSliderEntityId);
                    }
                   
                    //EntityId = int.Parse(homeSlider.HomeSliderEntityId);
                    //homeSlider.HomeSliderEntityId = "";
                }

            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");

            }



            return Page();
        }
        public IActionResult OnPost(int id)
        
        {
           
            try
            {
                var model = _context.HomeSliders.Where(c => c.HomeSliderId == id).FirstOrDefault();
                if (model==null)
                {
                    return Redirect("../Error");
                }
                if (homeSlider.HomeSliderTypeId == 1)
                {
                    
                    model.HomeSliderEntityId = Request.Form["CategoryId"];
                }
                if (homeSlider.HomeSliderTypeId == 2)
                {
                    model.HomeSliderEntityId = Request.Form["ItemId"];
                }
               
                if (homeSlider.HomeSliderTypeId == 3)
                {
                    if (homeSlider.HomeSliderEntityId == null|| homeSlider.HomeSliderEntityId=="")
                    {
                        ModelState.AddModelError("Validation", "enter link");
                        homeSlider.HomeSliderPic = model.HomeSliderPic;
                       
                        return Page();
                    }
                    model.HomeSliderEntityId = homeSlider.HomeSliderEntityId;
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
                    var ImagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/Slider/" + model.HomeSliderPic);
                    if (System.IO.File.Exists(ImagePath))
                    {
                        System.IO.File.Delete(ImagePath);
                    }
                    model.HomeSliderPic = uniqeFileName;
                }
                model.HomeSliderTypeId = homeSlider.HomeSliderTypeId;
                _context.Attach(model).State = EntityState.Modified;
                 _context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Home Slider Edited successfully");

            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");
            }

            return Redirect("./Index");

        }
    }
}
