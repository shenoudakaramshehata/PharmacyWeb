﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace Pharmacy.Models
{
    public partial class Customer
    {
        public Customer()
        {
            CustomerFav = new HashSet<CustomerFav>();
            Order = new HashSet<Order>();
            Cart = new HashSet<Cart>();
            Notifications = new HashSet<Notifications>();
        }

        public int CustomerId { get; set; }
        public string CustomerNameAr { get; set; }
        public string CustomerNameEn { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerImage { get; set; }
        public string CustomerRemarks { get; set; }

        public virtual ICollection<CustomerFav> CustomerFav { get; set; }
        public virtual ICollection<Order> Order { get; set; }
        public virtual ICollection<Cart> Cart { get; set; }

        public virtual ICollection<Notifications> Notifications { get; set; }

    }
}