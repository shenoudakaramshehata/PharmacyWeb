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

    public class PaymentMethodsController : Controller
    {
        private PharmacyContext _context;

        public PaymentMethodsController(PharmacyContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var paymentmethods = _context.PaymentMethods.Select(i => new {
                i.PaymentMethodId,
                i.PaymentMethodTlAr,
                i.PaymentMethodTlEn
            });

            // If you work with a large amount of data, consider specifying the PaginateViaPrimaryKey and PrimaryKey properties.
            // In this case, keys and data are loaded in separate queries. This can make the SQL execution plan more efficient.
            // Refer to the topic https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "PaymentMethodId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(paymentmethods, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new PaymentMethod();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.PaymentMethods.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.PaymentMethodId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.PaymentMethods.FirstOrDefaultAsync(item => item.PaymentMethodId == key);
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
            var model = await _context.PaymentMethods.FirstOrDefaultAsync(item => item.PaymentMethodId == key);

            _context.PaymentMethods.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(PaymentMethod model, IDictionary values) {
            string PAYMENT_METHOD_ID = nameof(PaymentMethod.PaymentMethodId);
            string PAYMENT_METHOD_TL_AR = nameof(PaymentMethod.PaymentMethodTlAr);
            string PAYMENT_METHOD_TL_EN = nameof(PaymentMethod.PaymentMethodTlEn);

            if(values.Contains(PAYMENT_METHOD_ID)) {
                model.PaymentMethodId = Convert.ToInt32(values[PAYMENT_METHOD_ID]);
            }

            if(values.Contains(PAYMENT_METHOD_TL_AR)) {
                model.PaymentMethodTlAr = Convert.ToString(values[PAYMENT_METHOD_TL_AR]);
            }

            if(values.Contains(PAYMENT_METHOD_TL_EN)) {
                model.PaymentMethodTlEn = Convert.ToString(values[PAYMENT_METHOD_TL_EN]);
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