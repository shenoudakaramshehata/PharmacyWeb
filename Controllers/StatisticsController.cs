using DevExtreme.AspNet.Mvc;
using Pharmacy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pharmacy.Data;
using System.Globalization;

namespace Pharmacy.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]

    public class StatisticsController : Controller
    {
        private PharmacyContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StatisticsController(Pharmacy.Data.PharmacyContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public object GetDailyOrder(DataSourceLoadOptions loadOptions)
        {
            var dailyOrder = _context.Orders
                .Where(o => o.OrderDate.Value.Year == DateTime.Now.Year)
                .GroupBy(c => c.OrderDate.Value.Date).

                Select(g => new
                {

                    day = g.Key,

                    sales = g.Count()

                }).OrderBy(r => r.day).ThenBy(r => DateTime.Now.Month);

            return dailyOrder;


        }

        [HttpGet]
        public object GetMonthlyOrder(DataSourceLoadOptions loadOptions)
        {
            var dailyOrder = _context.Orders
                .Where(o => o.OrderDate.Value.Year == DateTime.Now.Year)
                .GroupBy(c => c.OrderDate.Value.Date.Month).

                Select(g => new
                {

                    day = g.Key,

                    sales = g.Count()

                }).OrderByDescending(r => r.day);

            return dailyOrder;


        }

        [HttpGet]
        public object GetMonthlyOrdersRevenue(DataSourceLoadOptions loadOptions)
        {
            var monthlyOrder = _context.Orders
                .Where(o => o.OrderDate.Value.Year == DateTime.Now.Year)
                .GroupBy(c => c.OrderDate.Value.Date.Month).

                Select(g => new
                {

                    day = g.Key,

                    sales = g.Sum(s => s.Total)

                }).OrderByDescending(r => r.day);

            return monthlyOrder;


        }
        [HttpGet]
        public object GetDailyOrdersRevenue(DataSourceLoadOptions loadOptions)
        {
            var dailyOrder = _context.Orders
                .Where(o => o.OrderDate.Value.Year == DateTime.Now.Year)
                .GroupBy(c => c.OrderDate.Value.Date).

                Select(g => new
                {

                    day = g.Key,

                    sales = g.Sum(s => s.Total)

                }).OrderByDescending(r => r.day);

            return dailyOrder;


        }

    }
}
