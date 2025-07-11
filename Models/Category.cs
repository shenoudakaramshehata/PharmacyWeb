﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace Pharmacy.Models
{
    public partial class Category
    {
        public Category()
        {
            Item = new HashSet<Item>();
        }
        public int CategoryId { get; set; }
        public string CategoryTlAr { get; set; }
        public string CategoryTlEn { get; set; }
        public string CategoryPic { get; set; }
        public int? CategorySort { get; set; }
        public bool? IsActive { get; set; }
        public int? SectionId { get; set; }
        [JsonIgnore]

        public virtual Section Section{ get; set; }
        public virtual ICollection<Item> Item { get; set; }

    }
}