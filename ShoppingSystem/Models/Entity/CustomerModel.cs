using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Mvc;

namespace ShoppingSystem.Models.Entity
{
    [Bind("FirstName,LastName,Address,Discount,Orders")]
    public class CustomerModel
    {
        [Column("id")]
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [Column("fname", TypeName = "varchar(50)")]
        [Display(Name = "First Name")]
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "{0} length must be between {2} and {1}.")]
        public string FirstName { get; set; }

        [Column("lname", TypeName = "varchar(50)")]
        [Display(Name = "Last Name")]
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "{0} length must be between {2} and {1}.")]
        public string LastName { get; set; }

        [Column("address",TypeName = "varchar(50)")]
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "{0} length must be between {2} and {1}.")]
        public string Address { get; set; }

        [Column("discount",TypeName = "varchar(3)")]
        [StringLength(3, ErrorMessage = "{0} length should be less than or equal to {1}.")]
        public string Discount { get; set; }

        public List<OrderModel> Orders { get; set; }
    }
}
