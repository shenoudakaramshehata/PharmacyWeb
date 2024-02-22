﻿using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Pharmacy.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharmacy.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;
        public IRequestCultureFeature locale;
        public string BrowserCulture;
        private PharmacyContext _context;


        public string ContentAr { get; set; }

        public string ContentEn { get; set; }


        public PrivacyModel(PharmacyContext context)
        {
            _context = context;

        }


        public void OnGet()
        {
            locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            BrowserCulture = locale.RequestCulture.UICulture.ToString();
            var pageContent = _context.PageContent.FirstOrDefault(p => p.PageContentId == 4);
            if (pageContent != null)
            {
                ContentAr = pageContent.ContentAr;
                ContentEn = pageContent.ContentEn;

            }

        }
    }
}
