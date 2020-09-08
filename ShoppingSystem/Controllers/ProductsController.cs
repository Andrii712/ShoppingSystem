using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using ShoppingSystem.Data;
using ShoppingSystem.Models.Entity;

namespace ShoppingSystem.Controllers
{
    [Route("[controller]")]
    public class ProductsController : Controller
    {
        private readonly ShoppingContext context;

        public ProductsController(ShoppingContext context)
        {
            this.context = context;
        }

        #region View product

        [HttpGet("[action]/{*allparams}")]
        [HttpGet("")]
        public async Task<IActionResult> Index([FromQuery] string sortOrder, [FromQuery] string searchString)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["CurrentFilter"] = searchString;

            IEnumerable<ProductModel> products;

            if (!String.IsNullOrEmpty(searchString))
            {
                products = await context.Products
                    .AsNoTracking()
                    .Where(p => p.Name.Contains(searchString))
                    .ToListAsync();
            }
            else
                products = await context.Products.AsNoTracking().ToListAsync();

            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(p => p.Name);
                    break;
                default:
                    products = products.OrderBy(p => p.Name);
                    break;
            }
            return View(products);
        }

        [HttpGet("details/{id?}")]
        public async Task<IActionResult> View(int? id)
        {
            if (id is null) return NotFound();

            ProductModel product = await GetProductByIdAsync(id);

            if (product is null) return NotFound();

            return View(product);
        }

        #endregion

        #region Edit product

        [HttpGet("Edit/{id?}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return NotFound();

            ProductModel product = await GetProductByIdAsync(id);

            if (product is null) return NotFound();

            return View(product);
        }

        [HttpPost("Edit/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id is null) return NotFound();

            ProductModel productToUpdate = await context.Products
                .FirstOrDefaultAsync(product => product.ID.Equals(id));

            if (await TryUpdateModelAsync<ProductModel>(
                productToUpdate,
                "",
                p => p.Name, p => p.Price))
            {
                try
                {
                    await context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(productToUpdate);
        }

        #endregion

        #region Create product

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductModel product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    context.Products.Add(product);
                    await context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex */)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }

            return View(product);
        }

        #endregion

        #region Delete product

        [HttpGet("delete/{id?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            ProductModel product = await GetProductByIdAsync(id);

            if (product is null) return NotFound();

            return View(product);
        }

        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ProductModel product = await context.Products.FindAsync(id);
            if (!(product is null))
            {
                try
                {
                    context.Products.Remove(product);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("",
                        "Delete failed. Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion

        [NonAction]
        private async Task<ProductModel> GetProductByIdAsync(int? id)
        {
            return await context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(product => product.ID.Equals(id));
        }
    }
}
