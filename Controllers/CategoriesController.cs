﻿using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Pharmacy.Data;
using Pharmacy.Models;
using Microsoft.AspNetCore.Localization;

namespace Pharmacy.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]

    public class CategoriesController : Controller
    {
        private PharmacyContext _context;

        public CategoriesController(PharmacyContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var categories = _context.Categories.Include(a => a.Section).Select(i => new {
                i.CategoryId,
                i.CategoryTlAr,
                i.CategoryTlEn,
                i.CategoryPic,
                i.CategorySort,
                i.IsActive,
                i.SectionId
                
            });

            // If you work with a large amount of data, consider specifying the PaginateViaPrimaryKey and PrimaryKey properties.
            // In this case, keys and data are loaded in separate queries. This can make the SQL execution plan more efficient.
            // Refer to the topic https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "CategoryId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(categories, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Category();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Categories.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.CategoryId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Categories.FirstOrDefaultAsync(item => item.CategoryId == key);
            if(model == null)
                return StatusCode(409, "Object not found");

            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task Delete(int key) {
            var model = await _context.Categories.FirstOrDefaultAsync(item => item.CategoryId == key);

            _context.Categories.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> SectionsLookup(DataSourceLoadOptions loadOptions) {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var lookupEn = from i in _context.Sections
                               orderby i.SectionTlEn
                               select new
                               {
                                   Value = i.SectionId,
                                   Text = i.SectionTlEn
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }
            var lookupAr = from i in _context.Sections
                           orderby i.SectionTlAr
                           select new
                           {
                               Value = i.SectionId,
                               Text = i.SectionTlAr
                           };
            return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));



        }

        private void PopulateModel(Category model, IDictionary values) {
            string CATEGORY_ID = nameof(Category.CategoryId);
            string CATEGORY_TL_AR = nameof(Category.CategoryTlAr);
            string CATEGORY_TL_EN = nameof(Category.CategoryTlEn);
            string CATEGORY_PIC = nameof(Category.CategoryPic);
            string CATEGORY_SORT = nameof(Category.CategorySort);
            string IS_ACTIVE = nameof(Category.IsActive);

            if(values.Contains(CATEGORY_ID)) {
                model.CategoryId = Convert.ToInt32(values[CATEGORY_ID]);
            }

          

            if(values.Contains(CATEGORY_TL_AR)) {
                model.CategoryTlAr = Convert.ToString(values[CATEGORY_TL_AR]);
            }

            if(values.Contains(CATEGORY_TL_EN)) {
                model.CategoryTlEn = Convert.ToString(values[CATEGORY_TL_EN]);
            }

            if(values.Contains(CATEGORY_PIC)) {
                model.CategoryPic = Convert.ToString(values[CATEGORY_PIC]);
            }

            if(values.Contains(CATEGORY_SORT)) {
                model.CategorySort = values[CATEGORY_SORT] != null ? Convert.ToInt32(values[CATEGORY_SORT]) : (int?)null;
            }

            if(values.Contains(IS_ACTIVE)) {
                model.IsActive = values[IS_ACTIVE] != null ? Convert.ToBoolean(values[IS_ACTIVE]) : (bool?)null;
            }
        }

        private string GetFullErrorMessage(ModelStateDictionary modelState) {
            var messages = new List<string>();

            foreach(var entry in modelState) {
                foreach(var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return String.Join(" ", messages);
        }
    }
}