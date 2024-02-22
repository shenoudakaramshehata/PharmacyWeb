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

namespace Pharmacy.Areas.Admin.Pages.Areas
{
    public class DeleteModel : PageModel
    {

        private PharmacyContext _context;
        private readonly IToastNotification _toastNotification;

        public DeleteModel(PharmacyContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;

        }
        [BindProperty]
        public Area area { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
           
            try
            {
                area = await _context.Area.Include(c=>c.City.Country).FirstOrDefaultAsync(m => m.AreaId == id);
                if (area == null)
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
            area = await _context.Area.Include(c => c.City.Country).FirstOrDefaultAsync(m => m.AreaId == id);
            if (!ModelState.IsValid)
            {
                return Page();
            }
            

            try
            {
            
                if (area != null)
                {
                    _context.Area.Remove(area);
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("City Deleted successfully");


                }
            }
            catch (Exception)

            {
                _toastNotification.AddErrorToastMessage("Something went wrong");
                area = await _context.Area.Include(c => c.City.Country).FirstOrDefaultAsync(m => m.AreaId == id);
                return Page();

            }

            return RedirectToPage("./Index");
        }

    }
}
