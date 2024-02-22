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

namespace Pharmacy.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]

    public class BrandsController : Controller
    {
        private PharmacyContext _context;

        public BrandsController(PharmacyContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var brands = _context.Brands.Select(i => new {
                i.BrandId,
                i.BrandTlAr,
                i.BrandTlEn,
                i.BrandPic,
                i.BrandSort,
                i.IsActive
            });

            // If you work with a large amount of data, consider specifying the PaginateViaPrimaryKey and PrimaryKey properties.
            // In this case, keys and data are loaded in separate queries. This can make the SQL execution plan more efficient.
            // Refer to the topic https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "BrandId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(brands, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Brand();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Brands.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.BrandId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Brands.FirstOrDefaultAsync(item => item.BrandId == key);
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
            var model = await _context.Brands.FirstOrDefaultAsync(item => item.BrandId == key);

            _context.Brands.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(Brand model, IDictionary values) {
            string BRAND_ID = nameof(Brand.BrandId);
            string BRAND_TL_AR = nameof(Brand.BrandTlAr);
            string BRAND_TL_EN = nameof(Brand.BrandTlEn);
            string BRAND_PIC = nameof(Brand.BrandPic);
            string BRAND_SORT = nameof(Brand.BrandSort);
            string IS_ACTIVE = nameof(Brand.IsActive);

            if(values.Contains(BRAND_ID)) {
                model.BrandId = Convert.ToInt32(values[BRAND_ID]);
            }

            if(values.Contains(BRAND_TL_AR)) {
                model.BrandTlAr = Convert.ToString(values[BRAND_TL_AR]);
            }

            if(values.Contains(BRAND_TL_EN)) {
                model.BrandTlEn = Convert.ToString(values[BRAND_TL_EN]);
            }

            if(values.Contains(BRAND_PIC)) {
                model.BrandPic = Convert.ToString(values[BRAND_PIC]);
            }

            if(values.Contains(BRAND_SORT)) {
                model.BrandSort = values[BRAND_SORT] != null ? Convert.ToInt32(values[BRAND_SORT]) : (int?)null;
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