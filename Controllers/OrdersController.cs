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
    public class OrdersController : Controller
    {
        private PharmacyContext _context;

        public OrdersController(PharmacyContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var orders = _context.Orders.Select(i => new {
                i.OrderId,
                i.OrderDate,
                i.OrderSerial,
                i.Total,
                i.CustomerId,
                i.Addrerss,
                i.Closed,
                i.PaymentMethodId,
                i.Remarks
            });

            // If you work with a large amount of data, consider specifying the PaginateViaPrimaryKey and PrimaryKey properties.
            // In this case, keys and data are loaded in separate queries. This can make the SQL execution plan more efficient.
            // Refer to the topic https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "OrderId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(orders, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> GetByCustomerId(int customerId, DataSourceLoadOptions loadOptions) {
            var orders = _context.Orders.Where(c=>c.CustomerId== customerId).Select(i => new {
                i.OrderId,
                i.OrderDate,
                i.OrderSerial,
                i.Total,
                i.CustomerId,
                i.Addrerss,
                i.Closed,
                i.PaymentMethodId,
                i.Remarks
            });

            return Json(await DataSourceLoader.LoadAsync(orders, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Order();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Orders.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.OrderId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Orders.FirstOrDefaultAsync(item => item.OrderId == key);
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
            var model = await _context.Orders.FirstOrDefaultAsync(item => item.OrderId == key);

            _context.Orders.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> CustomersLookup(DataSourceLoadOptions loadOptions) {
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var lookupEn = from i in _context.Customers
                               orderby i.CustomerNameEn
                               select new
                               {
                                   Value = i.CustomerId,
                                   Text = i.CustomerNameEn
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }
            var lookupAr = from i in _context.Customers
                           orderby i.CustomerNameAr
                           select new
                           {
                               Value = i.CustomerId,
                               Text = i.CustomerNameAr
                           };
            return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));
 }
        [HttpGet]
        public async Task<IActionResult> DeliveriesLookup(DataSourceLoadOptions loadOptions)
        {
            
                var lookupEn = from i in _context.Deliveries
                               orderby i.DeliveryId
                               select new
                               {
                                   Value = i.DeliveryId,
                                   Text = i.Title
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            
        }


        [HttpGet]
        public async Task<IActionResult> PaymentMethodLookup(DataSourceLoadOptions loadOptions) {

            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var BrowserCulture = locale.RequestCulture.UICulture.ToString();

            if (BrowserCulture == "en-US")
            {
                var lookupEn = from i in _context.PaymentMethods
                               orderby i.PaymentMethodTlEn
                               select new
                               {
                                   Value = i.PaymentMethodId,
                                   Text = i.PaymentMethodTlEn
                               };
                return Json(await DataSourceLoader.LoadAsync(lookupEn, loadOptions));
            }
            var lookupAr = from i in _context.PaymentMethods
                           orderby i.PaymentMethodTlAr
                           select new
                           {
                               Value = i.PaymentMethodId,
                               Text = i.PaymentMethodTlAr
                           };
            return Json(await DataSourceLoader.LoadAsync(lookupAr, loadOptions));
 }

        private void PopulateModel(Order model, IDictionary values) {
            string ORDER_ID = nameof(Order.OrderId);
            string ORDER_DATE = nameof(Order.OrderDate);
            string ORDER_SERIAL = nameof(Order.OrderSerial);
            string TOTAL = nameof(Order.Total);
            string CUSTOMER_ID = nameof(Order.CustomerId);
            string ADDRERSS = nameof(Order.Addrerss);
            string CLOSED = nameof(Order.Closed);
            string PAYMENT_METHOD_ID = nameof(Order.PaymentMethodId);
            string REMARKS = nameof(Order.Remarks);

            if(values.Contains(ORDER_ID)) {
                model.OrderId = Convert.ToInt32(values[ORDER_ID]);
            }

            if(values.Contains(ORDER_DATE)) {
                model.OrderDate = values[ORDER_DATE] != null ? Convert.ToDateTime(values[ORDER_DATE]) : (DateTime?)null;
            }

            if(values.Contains(ORDER_SERIAL)) {
                model.OrderSerial = Convert.ToString(values[ORDER_SERIAL]);
            }

            if(values.Contains(TOTAL)) {
                model.Total = values[TOTAL] != null ? Convert.ToDouble(values[TOTAL], CultureInfo.InvariantCulture) : (double?)null;
            }

            if(values.Contains(CUSTOMER_ID)) {
                model.CustomerId = values[CUSTOMER_ID] != null ? Convert.ToInt32(values[CUSTOMER_ID]) : (int?)null;
            }

            if(values.Contains(ADDRERSS)) {
                model.Addrerss = Convert.ToString(values[ADDRERSS]);
            }

            if(values.Contains(CLOSED)) {
                model.Closed = values[CLOSED] != null ? Convert.ToBoolean(values[CLOSED]) : (bool?)null;
            }

            if(values.Contains(PAYMENT_METHOD_ID)) {
                model.PaymentMethodId = Convert.ToInt32(values[PAYMENT_METHOD_ID]);
            }

            if(values.Contains(REMARKS)) {
                model.Remarks = Convert.ToString(values[REMARKS]);
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