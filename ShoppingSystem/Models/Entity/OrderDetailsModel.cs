using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Mvc;

namespace ShoppingSystem.Models.Entity
{
    [Bind("Id,OrderId,ProductModelId,Quantity")]
    public class OrderDetailsModel
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("order_id")]
        [ForeignKey("Order")]
        public int OrderId { get; set; }

        public OrderModel Order { get; set; }

        [Column("product_id")]
        [ForeignKey("ProductModel")]
        public int ProductModelId { get; set; }

        public ProductModel ProductModel { get; set; }

        [Column("quantity",TypeName = "float")]
        public double Quantity { get; set; }
    }
}
