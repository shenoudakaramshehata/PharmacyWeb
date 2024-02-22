﻿using DevExtreme.AspNet.Data;
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

    public class ConfigurationsController : Controller
    {
        private PharmacyContext _context;

        public ConfigurationsController(PharmacyContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var configurations = _context.Configurations.Select(i => new {
                i.ConfigurationId,
                i.Phone,
                i.Email,
                i.Address,
                i.Facebook,
                i.WhatsApp,
                i.LinkedIn,
                i.Instgram,
                i.Twitter
            });

            // If you work with a large amount of data, consider specifying the PaginateViaPrimaryKey and PrimaryKey properties.
            // In this case, keys and data are loaded in separate queries. This can make the SQL execution plan more efficient.
            // Refer to the topic https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "ConfigurationId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(configurations, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Configuration();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Configurations.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.ConfigurationId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Configurations.FirstOrDefaultAsync(item => item.ConfigurationId == key);
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
            var model = await _context.Configurations.FirstOrDefaultAsync(item => item.ConfigurationId == key);

            _context.Configurations.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(Configuration model, IDictionary values) {
            string CONFIGURATION_ID = nameof(Configuration.ConfigurationId);
            string PHONE = nameof(Configuration.Phone);
            string EMAIL = nameof(Configuration.Email);
            string ADDRESS = nameof(Configuration.Address);
            string FACEBOOK = nameof(Configuration.Facebook);
            string WHATS_APP = nameof(Configuration.WhatsApp);
            string LINKED_IN = nameof(Configuration.LinkedIn);
            string INSTGRAM = nameof(Configuration.Instgram);
            string TWITTER = nameof(Configuration.Twitter);

            if(values.Contains(CONFIGURATION_ID)) {
                model.ConfigurationId = Convert.ToInt32(values[CONFIGURATION_ID]);
            }

            if(values.Contains(PHONE)) {
                model.Phone = Convert.ToString(values[PHONE]);
            }

            if(values.Contains(EMAIL)) {
                model.Email = Convert.ToString(values[EMAIL]);
            }

            if(values.Contains(ADDRESS)) {
                model.Address = Convert.ToString(values[ADDRESS]);
            }

            if(values.Contains(FACEBOOK)) {
                model.Facebook = Convert.ToString(values[FACEBOOK]);
            }

            if(values.Contains(WHATS_APP)) {
                model.WhatsApp = Convert.ToString(values[WHATS_APP]);
            }

            if(values.Contains(LINKED_IN)) {
                model.LinkedIn = Convert.ToString(values[LINKED_IN]);
            }

            if(values.Contains(INSTGRAM)) {
                model.Instgram = Convert.ToString(values[INSTGRAM]);
            }

            if(values.Contains(TWITTER)) {
                model.Twitter = Convert.ToString(values[TWITTER]);
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