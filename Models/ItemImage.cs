﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace Pharmacy.Models
{
    public partial class ItemImage
    {
        public int ItemImageId { get; set; }
        public int ItemId { get; set; }
        public string ImageUrl { get; set; }

        public virtual Item Item { get; set; }
    }
}