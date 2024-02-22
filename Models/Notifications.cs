using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Models
{
    public class Notifications
    {
        [Key]
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime? Date { get; set; }
        public bool IsReaded { get; set; }
       
       
        public int CustomerId { get; set; } 
        public virtual Customer Customer { get; set; }

    }
}
