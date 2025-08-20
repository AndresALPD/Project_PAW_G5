using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PAWScrum.Business.Interfaces;
using PAWScrum.Data.Context;
using PAWScrum.Models;
using PAWScrum.Services.Interfaces;
using PAWScrum.Models.DTOs.ProductBacklog;

namespace PAWScrum.Mvc.Controllers
{
    public class ProductBacklogController : Controller
    {
        private readonly IProductBacklogService _pbService;
        private readonly IProjectService _projectService;

        public ProductBacklogController(IProductBacklogService pbService, IProjectService projectService)
        {
            _pbService = pbService;
            _projectService = projectService;
        }

        // GET: /ProductBacklog
        public async Task<IActionResult> Index()
        {
            var items = await _pbService.GetAllAsync();

            var projects = await _projectService.GetAllAsync();
            ViewBag.Projects = new SelectList(projects, "ProjectId", "ProjectName");

            return View(items.ToList());
        }

        // POST: /ProductBacklog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductBacklogCreateDto dto)
        {
            if (ModelState.IsValid)
            {
                var ok = await _pbService.CreateAsync(dto);
                if (ok) return RedirectToAction(nameof(Index));

                ModelState.AddModelError(string.Empty, "Could not create the backlog item.");
            }

            var projects = await _projectService.GetAllAsync();
            ViewBag.Projects = new SelectList(projects, "ProjectId", "ProjectName", dto.ProjectId);

            var items = await _pbService.GetAllAsync();
            return View("Index", items.ToList());
        }

        // GET: /ProductBacklog/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _pbService.GetByIdAsync(id);
            if (item == null) return NotFound();

            var dto = new ProductBacklogCreateDto
            {
                ProjectId = item.ProjectId,
                Title = item.Title,
                Description = item.Description,
                Priority = item.Priority,
                Status = item.Status
            };

            var projects = await _projectService.GetAllAsync();
            ViewBag.Projects = new SelectList(projects, "ProjectId", "ProjectName", dto.ProjectId);
            ViewBag.ItemId = id;

            return View(dto);
        }

        // POST: /ProductBacklog/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductBacklogCreateDto dto)
        {
            if (ModelState.IsValid)
            {
                var ok = await _pbService.UpdateAsync(id, dto);
                if (ok) return RedirectToAction(nameof(Index));

                ModelState.AddModelError(string.Empty, "Could not update the backlog item.");
            }

            var projects = await _projectService.GetAllAsync();
            ViewBag.Projects = new SelectList(projects, "ProjectId", "ProjectName", dto.ProjectId);
            ViewBag.ItemId = id;

            return View(dto);
        }

        // GET: /ProductBacklog/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _pbService.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item); // vista usa ProductBacklogDto
        }

        // POST: /ProductBacklog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _pbService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
