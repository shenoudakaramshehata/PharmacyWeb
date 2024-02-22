using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using Pharmacy.Data;
using Pharmacy.Models;

namespace Pharmacy.Areas.Admin.Pages.DeliveryAgent
{
    public class AddDeliveryAgentModel : PageModel
    {
        private PharmacyContext _context;
        private readonly IToastNotification _toastNotification;
        private readonly UserManager<ApplicationUser> _userManager;

       
        public AddDeliveryAgentModel(PharmacyContext context,IToastNotification toastNotification, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _toastNotification = toastNotification;
            _userManager = userManager;

        }

        public void OnGet()
        {



        }
        public async Task<IActionResult> OnPostAsync(Delivery model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Email);
            if (userExists != null)
            {
                _toastNotification.AddErrorToastMessage("Email is already taken. Enter another Email!");
                return Page();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

           
            try
            {
                
                _context.Deliveries.Add(model);
                _context.SaveChanges();

                if (model.DeliveryId > 0)
                {
                    var user = new ApplicationUser
                    {
                        UserName = model.Title,
                        Email = model.Email,
                        PhoneNumber = model.Phone1,
                        EntityId = model.DeliveryId,
                        EntityName = "Delivery"

                    };

                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (!result.Succeeded)
                    {
                        _context.Deliveries.Remove(model);
                        _context.SaveChanges();
                        _toastNotification.AddErrorToastMessage("Something went Error");
                        return Redirect("DeliveryAgentIndex");

                    }
                    await _userManager.AddToRoleAsync(user, "Delivery");
                    _toastNotification.AddSuccessToastMessage("Delivery Added successfully");
                }

            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");
            }

            return Redirect("DeliveryAgentIndex");

        }
    }
}
