using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using ShoppingSystem.Models.Entity;

namespace ShoppingSystem.Data
{
    public static class SampleData
    {
        public static async Task InitializeAsync(ShoppingContext context)
        {
            if (await context.Database.EnsureCreatedAsync()) return;

            if (context.Customers.Any()) return;

            CustomerModel[] customers = new CustomerModel[]
            {
                new CustomerModel { FirstName= "Carson", LastName="Alexander", Address="Lviv", Discount = "5" },
                new CustomerModel { FirstName="Meredith", LastName="Alonso", Address="Kyiv", Discount = "65" },
                new CustomerModel { FirstName="Arturo", LastName="Anand", Address="Mykolaiv", Discount = "3" },
                new CustomerModel { FirstName="Gytis", LastName="Barzdukas", Address="Odessa", Discount = "56" },
                new CustomerModel { FirstName="Yan", LastName="Li", Address="Lviv", Discount = "22" },
                new CustomerModel { FirstName="Peggy", LastName="Justice", Address="Lviv", Discount = "15" },
                new CustomerModel { FirstName="Laura", LastName="Norman", Address="Kyiv", Discount = "15" },
                new CustomerModel { FirstName="Nino", LastName="Olivetto", Address="London", Discount = "10" }
            };

            await context.Customers.AddRangeAsync(customers);
            await context.SaveChangesAsync();

            ProductModel[] products = new ProductModel[]
            {
                new ProductModel { Name = "iPhone X", Price = 1250 },
                new ProductModel { Name = "iPhone Xr", Price = 1350 },
                new ProductModel { Name = "iPhone XS", Price = 1450 },
                new ProductModel { Name = "iPhone 5", Price = 350 },
                new ProductModel { Name = "iPhone 6", Price = 400 },
                new ProductModel { Name = "iPhone 7", Price = 500 },
                new ProductModel { Name = "iPhone 8", Price = 600 },
                new ProductModel { Name = "iPhone 4", Price = 250 }
            };

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();

            SupermarketModel[] supermarkets = new SupermarketModel[]
            {
                new SupermarketModel { Address = "Lviv", Name = "Silpo" },
                new SupermarketModel { Address = "Kyiv", Name = "Silpo" },
                new SupermarketModel { Address = "Lviv", Name = "Metro" },
                new SupermarketModel { Address = "Mykolaiv", Name = "Nash Kray" },
                new SupermarketModel { Address = "Lviv", Name = "Rukavychka" },
                new SupermarketModel { Address = "Mykolaiv", Name = "Rukavychka" },
                new SupermarketModel { Address = "Kyiv", Name = "Rukavychka"}
            };

            await context.Supermarkets.AddRangeAsync(supermarkets);
            await context.SaveChangesAsync();

            List<Object>[] orders = new List<Object>[]
            {
                new List<Object>
                {
                    new OrderModel { CustomerModelId = 2, SupermarketModelId = 3, OrderDate = new DateTime(2012, 6, 3) },
                    new OrderDetailsModel { ProductModel = products[0], Quantity = 2 },
                    new OrderDetailsModel { ProductModel = products[1], Quantity = 2 }
                },
                new List<Object>
                {
                    new OrderModel { CustomerModelId = 1, SupermarketModelId = 6, OrderDate = new DateTime(2015,7,3) },
                    new OrderDetailsModel { ProductModel = products[1], Quantity = 1 },
                    new OrderDetailsModel { ProductModel = products[3], Quantity = 13 }
                },
                new List<Object>
                {
                    new OrderModel { CustomerModelId = 5, SupermarketModelId = 2, OrderDate = new DateTime(2011,4,3) },
                    new OrderDetailsModel { ProductModel = products[3], Quantity = 9 },
                    new OrderDetailsModel { ProductModel = products[4], Quantity = 4 }
                },
                new List<Object>
                {
                    new OrderModel { CustomerModelId = 4, SupermarketModelId = 3, OrderDate = new DateTime(2016,2,3) },
                    new OrderDetailsModel { ProductModel = products[4], Quantity = 15 },
                    new OrderDetailsModel { ProductModel = products[5], Quantity = 3 },
                },
                new List<Object>
                {
                    new OrderModel { CustomerModelId = 4, SupermarketModelId = 5, OrderDate = new DateTime(2018,8,3) },
                    new OrderDetailsModel { ProductModel = products[6], Quantity = 7 },
                    new OrderDetailsModel { ProductModel = products[1], Quantity = 17 }
                }
            };

            foreach (List<Object> item in orders)
            {
                OrderModel order = (OrderModel)item[0];
                context.Orders.Add(order);
                await context.SaveChangesAsync();

                OrderDetailsModel details1 = ((OrderDetailsModel)item[1]);
                details1.OrderId = order.Id;
                details1.Order = order;
                context.OrderDetails.Add(details1);
                await context.SaveChangesAsync();

                OrderDetailsModel details2 = ((OrderDetailsModel)item[2]);
                details2.OrderId = order.Id;
                details2.Order = order;
                context.OrderDetails.Add(details2);
                await context.SaveChangesAsync();
            }
        }
    }
}
