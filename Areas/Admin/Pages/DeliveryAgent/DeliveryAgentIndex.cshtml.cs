using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using Pharmacy.Data;

namespace Pharmacy.Areas.Admin.Pages.DeliveryAgent
{
    public class DeliveryAgentIndexModel : PageModel
    {
        private PharmacyContext _context;
        private readonly IToastNotification _toastNotification;
        public DeliveryAgentIndexModel(PharmacyContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }
        public void OnGet()
        {
        }
        public IActionResult OnGetGridData(DataSourceLoadOptions loadOptions)
        {
            var deliveries = _context.Deliveries.Select(e => new
            {
                e.DeliveryId,
                e.Title,
                e.Address,
                e.Email,
                e.Phone1,
                e.Phone2,
                e.Description
            }
            );
                return new JsonResult(DataSourceLoader.Load(deliveries, loadOptions));

        }

    }
}
