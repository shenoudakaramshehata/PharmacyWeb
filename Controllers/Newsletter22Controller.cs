using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Data;
using Pharmacy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharmacy.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]

    public class Newsletter22Controller : Controller
    {
        private readonly PharmacyContext _context;
        public Newsletter22Controller(PharmacyContext context)
        {
            _context = context;
        }
        [BindProperty]
        public List<Newsletter> Newsletters  { get; set; }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            Newsletters =await _context.Newsletters.ToListAsync();

            return Json(Newsletters);
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
