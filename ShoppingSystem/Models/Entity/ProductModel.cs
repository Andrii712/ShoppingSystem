using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Mvc;

namespace ShoppingSystem.Models.Entity
{
    [Bind("Name,Price,OrderDetails")]
    public class ProductModel
    {
        [Column("id")]
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [Column("name", TypeName = "varchar(50)")]
        [Required]
        [StringLength(50, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        public string Name { get; set; }

        [Column("price", TypeName = "float")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F}")]
        [Range(0, 10000, ErrorMessage = "The price must be between 0 and 10000.")]
        public double Price { get; set; }

        public OrderDetailsModel OrderDetails { get; set; }
    }
}
