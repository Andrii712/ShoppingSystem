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
    public class OrderDetailsController : Controller
    {
        private readonly ShoppingContext _context;

        public OrderDetailsController(ShoppingContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetailsModel = await _context.OrderDetails.FindAsync(id);
            if (orderDetailsModel == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", orderDetailsModel.OrderId);
            ViewData["ProductModelId"] = new SelectList(_context.Products, "ID", "Name", orderDetailsModel.ProductModel);
            return View(orderDetailsModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderId,ProductModelId,Quantity")] OrderDetailsModel orderDetailsModel)
        {
            if (id != orderDetailsModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderDetailsModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderDetailsModelExists(orderDetailsModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index","Orders");
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", orderDetailsModel.OrderId);
            ViewData["ProductModelId"] = new SelectList(_context.Products, "ID", "Name", orderDetailsModel.ProductModel);
            return View(orderDetailsModel);
        }

        // GET: OrderDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetailsModel = await _context.OrderDetails
                .Include(o => o.Order)
                .Include(o => o.ProductModel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderDetailsModel == null)
            {
                return NotFound();
            }

            return View(orderDetailsModel);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderDetailsModel = await _context.OrderDetails.FindAsync(id);
            _context.OrderDetails.Remove(orderDetailsModel);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Orders");
        }

        private bool OrderDetailsModelExists(int id)
        {
            return _context.OrderDetails.Any(e => e.Id == id);
        }
    }
}
