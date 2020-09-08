using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Mvc;

namespace ShoppingSystem.Models.Entity
{
    [Bind("Id,CustomerModelId,CustomerModel,SupermarketModelId,SupermarketModel,OrderDate,OrderDetails")]
    public class OrderModel
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("customer_id")]
        [ForeignKey("CustomerModel")]
        [Display(Name = "Customer")]
        public int CustomerModelId { get; set; }

        [Display(Name = "Customer")]
        public CustomerModel CustomerModel { get; set; }

        [Column("supermarket_id")]
        [ForeignKey("SupermarketModel")]
        [Display(Name = "Supermarket")]
        public int SupermarketModelId { get; set; }
        
        [Display(Name = "Supermarket")]
        public SupermarketModel SupermarketModel { get; set; }

        [Column("order_date")]
        public DateTime OrderDate { get; set; }

        //public OrderDetailsModel OrderDetails { get; set; }
        public ICollection<OrderDetailsModel> OrderDetails { get; set; }
    }
}
