﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace Pharmacy.Models
{
    public partial class CustomerFav
    {
        public int CustomerFavId { get; set; }
        public int CustomerId { get; set; }
        public int ItemId { get; set; }

        public virtual Customer Customer { get; set; }
    }
}