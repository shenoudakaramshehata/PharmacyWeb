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

    public class OrderItemsController : Controller
    {
        private PharmacyContext _context;

        public OrderItemsController(PharmacyContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var orderitems = _context.OrderItems.Select(i => new {
                i.OrderItemId,
                i.OrderId,
                i.ItemId,
                i.ItemPrice,
                i.Qty,
                i.Total,
                i.Remakrs
            });

            // If you work with a large amount of data, consider specifying the PaginateViaPrimaryKey and PrimaryKey properties.
            // In this case, keys and data are loaded in separate queries. This can make the SQL execution plan more efficient.
            // Refer to the topic https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "OrderItemId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(orderitems, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new OrderItem();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.OrderItems.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.OrderItemId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.OrderItems.FirstOrDefaultAsync(item => item.OrderItemId == key);
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
            var model = await _context.OrderItems.FirstOrDefaultAsync(item => item.OrderItemId == key);

            _context.OrderItems.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> ItemsLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Items
                         orderby i.ItemTlAr
                         select new {
                             Value = i.ItemId,
                             Text = i.ItemTlAr
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> OrdersLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Orders
                         orderby i.OrderSerial
                         select new {
                             Value = i.OrderId,
                             Text = i.OrderSerial
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(OrderItem model, IDictionary values) {
            string ORDER_ITEM_ID = nameof(OrderItem.OrderItemId);
            string ORDER_ID = nameof(OrderItem.OrderId);
            string ITEM_ID = nameof(OrderItem.ItemId);
            string ITEM_PRICE = nameof(OrderItem.ItemPrice);
            string QTY = nameof(OrderItem.Qty);
            string TOTAL = nameof(OrderItem.Total);
            string REMAKRS = nameof(OrderItem.Remakrs);

            if(values.Contains(ORDER_ITEM_ID)) {
                model.OrderItemId = Convert.ToInt32(values[ORDER_ITEM_ID]);
            }

            if(values.Contains(ORDER_ID)) {
                model.OrderId = values[ORDER_ID] != null ? Convert.ToInt32(values[ORDER_ID]) : (int?)null;
            }

            if(values.Contains(ITEM_ID)) {
                model.ItemId = Convert.ToInt32(values[ITEM_ID]);
            }

            if(values.Contains(ITEM_PRICE)) {
                model.ItemPrice = values[ITEM_PRICE] != null ? Convert.ToDouble(values[ITEM_PRICE], CultureInfo.InvariantCulture) : (double?)null;
            }

            if(values.Contains(QTY)) {
                model.Qty = values[QTY] != null ? Convert.ToInt32(values[QTY]) : (int?)null;
            }

            if(values.Contains(TOTAL)) {
                model.Total = values[TOTAL] != null ? Convert.ToDouble(values[TOTAL], CultureInfo.InvariantCulture) : (double?)null;
            }

            if(values.Contains(REMAKRS)) {
                model.Remakrs = Convert.ToString(values[REMAKRS]);
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