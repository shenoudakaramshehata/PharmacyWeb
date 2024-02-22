using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using Pharmacy.Data;
using Pharmacy.Models;

namespace Pharmacy.Areas.Admin.Pages.Home_slider
{
    public class IndexModel : PageModel
    {
        private PharmacyContext _context;
        private readonly IToastNotification _toastNotification;

        public IndexModel(PharmacyContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

        [BindProperty(SupportsGet = true)]

        public List<HomeSlider> SliderLst { get; set; }

        public void OnGet()
        {
            try
            {
                SliderLst = _context.HomeSliders.ToList();
                foreach (var item in SliderLst)
                {
                    if (item.HomeSliderEntityId == "" || item.HomeSliderEntityId == null)
                        continue;

                    if (item.HomeSliderTypeId == 1)
                    {
                        var id = Convert.ToInt32(item.HomeSliderEntityId);

                        item.HomeSliderEntityId = _context.Categories.FirstOrDefault(c => c.CategoryId == id)?.CategoryTlAr;
                    }
                    if (item.HomeSliderTypeId == 2)
                    {
                        var id = Convert.ToInt32(item.HomeSliderEntityId);

                        item.HomeSliderEntityId = _context.Items.FirstOrDefault(c => c.ItemId == id)?.ItemTlAr;
                    }

                }
            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");

            }

        }
    }
}

