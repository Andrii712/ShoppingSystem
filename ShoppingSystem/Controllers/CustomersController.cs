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
    public class CustomersController : Controller
    {
        private readonly ShoppingContext context;

        public CustomersController(ShoppingContext context)
        {
            this.context = context;
        }

        #region View customers

        [HttpGet("[action]/{*allparams}")]
        [HttpGet("")]
        public async Task<IActionResult> Index([FromQuery] string sortOrder, [FromQuery] string searchString)
        {
            ViewData["LNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["AddressSortParm"] = String.IsNullOrEmpty(sortOrder) ? "addr_desc" : "addr";
            ViewData["CurrentFilter"] = searchString;

            IEnumerable<CustomerModel> customers;

            if (!String.IsNullOrEmpty(searchString))
            {
                customers = await context.Customers
                    .AsNoTracking()
                    .Where(c => c.LastName.Contains(searchString)
                            || c.FirstName.Contains(searchString))
                    .ToListAsync();
            }
            else
                customers = await context.Customers.AsNoTracking().ToListAsync();

            switch (sortOrder)
            {
                case "name_desc":
                    customers = customers.OrderByDescending(c => c.LastName);
                    break;
                case "addr":
                    customers = customers.OrderBy(c => c.Address);
                    break;
                case "addr_desc":
                    customers = customers.OrderByDescending(c => c.Address);
                    break;
                default:
                    customers = customers.OrderBy(c => c.LastName);
                    break;
            }
            return View(customers);
        }

        [HttpGet("details/{id?}")]
        public async Task<IActionResult> View(int? id)
        {
            if (id is null) return NotFound();

            CustomerModel customer = await context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(customer => customer.ID.Equals(id));

            if (customer is null) return NotFound();

            return View(customer);
        }

        #endregion

        #region Edit customers

        [HttpGet("Edit/{id?}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return NotFound();

            CustomerModel customer = await GetCustomerByIdAsync(id);

            if (customer is null) return NotFound();

            return View(customer);
        }

        [HttpPost("Edit/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id is null) return NotFound();

            CustomerModel customerToUpdate = await context.Customers
                .FirstOrDefaultAsync(s => s.ID.Equals(id));

            if (await TryUpdateModelAsync<CustomerModel>(
                customerToUpdate,
                "",
                c => c.FirstName, c => c.LastName, c => c.Address, c => c.Discount))
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
            return View(customerToUpdate);
        }

        #endregion

        #region Create customer

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerModel customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    context.Customers.Add(customer);
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
            return View(customer);
        }

        #endregion

        #region Delete customer

        [HttpGet("delete/{id?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            CustomerModel customer = await GetCustomerByIdAsync(id);

            if (customer is null) return NotFound();

            return View(customer);
        }

        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            CustomerModel customer = await context.Customers.FindAsync(id);
            if (!(customer is null))
            {
                try
                {
                    context.Customers.Remove(customer);
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
        private async Task<CustomerModel> GetCustomerByIdAsync(int? id)
        {
            return await context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(product => product.ID.Equals(id));
        }
    }
}
