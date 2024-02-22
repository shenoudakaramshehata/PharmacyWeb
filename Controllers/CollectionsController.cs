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

    public class CollectionsController : Controller
    {
        private PharmacyContext _context;

        public CollectionsController(PharmacyContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var collections = _context.Collections.Select(i => new {
                i.CollectionId,
                i.CollectionTlAr,
                i.CollectionTlEn,
                i.CollectionSort,
                i.IsActive
            });

            // If you work with a large amount of data, consider specifying the PaginateViaPrimaryKey and PrimaryKey properties.
            // In this case, keys and data are loaded in separate queries. This can make the SQL execution plan more efficient.
            // Refer to the topic https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "CollectionId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(collections, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Collection();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Collections.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.CollectionId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Collections.FirstOrDefaultAsync(item => item.CollectionId == key);
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
            var model = await _context.Collections.FirstOrDefaultAsync(item => item.CollectionId == key);

            _context.Collections.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(Collection model, IDictionary values) {
            string COLLECTION_ID = nameof(Collection.CollectionId);
            string COLLECTION_TL_AR = nameof(Collection.CollectionTlAr);
            string COLLECTION_TL_EN = nameof(Collection.CollectionTlEn);
            string COLLECTION_SORT = nameof(Collection.CollectionSort);
            string IS_ACTIVE = nameof(Collection.IsActive);

            if(values.Contains(COLLECTION_ID)) {
                model.CollectionId = Convert.ToInt32(values[COLLECTION_ID]);
            }

            if(values.Contains(COLLECTION_TL_AR)) {
                model.CollectionTlAr = Convert.ToString(values[COLLECTION_TL_AR]);
            }

            if(values.Contains(COLLECTION_TL_EN)) {
                model.CollectionTlEn = Convert.ToString(values[COLLECTION_TL_EN]);
            }

            if(values.Contains(COLLECTION_SORT)) {
                model.CollectionSort = values[COLLECTION_SORT] != null ? Convert.ToInt32(values[COLLECTION_SORT]) : (int?)null;
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