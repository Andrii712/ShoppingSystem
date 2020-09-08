using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using ShoppingSystem.Data;
using ShoppingSystem.Models.Entity;

namespace ShoppingSystem.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ShoppingContext _context;

        public OrdersController(ShoppingContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? id)
        {
            ViewBag.Id = id;
            ViewBag.OrderWithDetails = _context.Orders
                .Include(o => o.OrderDetails).ThenInclude(o=>o.ProductModel)
                .FirstOrDefault(m => m.Id == id);
            var shoppingContext = _context.Orders
                    .Include(o => o.CustomerModel)
                    .Include(o => o.SupermarketModel);
            return View(shoppingContext.ToList());
        }


        public IActionResult Create()
        {
            ViewData["CustomerModelId"] = new SelectList(_context.Customers, "ID", "FirstName");
            ViewData["SupermarketModelId"] = new SelectList(_context.Supermarkets, "ID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerModelId,SupermarketModelId,OrderDate")] OrderModel orderModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerModelId"] = new SelectList(_context.Customers, "ID", "FirstName", orderModel.CustomerModelId);
            ViewData["SupermarketModelId"] = new SelectList(_context.Supermarkets, "ID", "Name", orderModel.SupermarketModelId);
            return View(orderModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderModel = await _context.Orders.FindAsync(id);
            if (orderModel == null)
            {
                return NotFound();
            }
            ViewData["CustomerModelId"] = new SelectList(_context.Customers, "ID", "FirstName", orderModel.CustomerModelId);
            ViewData["SupermarketModelId"] = new SelectList(_context.Supermarkets, "ID", "Name", orderModel.SupermarketModelId);
            return View(orderModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerModelId,SupermarketModelId,OrderDate")] OrderModel orderModel)
        {
            if (id != orderModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderModelExists(orderModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerModelId"] = new SelectList(_context.Customers, "ID", "Address", orderModel.CustomerModelId);
            ViewData["SupermarketModelId"] = new SelectList(_context.Supermarkets, "ID", "Address", orderModel.SupermarketModelId);
            return View(orderModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderModel = await _context.Orders
                .Include(o => o.CustomerModel)
                .Include(o => o.SupermarketModel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderModel == null)
            {
                return NotFound();
            }

            return View(orderModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderModel = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(orderModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderModelExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
