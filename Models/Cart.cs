﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace Pharmacy.Models
{
    public partial class Cart
    {
      
        public int CartId { get; set; }
        public DateTime? CartDate { get; set; }
        public int? CustomerId { get; set; }
        public int? ItemId { get; set; }
        public string Remarks { get; set; }
        public int? QTY { get; set; }
        public double? UnitPrice { get; set; }
        public double? Total { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Item Item { get; set; }
    }
}