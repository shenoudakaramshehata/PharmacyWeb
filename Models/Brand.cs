﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace Pharmacy.Models
{
    public partial class Brand
    {
        public Brand()
        {
            Item = new HashSet<Item>();
        }

        public int BrandId { get; set; }
        public string BrandTlAr { get; set; }
        public string BrandTlEn { get; set; }
        public string BrandPic { get; set; }
        public int? BrandSort { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Item> Item { get; set; }
    }
}