﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Pharmacy.Models
{
    public partial class Area
    {

        public int AreaId { get; set; }
        public int CityId { get; set; }
        public string AreaTlAr { get; set; }
        public string AreaTlEn { get; set; }
        public bool? AreaIsActive { get; set; }
        public int? AreaOrderIndex { get; set; }

        public double DeliveryCost { get; set; }
        public virtual City City { get; set; }
    }
}