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

    public class CollectionItemsController : Controller
    {
        private PharmacyContext _context;

        public CollectionItemsController(PharmacyContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var collectionitems = _context.CollectionItems.Select(i => new {
                i.CollectionItemId,
                i.CollectionId,
                i.ItemId
            });

            // If you work with a large amount of data, consider specifying the PaginateViaPrimaryKey and PrimaryKey properties.
            // In this case, keys and data are loaded in separate queries. This can make the SQL execution plan more efficient.
            // Refer to the topic https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "CollectionItemId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(collectionitems, loadOptions));
        }



        [HttpGet]
        public async Task<IActionResult> GetByCollectionId(int collectionId, DataSourceLoadOptions loadOptions)
        {
            var collectionitems = _context.CollectionItems.Where(c=>c.CollectionId== collectionId).Select(i => new {
                i.CollectionItemId,
                i.CollectionId,
                i.ItemId,
                i.Item
            });
            // If you work with a large amount of data, consider specifying the PaginateViaPrimaryKey and PrimaryKey properties.
            // In this case, keys and data are loaded in separate queries. This can make the SQL execution plan more efficient.
            // Refer to the topic https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "CollectionItemId" };
            // loadOptions.PaginateViaPrimaryKey = true;
            return Json(await DataSourceLoader.LoadAsync(collectionitems, loadOptions));
        }


        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new CollectionItem();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.CollectionItems.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.CollectionItemId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.CollectionItems.FirstOrDefaultAsync(item => item.CollectionItemId == key);
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
            var model = await _context.CollectionItems.FirstOrDefaultAsync(item => item.CollectionItemId == key);

            _context.CollectionItems.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> CollectionsLookup(DataSourceLoadOptions loadOptions) {

            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var lookupEn = from i in _context.Collections
                               orderby i.CollectionTlEn
                               select new
                               {
                                   Value = i.CollectionId,
                                   Text = i.CollectionTlEn
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }
            var lookupAr = from i in _context.Collections
                           orderby i.CollectionTlEn
                           select new
                           {
                               Value = i.CollectionId,
                               Text = i.CollectionTlEn
                           };
            return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));


        }

        [HttpGet]
        public async Task<IActionResult> ItemsLookup(DataSourceLoadOptions loadOptions) {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var lookupEn = from i in _context.Items
                               orderby i.ItemTlEn
                               select new
                               {
                                   Value = i.ItemId,
                                   Text = i.ItemTlEn
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }
            var lookupAr = from i in _context.Items
                           orderby i.ItemTlAr
                           select new
                           {
                               Value = i.ItemId,
                               Text = i.ItemTlAr
                           };
            return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));






        }

        private void PopulateModel(CollectionItem model, IDictionary values) {
            string COLLECTION_ITEM_ID = nameof(CollectionItem.CollectionItemId);
            string COLLECTION_ID = nameof(CollectionItem.CollectionId);
            string ITEM_ID = nameof(CollectionItem.ItemId);

            if(values.Contains(COLLECTION_ITEM_ID)) {
                model.CollectionItemId = Convert.ToInt32(values[COLLECTION_ITEM_ID]);
            }

            if(values.Contains(COLLECTION_ID)) {
                model.CollectionId = Convert.ToInt32(values[COLLECTION_ID]);
            }

            if(values.Contains(ITEM_ID)) {
                model.ItemId = Convert.ToInt32(values[ITEM_ID]);
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