﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace Pharmacy.Models
{
    public partial class OrderItem
    {
        public int OrderItemId { get; set; }
        public int? OrderId { get; set; }
        public int ItemId { get; set; }
        public double? ItemPrice { get; set; }
        public int? Qty { get; set; }
        public double? Total { get; set; }
        public string Remakrs { get; set; }
        [JsonIgnore]
        public virtual Item Item { get; set; }
        [JsonIgnore]
        public virtual Order Order { get; set; }
    }
}