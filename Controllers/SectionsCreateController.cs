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

    public class SectionsCreateController : Controller
    {
        private PharmacyContext _context;

        public SectionsCreateController(PharmacyContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var sections = _context.Sections.Select(i => new {
                i.SectionId,
                i.SectionTlAr,
                i.SectionTlEn,
                i.SectionPic,
                i.SectionSort,
                i.IsActive
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "SectionId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(sections, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Section();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Sections.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.SectionId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Sections.FirstOrDefaultAsync(item => item.SectionId == key);
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
            var model = await _context.Sections.FirstOrDefaultAsync(item => item.SectionId == key);

            _context.Sections.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(Section model, IDictionary values) {
            string SECTION_ID = nameof(Section.SectionId);
            string SECTION_TL_AR = nameof(Section.SectionTlAr);
            string SECTION_TL_EN = nameof(Section.SectionTlEn);
            string SECTION_PIC = nameof(Section.SectionPic);
            string SECTION_SORT = nameof(Section.SectionSort);
            string IS_ACTIVE = nameof(Section.IsActive);

            if(values.Contains(SECTION_ID)) {
                model.SectionId = Convert.ToInt32(values[SECTION_ID]);
            }

            if(values.Contains(SECTION_TL_AR)) {
                model.SectionTlAr = Convert.ToString(values[SECTION_TL_AR]);
            }

            if(values.Contains(SECTION_TL_EN)) {
                model.SectionTlEn = Convert.ToString(values[SECTION_TL_EN]);
            }

            if(values.Contains(SECTION_PIC)) {
                model.SectionPic = Convert.ToString(values[SECTION_PIC]);
            }

            if(values.Contains(SECTION_SORT)) {
                model.SectionSort = values[SECTION_SORT] != null ? Convert.ToInt32(values[SECTION_SORT]) : (int?)null;
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