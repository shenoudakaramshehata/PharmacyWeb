using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pharmacy.Models
{
    public class Delivery
    {
        public int DeliveryId { get; set; }
        public string Title { get; set; }
        [RegularExpression("^[0-9]+$", ErrorMessage = " Accept Number Only")]
        public string Phone1 { get; set; }
        [ RegularExpression("^[0-9]+$", ErrorMessage = " Accept Number Only")]
        public string Phone2 { get; set; }
        public string Address { get; set; }
        [EmailAddress]
        [Required, RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Not Valid")]
        public string Email { get; set; }
        public string Description { get; set; }
        [NotMapped]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{6,}$", ErrorMessage = "Should have at least one lower case , one upper case,one number , one special character and minimum length 6 characters")]
        public string Password { get; set; }

        [NotMapped]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{6,}$", ErrorMessage = "Should have at least one lower case , one upper case,one number , one special character and minimum length 6 characters")]
        public string OldPassword { get; set; }
    }
}
