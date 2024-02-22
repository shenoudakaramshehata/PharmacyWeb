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

    public class ContactUsController : Controller
    {
        private PharmacyContext _context;

        public ContactUsController(PharmacyContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var contactus = _context.ContactUs.Select(i => new {
                i.ContactUsId,
                i.FullName,
                i.Mobile,
                i.Email,
                i.Message
            });

            // If you work with a large amount of data, consider specifying the PaginateViaPrimaryKey and PrimaryKey properties.
            // In this case, keys and data are loaded in separate queries. This can make the SQL execution plan more efficient.
            // Refer to the topic https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "ContactUsId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(contactus, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new ContactUs();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.ContactUs.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.ContactUsId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.ContactUs.FirstOrDefaultAsync(item => item.ContactUsId == key);
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
            var model = await _context.ContactUs.FirstOrDefaultAsync(item => item.ContactUsId == key);

            _context.ContactUs.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(ContactUs model, IDictionary values) {
            string CONTACT_US_ID = nameof(ContactUs.ContactUsId);
            string FULL_NAME = nameof(ContactUs.FullName);
            string MOBILE = nameof(ContactUs.Mobile);
            string EMAIL = nameof(ContactUs.Email);
            string MESSAGE = nameof(ContactUs.Message);

            if(values.Contains(CONTACT_US_ID)) {
                model.ContactUsId = Convert.ToInt32(values[CONTACT_US_ID]);
            }

            if(values.Contains(FULL_NAME)) {
                model.FullName = Convert.ToString(values[FULL_NAME]);
            }

            if(values.Contains(MOBILE)) {
                model.Mobile = Convert.ToString(values[MOBILE]);
            }

            if(values.Contains(EMAIL)) {
                model.Email = Convert.ToString(values[EMAIL]);
            }

            if(values.Contains(MESSAGE)) {
                model.Message = Convert.ToString(values[MESSAGE]);
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