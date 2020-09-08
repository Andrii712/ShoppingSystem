using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Mvc;

namespace ShoppingSystem.Models.Entity
{
    [Bind("Name,Address,Orders")]
    public class SupermarketModel
    {
        [Column("id")]
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [Column("name",TypeName = "varchar(50)")]
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "{0} length must be between {2} and {1}.")]
        public string Name { get; set; }

        [Column("address",TypeName = "varchar(150)")]
        [Required]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "{0} length must be between {2} and {1}.")]
        public string Address { get; set; }

        public List<OrderModel> Orders { get; set; }
    }
}
