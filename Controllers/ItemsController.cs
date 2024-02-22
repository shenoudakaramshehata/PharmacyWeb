using DevExtreme.AspNet.Data;
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

    public class ItemsController : Controller
    {
        private PharmacyContext _context;

        public ItemsController(PharmacyContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var items = _context.Items.Select(i => new {
                i.ItemId,
                i.ItemTlAr,
                i.ItemTlEn,
                i.Price,
                i.BrandId,
                i.Description,
                i.Remarks,
                i.IsActive,
                i.CategoryId,
                i.ItemPic,
                i.Stock
            });

            // If you work with a large amount of data, consider specifying the PaginateViaPrimaryKey and PrimaryKey properties.
            // In this case, keys and data are loaded in separate queries. This can make the SQL execution plan more efficient.
            // Refer to the topic https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "ItemId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(items, loadOptions));
        }

         [HttpGet]
        public async Task<IActionResult> GetFavByCustomerId(int customerId,DataSourceLoadOptions loadOptions) {
            var itemIds = _context.CustomerFav.Where(c => c.CustomerId == customerId).Select(c => c.ItemId);


            var items = _context.Items.Where(c=> itemIds.Contains(c.ItemId)).Select(i => new {
                i.ItemId,
                i.ItemTlAr,
                i.ItemTlEn,
                i.Price,
                i.BrandId,
                i.Description,
                i.Remarks,
                i.IsActive,
                i.CategoryId,
                i.ItemPic
            });

            return Json(await DataSourceLoader.LoadAsync(items, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Item();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Items.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.ItemId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Items.FirstOrDefaultAsync(item => item.ItemId == key);
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
            var model = await _context.Items.FirstOrDefaultAsync(item => item.ItemId == key);

            _context.Items.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> BrandsLookup(DataSourceLoadOptions loadOptions) {

            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var lookupEn = from i in _context.Brands
                                 orderby i.BrandTlEn
                                 select new
                                 {
                                     Value = i.BrandId,
                                     Text = i.BrandTlEn
                                 };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }
            var lookupAr = from i in _context.Brands
                             orderby i.BrandTlAr
                             select new
                             {
                                 Value = i.BrandId,
                                 Text = i.BrandTlAr
                             };
            return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));

 }

        [HttpGet]
        public async Task<IActionResult> CategoriesLookup(DataSourceLoadOptions loadOptions) {

            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var lookupEn = from i in _context.Categories
                               orderby i.CategoryTlEn
                               select new
                               {
                                   Value = i.CategoryId,
                                   Text = i.CategoryTlEn
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }
            var lookupAr = from i in _context.Categories
                           orderby i.CategoryTlAr
                           select new
                           {
                               Value = i.CategoryId,
                               Text = i.CategoryTlAr
                           };
            return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));



        }

        private void PopulateModel(Item model, IDictionary values) {
            string ITEM_ID = nameof(Item.ItemId);
            string ITEM_TL_AR = nameof(Item.ItemTlAr);
            string ITEM_TL_EN = nameof(Item.ItemTlEn);
            string PRICE = nameof(Item.Price);
            string STOCK = nameof(Item.Stock);
            string BRAND_ID = nameof(Item.BrandId);
            string DESCRIPTION = nameof(Item.Description);
            string REMARKS = nameof(Item.Remarks);
            string IS_ACTIVE = nameof(Item.IsActive);
            string CATEGORY_ID = nameof(Item.CategoryId);
            string ITEM_PIC = nameof(Item.ItemPic);

            if(values.Contains(ITEM_ID)) {
                model.ItemId = Convert.ToInt32(values[ITEM_ID]);
            }

            if(values.Contains(ITEM_TL_AR)) {
                model.ItemTlAr = Convert.ToString(values[ITEM_TL_AR]);
            }

            if(values.Contains(ITEM_TL_EN)) {
                model.ItemTlEn = Convert.ToString(values[ITEM_TL_EN]);
            }

            if(values.Contains(PRICE)) {
                model.Price = values[PRICE] != null ? Convert.ToDouble(values[PRICE], CultureInfo.InvariantCulture) : (double?)null;
            }
            if (values.Contains(STOCK))
            {
                model.Stock = values[STOCK] != null ? Convert.ToInt32(values[STOCK], CultureInfo.InvariantCulture) : (int?)null;
            }

            if (values.Contains(BRAND_ID)) {
                model.BrandId = values[BRAND_ID] != null ? Convert.ToInt32(values[BRAND_ID]) : (int?)null;
            }

            if(values.Contains(DESCRIPTION)) {
                model.Description = Convert.ToString(values[DESCRIPTION]);
            }

            if(values.Contains(REMARKS)) {
                model.Remarks = Convert.ToString(values[REMARKS]);
            }

            if(values.Contains(IS_ACTIVE)) {
                model.IsActive = values[IS_ACTIVE] != null ? Convert.ToBoolean(values[IS_ACTIVE]) : (bool?)null;
            }

            if(values.Contains(CATEGORY_ID)) {
                model.CategoryId = Convert.ToInt32(values[CATEGORY_ID]);
            }

            if(values.Contains(ITEM_PIC)) {
                model.ItemPic = Convert.ToString(values[ITEM_PIC]);
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