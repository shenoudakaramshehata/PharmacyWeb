using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pharmacy.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pharmacy.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using NToastNotify;

namespace Pharmacy.Areas.Admin.Pages.SystemConfigration
{
   

    public class EditPageContentModel : PageModel
    {

        private PharmacyContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;

        public EditPageContentModel(PharmacyContext context, IWebHostEnvironment hostEnvironment, IToastNotification toastNotification)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;

        }


        [BindProperty]
        public string ContentAr { get; set; }
        [BindProperty]
        public string ContentEn { get; set; }
    


        [BindProperty]
        public PageContent pageContent { get; set; }
        public IActionResult OnGet(int id)
        {
            try
            {
                pageContent = _context.PageContent.FirstOrDefault(p => p.PageContentId == id);
                if (pageContent != null)
                {
                    ContentAr = pageContent.PageTitleAr;
                    ContentEn = pageContent.ContentEn;
                }


            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something went wrong");

            }

            return Page();


        }
        public  IActionResult OnPost(int id)
        {
            try
            {
                var model = _context.PageContent.FirstOrDefault(p => p.PageContentId == id);
                if (model != null)
                {
                    model.ContentAr = pageContent.ContentAr;
                    model.ContentEn = pageContent.ContentEn;
                    _context.Attach(model).State = EntityState.Modified;
                    _context.SaveChanges();
                    _toastNotification.AddSuccessToastMessage("Page Content Edited successfully");

                }

            }
            catch (Exception)
            {

                _toastNotification.AddErrorToastMessage("Something went wrong");
                return Page();


            }
            return RedirectToPage("./PagesContent");


        }
    }
}
