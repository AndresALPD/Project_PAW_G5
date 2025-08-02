using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PAWScrum.Business.Interfaces;
using PAWScrum.Data.Context;
using PAWScrum.Models;
using PAWScrum.Services.Interfaces;

namespace PAWScrum.Mvc.Controllers
{
    namespace PAWScrum.Mvc.Controllers
    {
        public class ProductBacklogController : Controller
        {
            private readonly IProductBacklogService _productBacklogService;

            public ProductBacklogController(IProductBacklogService productBacklogService)
            {
                _productBacklogService = productBacklogService;
            }

            // GET: ProductBacklog
            public async Task<IActionResult> Index()
            {
                var items = await _productBacklogService.GetAllAsync();
                return View(items);
            }

            // GET: ProductBacklog/Details/5
            public async Task<IActionResult> Details(int id)
            {
                var item = await _productBacklogService.GetByIdAsync(id);
                if (item == null)
                    return NotFound();

                return View(item);
            }

            // GET: ProductBacklog/Create
            public IActionResult Create()
            {
                return View();
            }

            // POST: ProductBacklog/Create
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(ProductBacklogItem item)
            {
                if (!ModelState.IsValid)
                    return View(item);

                var success = await _productBacklogService.CreateAsync(item);
                if (success)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError(string.Empty, "Error al crear el ítem.");
                return View(item);
            }

            // GET: ProductBacklog/Edit/5
            public async Task<IActionResult> Edit(int id)
            {
                var item = await _productBacklogService.GetByIdAsync(id);
                if (item == null)
                    return NotFound();

                return View(item);
            }

            // POST: ProductBacklog/Edit/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(int id, ProductBacklogItem item)
            {
                if (id != item.ItemId)
                    return NotFound();

                if (!ModelState.IsValid)
                    return View(item);

                var success = await _productBacklogService.UpdateAsync(item);
                if (success)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError(string.Empty, "Error al actualizar el ítem.");
                return View(item);
            }

            // GET: ProductBacklog/Delete/5
            public async Task<IActionResult> Delete(int id)
            {
                var item = await _productBacklogService.GetByIdAsync(id);
                if (item == null)
                    return NotFound();

                return View(item);
            }

            // POST: ProductBacklog/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                var success = await _productBacklogService.DeleteAsync(id);
                if (!success)
                {
                    TempData["ErrorMessage"] = "No se pudo eliminar el ítem.";
                    return RedirectToAction(nameof(Delete), new { id });
                }

                return RedirectToAction(nameof(Index));
            }
        }
    }
}
