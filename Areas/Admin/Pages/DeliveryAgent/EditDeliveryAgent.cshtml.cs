using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Pharmacy.Data;
using Pharmacy.Models;

namespace Pharmacy.Areas.Admin.Pages.DeliveryAgent
{
    public class EditDeliveryAgentModel : PageModel
    {
        private PharmacyContext _context;
        private readonly IToastNotification _toastNotification;
        UserManager<ApplicationUser> UserManger;

        public EditDeliveryAgentModel(PharmacyContext context,  IToastNotification toastNotification, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _toastNotification = toastNotification;
            UserManger = userManager;
        }
        [BindProperty]
        public Delivery delivery { get; set; }



        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {

                delivery = await _context.Deliveries.FirstOrDefaultAsync(m => m.DeliveryId == id);
                if (delivery == null)
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
                
                var model = _context.Deliveries.Where(c => c.DeliveryId == id).FirstOrDefault();
                var user = await UserManger.FindByNameAsync(model.Title);
                if (delivery.Title != model.Title)
                {
                    var userExists = await UserManger.FindByNameAsync(delivery.Title);

                    if (userExists != null)
                    {
                        _toastNotification.AddErrorToastMessage("Title is already taken. Enter another title!");
                        return Page();
                    }
                }
                if (model == null)
                {
                    return Redirect("../Error");
                }
                if (delivery.Password != null)
                {
                    if (user == null)
                    {
                        _toastNotification.AddErrorToastMessage("User doesnot exit");
                        return Page();
                    }
                    var token = await UserManger.GeneratePasswordResetTokenAsync(user);

                    var result = await UserManger.ResetPasswordAsync(user, token,delivery.Password);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return Page();
                    }
                }

                model.Title = delivery.Title;
                user.UserName = delivery.Title;
                model.Address = delivery.Address;
                model.Email = delivery.Email;
                model.Phone1 = delivery.Phone1;
                model.Phone2 = delivery.Phone2;
                model.Description = delivery.Description;
                _context.Attach(model).State = EntityState.Modified;
                await UserManger.UpdateAsync(user);
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("Delivery Agent Edited successfully");


            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");

            }

            return RedirectToPage("DeliveryAgentIndex");
        }
    }
}
