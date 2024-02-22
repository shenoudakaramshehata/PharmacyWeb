using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharmacy.Models
{
    public class InvoiceVM
    {
        public DateTime? OrderDate { get; set; }
        public string OrderSerial { get; set; }
        public string PaymentTitle { get; set; }
        public string customername { get; set; }
        public string deliveryName { get; set; }
        public string ItemTitle { get; set; }
        public double? ItemPrice { get; set; }
        public int? Qty { get; set; }
        public double? Total { get; set; }
        public double? ItemTotal { get; set; }
        public string ItemDescription { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime invoicedate { get; set; }
        public string CustomerAddress{ get; set; }

    }
}
