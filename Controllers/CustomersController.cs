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

    public class CustomersController : Controller
    {
        private PharmacyContext _context;

        public CustomersController(PharmacyContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var customers = _context.Customers.Select(i => new {
                i.CustomerId,
                i.CustomerNameAr,
                i.CustomerNameEn,
                i.CustomerAddress,
                i.CustomerPhone,
                i.CustomerEmail,
                i.CustomerImage,
                i.CustomerRemarks
            });

            // If you work with a large amount of data, consider specifying the PaginateViaPrimaryKey and PrimaryKey properties.
            // In this case, keys and data are loaded in separate queries. This can make the SQL execution plan more efficient.
            // Refer to the topic https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "CustomerId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(customers, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Customer();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Customers.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.CustomerId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Customers.FirstOrDefaultAsync(item => item.CustomerId == key);
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
            var model = await _context.Customers.FirstOrDefaultAsync(item => item.CustomerId == key);

            _context.Customers.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(Customer model, IDictionary values) {
            string CUSTOMER_ID = nameof(Customer.CustomerId);
            string CUSTOMER_NAME_AR = nameof(Customer.CustomerNameAr);
            string CUSTOMER_NAME_EN = nameof(Customer.CustomerNameEn);
            string CUSTOMER_ADDRESS = nameof(Customer.CustomerAddress);
            string CUSTOMER_PHONE = nameof(Customer.CustomerPhone);
            string CUSTOMER_EMAIL = nameof(Customer.CustomerEmail);
            string CUSTOMER_IMAGE = nameof(Customer.CustomerImage);
            string CUSTOMER_REMARKS = nameof(Customer.CustomerRemarks);

            if(values.Contains(CUSTOMER_ID)) {
                model.CustomerId = Convert.ToInt32(values[CUSTOMER_ID]);
            }

            if(values.Contains(CUSTOMER_NAME_AR)) {
                model.CustomerNameAr = Convert.ToString(values[CUSTOMER_NAME_AR]);
            }

            if(values.Contains(CUSTOMER_NAME_EN)) {
                model.CustomerNameEn = Convert.ToString(values[CUSTOMER_NAME_EN]);
            }

            if(values.Contains(CUSTOMER_ADDRESS)) {
                model.CustomerAddress = Convert.ToString(values[CUSTOMER_ADDRESS]);
            }

            if(values.Contains(CUSTOMER_PHONE)) {
                model.CustomerPhone = Convert.ToString(values[CUSTOMER_PHONE]);
            }

            if(values.Contains(CUSTOMER_EMAIL)) {
                model.CustomerEmail = Convert.ToString(values[CUSTOMER_EMAIL]);
            }

            if(values.Contains(CUSTOMER_IMAGE)) {
                model.CustomerImage = Convert.ToString(values[CUSTOMER_IMAGE]);
            }

            if(values.Contains(CUSTOMER_REMARKS)) {
                model.CustomerRemarks = Convert.ToString(values[CUSTOMER_REMARKS]);
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