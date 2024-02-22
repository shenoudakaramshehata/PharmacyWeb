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

    public class ItemImagesController : Controller
    {
        private PharmacyContext _context;

        public ItemImagesController(PharmacyContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var itemimages = _context.ItemImages.Select(i => new {
                i.ItemImageId,
                i.ItemId,
                i.ImageUrl
            });

            // If you work with a large amount of data, consider specifying the PaginateViaPrimaryKey and PrimaryKey properties.
            // In this case, keys and data are loaded in separate queries. This can make the SQL execution plan more efficient.
            // Refer to the topic https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "ItemImageId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(itemimages, loadOptions));
        }

        [HttpGet]
        public async Task<object> GetImagesForItem([FromQuery] int id)
        {
            var productimages = _context.ItemImages.Where(p => p.ItemId == id).Select(i => new {
                i.ItemId,
                i.ImageUrl,
                i.ItemImageId
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "ImageId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return productimages;
        }



        [HttpPost]
        public async Task<int> RemoveImageById([FromQuery] int id)
        {
            var itemPic = _context.ItemImages.FirstOrDefault(p => p.ItemImageId == id);
            _context.ItemImages.Remove(itemPic);
            _context.SaveChanges();

            return id;
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new ItemImage();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.ItemImages.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.ItemImageId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.ItemImages.FirstOrDefaultAsync(item => item.ItemImageId == key);
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
            var model = await _context.ItemImages.FirstOrDefaultAsync(item => item.ItemImageId == key);

            _context.ItemImages.Remove(model);
            await _context.SaveChangesAsync();
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

        private void PopulateModel(ItemImage model, IDictionary values) {
            string ITEM_IMAGE_ID = nameof(ItemImage.ItemImageId);
            string ITEM_ID = nameof(ItemImage.ItemId);
            string IMAGE_URL = nameof(ItemImage.ImageUrl);

            if(values.Contains(ITEM_IMAGE_ID)) {
                model.ItemImageId = Convert.ToInt32(values[ITEM_IMAGE_ID]);
            }

            if(values.Contains(ITEM_ID)) {
                model.ItemId = Convert.ToInt32(values[ITEM_ID]);
            }

            if(values.Contains(IMAGE_URL)) {
                model.ImageUrl = Convert.ToString(values[IMAGE_URL]);
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