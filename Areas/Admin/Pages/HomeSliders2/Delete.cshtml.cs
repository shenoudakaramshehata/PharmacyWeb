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
using Microsoft.Extensions.Localization;
using NToastNotify;
using Pharmacy.Data;
using Pharmacy.Models;

namespace Pharmacy.Areas.Admin.Pages.HomeSliders2
{
    public class DeleteModel : PageModel
    {
        private PharmacyContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IStringLocalizer<SharedResource> _sharedResource;
        private readonly IToastNotification _toastNotification;

        public DeleteModel(PharmacyContext context, IWebHostEnvironment hostEnvironment, IStringLocalizer<SharedResource> sharedResource, IToastNotification toastNotification)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _sharedResource = sharedResource;
            _toastNotification = toastNotification;

        }


        [BindProperty]
        public HomeSlider homeSlider { get; set; }


        [BindProperty]
        public string EntityNameEn { get; set; }
        [BindProperty]
        public string EntityNameAr { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                homeSlider = await _context.HomeSliders.Include(c => c.HomeSliderType).Where(c => c.HomeSliderId == id).FirstOrDefaultAsync();
                if (homeSlider == null)
                {
                    return Redirect("../Error");
                }

                if (homeSlider.HomeSliderTypeId == 1)
                {
                    var EntityId = Convert.ToInt32(homeSlider.HomeSliderEntityId);

                    EntityNameEn = _context.Categories.FirstOrDefault(c => c.CategoryId == EntityId)?.CategoryTlEn;
                    EntityNameAr = _context.Categories.FirstOrDefault(c => c.CategoryId == EntityId)?.CategoryTlAr;
                }
                if (homeSlider.HomeSliderTypeId == 2)
                {
                    var EntityId = Convert.ToInt32(homeSlider.HomeSliderEntityId);

                    EntityNameEn = _context.Items.FirstOrDefault(c => c.ItemId == EntityId)?.ItemTlEn;
                    EntityNameAr = _context.Items.FirstOrDefault(c => c.ItemId == EntityId)?.ItemTlAr;
                }
                if (homeSlider.HomeSliderTypeId == 3)
                {

                    EntityNameAr = homeSlider.HomeSliderEntityId;
                    EntityNameEn = homeSlider.HomeSliderEntityId;
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
                var ImagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/Slider/" + homeSlider.HomeSliderPic);


                homeSlider = await _context.HomeSliders.FindAsync(id);
                if (homeSlider == null)
                {
                    return Redirect("../Error");

                }
                _context.HomeSliders.Remove(homeSlider);
                await _context.SaveChangesAsync();
                if (System.IO.File.Exists(ImagePath))
                {
                    System.IO.File.Delete(ImagePath);
                }
                _context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Home Slider Deleted successfully");



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