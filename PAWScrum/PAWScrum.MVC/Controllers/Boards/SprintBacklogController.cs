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
        public class SprintBacklogController : Controller
        {
            private readonly ISprintBacklogService _sprintBacklogService;

            public SprintBacklogController(ISprintBacklogService sprintBacklogService)
            {
                _sprintBacklogService = sprintBacklogService;
            }

            // GET: SprintBacklog
            public async Task<IActionResult> Index()
            {
                var items = await _sprintBacklogService.GetAllAsync();
                return View(items);
            }

            // GET: SprintBacklog/Details/5
            public async Task<IActionResult> Details(int id)
            {
                var item = await _sprintBacklogService.GetByIdAsync(id);
                if (item == null)
                {
                    return NotFound();
                }
                return View(item);
            }

            // GET: SprintBacklog/Create
            public IActionResult Create()
            {
                return View();
            }

            // POST: SprintBacklog/Create
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(SprintBacklogItem item)
            {
                if (ModelState.IsValid)
                {
                    var created = await _sprintBacklogService.CreateAsync(item);
                    if (created)
                        return RedirectToAction(nameof(Index));
                    ModelState.AddModelError("", "No se pudo crear el registro");
                }
                return View(item);
            }

            // GET: SprintBacklog/Edit/5
            public async Task<IActionResult> Edit(int id)
            {
                var item = await _sprintBacklogService.GetByIdAsync(id);
                if (item == null)
                {
                    return NotFound();
                }
                return View(item);
            }

            // POST: SprintBacklog/Edit/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(int id, SprintBacklogItem item)
            {
                if (id != item.SprintItemId)
                {
                    return BadRequest();
                }

                if (ModelState.IsValid)
                {
                    var updated = await _sprintBacklogService.UpdateAsync(item);
                    if (updated)
                        return RedirectToAction(nameof(Index));
                    ModelState.AddModelError("", "No se pudo actualizar el registro");
                }
                return View(item);
            }

            // GET: SprintBacklog/Delete/5
            public async Task<IActionResult> Delete(int id)
            {
                var item = await _sprintBacklogService.GetByIdAsync(id);
                if (item == null)
                {
                    return NotFound();
                }
                return View(item);
            }

            // POST: SprintBacklog/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                var deleted = await _sprintBacklogService.DeleteAsync(id);
                if (!deleted)
                {
                    ModelState.AddModelError("", "No se pudo eliminar el registro");
                    var item = await _sprintBacklogService.GetByIdAsync(id);
                    return View(item);
                }
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
