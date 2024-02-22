using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Data;
using Pharmacy.Models;
using NToastNotify;

namespace Pharmacy.Areas.Admin.Pages.ClientsMessages
{
    public class DetailsModel : PageModel
    {
        private PharmacyContext _context;
        private readonly IToastNotification _toastNotification;

        public DetailsModel(PharmacyContext context,  IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;

        }

        public ContactUs ContactUsMessages { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            try
            {
                if (id == null)
                {
                    return Redirect("../Error");

                }

                ContactUsMessages = await _context.ContactUs.FirstOrDefaultAsync(m => m.ContactUsId == id);

                if (ContactUsMessages == null)
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
    }
}
