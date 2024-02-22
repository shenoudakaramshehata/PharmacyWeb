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

    public class HomeSlidersController : Controller
    {
        private PharmacyContext _context;

        public HomeSlidersController(PharmacyContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var homesliders = _context.HomeSliders.Select(i => new {
                i.HomeSliderId,
                i.HomeSliderPic,
                i.HomeSliderTypeId,
                i.HomeSliderEntityId

            });

            // If you work with a large amount of data, consider specifying the PaginateViaPrimaryKey and PrimaryKey properties.
            // In this case, keys and data are loaded in separate queries. This can make the SQL execution plan more efficient.
            // Refer to the topic https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "HomeSliderId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(homesliders, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new HomeSlider();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.HomeSliders.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.HomeSliderId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.HomeSliders.FirstOrDefaultAsync(item => item.HomeSliderId == key);
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
            var model = await _context.HomeSliders.FirstOrDefaultAsync(item => item.HomeSliderId == key);

            _context.HomeSliders.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> HomeSliderTypesLookup(DataSourceLoadOptions loadOptions) {

            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var lookupEn = from i in _context.HomeSliderTypes
                               orderby i.HomeSliderTypeTlEn
                               select new
                               {
                                   Value = i.HomeSliderTypeId,
                                   Text = i.HomeSliderTypeTlEn
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }
            var lookupAr = from i in _context.HomeSliderTypes
                           orderby i.HomeSliderTypeTlAr
                           select new
                           {
                               Value = i.HomeSliderTypeId,
                               Text = i.HomeSliderTypeTlAr
                           };
            return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));
            
        }

        private void PopulateModel(HomeSlider model, IDictionary values) {
            string HOME_SLIDER_ID = nameof(HomeSlider.HomeSliderId);
            string HOME_SLIDER_PIC = nameof(HomeSlider.HomeSliderPic);
            string HOME_SLIDER_TYPE_ID = nameof(HomeSlider.HomeSliderTypeId);
            string HOME_SLIDER_ENTITY_ID = nameof(HomeSlider.HomeSliderEntityId);

            if(values.Contains(HOME_SLIDER_ID)) {
                model.HomeSliderId = Convert.ToInt32(values[HOME_SLIDER_ID]);
            }

            if(values.Contains(HOME_SLIDER_PIC)) {
                model.HomeSliderPic = Convert.ToString(values[HOME_SLIDER_PIC]);
            }

            if(values.Contains(HOME_SLIDER_TYPE_ID)) {
                model.HomeSliderTypeId = values[HOME_SLIDER_TYPE_ID] != null ? Convert.ToInt32(values[HOME_SLIDER_TYPE_ID]) : (int?)null;
            }

            if(values.Contains(HOME_SLIDER_ENTITY_ID)) {
                model.HomeSliderEntityId = Convert.ToString(values[HOME_SLIDER_ENTITY_ID]);
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