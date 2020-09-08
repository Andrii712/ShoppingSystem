using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using ShoppingSystem.Data;
using ShoppingSystem.Models.Entity;

namespace ShoppingSystem.Controllers
{
    [Route("[controller]")]
    public class SupermarketsController : Controller
    {
        private readonly ShoppingContext context;

        public SupermarketsController(ShoppingContext context)
        {
            this.context = context;
        }

        #region View supermarkets

        [HttpGet("[action]/{*allparams}")]
        [HttpGet("")]
        public async Task<IActionResult> Index(
            [FromQuery] string sortOrder, [FromQuery] string searchString,
            [FromQuery] string currentFilter, [FromQuery] int? pageNumber)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentSort"] = sortOrder;

            IQueryable<SupermarketModel> supermarkets = context.Supermarkets.AsNoTracking();
            int pageSize = 3;

            if (!String.IsNullOrEmpty(searchString))
            {
                pageNumber = 1;
                supermarkets = context.Supermarkets
                    .AsNoTracking()
                    .Where(s => s.Name.Contains(searchString));
            }
            else
            {
                searchString = currentFilter;
            }

            switch (sortOrder)
            {
                case "name_desc":
                    supermarkets = supermarkets.OrderByDescending(s => s.Name);
                    break;
                default:
                    supermarkets = supermarkets.OrderBy(s => s.Name);
                    break;
            }
            //return View(supermarkets);
            return View(await PaginatedList<SupermarketModel>.CreateAsync(
                    supermarkets, pageNumber ?? 1, pageSize));
        }

        [HttpGet("details/{id?}")]
        public async Task<IActionResult> View(int? id)
        {
            if (id is null) return NotFound();

            SupermarketModel supermarket = await GetSupermarketByIdAsync(id);

            if (supermarket is null) return NotFound();

            return View(supermarket);
        }

        #endregion

        #region Edit product

        [HttpGet("Edit/{id?}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return NotFound();

            SupermarketModel supermarket = await GetSupermarketByIdAsync(id);

            if (supermarket is null) return NotFound();

            return View(supermarket);
        }

        [HttpPost("Edit/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id is null) return NotFound();

            SupermarketModel supermarketToUpdate = await context.Supermarkets
                .FirstOrDefaultAsync(sm => sm.ID.Equals(id));

            if (await TryUpdateModelAsync<SupermarketModel>(
                supermarketToUpdate,
                "",
                sm => sm.Name, sm => sm.Address))
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
            return View(supermarketToUpdate);
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
        public async Task<IActionResult> Create(SupermarketModel supermarket)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    context.Supermarkets.Add(supermarket);
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
            return View(supermarket);
        }

        #endregion

        #region Delete supermarket

        [HttpGet("delete/{id?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            SupermarketModel supermarket = await GetSupermarketByIdAsync(id);

            if (supermarket is null) return NotFound();

            return View(supermarket);
        }

        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            SupermarketModel supermarket = await context.Supermarkets.FindAsync(id);
            if (!(supermarket is null))
            {
                try
                {
                    context.Supermarkets.Remove(supermarket);
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
        private async Task<SupermarketModel> GetSupermarketByIdAsync(int? id)
        {
            return await context.Supermarkets
                .AsNoTracking()
                .FirstOrDefaultAsync(product => product.ID.Equals(id));
        }
    }
}
