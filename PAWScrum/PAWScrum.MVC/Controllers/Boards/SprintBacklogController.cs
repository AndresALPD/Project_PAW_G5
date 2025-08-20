using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PAWScrum.Business.Interfaces;
using PAWScrum.Data.Context;
using PAWScrum.Models;
using PAWScrum.Models.DTOs.SprintBacklog;
using PAWScrum.Services.Interfaces;

namespace PAWScrum.Mvc.Controllers
{
    public class SprintBacklogController : Controller
    {
        private readonly ISprintBacklogService _sbService;
        private readonly ISprintService _sprintService;
        private readonly IProductBacklogService _pbService;
        private readonly PAWScrumDbContext _context;

        public SprintBacklogController(
            ISprintBacklogService sbService,
            ISprintService sprintService,
            IProductBacklogService pbService,
            PAWScrumDbContext context)
        {
            _sbService = sbService;
            _sprintService = sprintService;
            _pbService = pbService;
            _context = context;
        }

        private async Task PopulateDropDownsAsync(int? selectedSprintId = null,
                                                  int? selectedBacklogId = null,
                                                  int? selectedAssignee = null)
        {
            var sprints = await _sprintService.GetAllAsync();
            ViewBag.Sprints = new SelectList(sprints, "SprintId", "SprintId", selectedSprintId);

            var pbItems = await _pbService.GetAllAsync();
            ViewBag.ProductBacklogItems = new SelectList(pbItems, "ItemId", "ItemId", selectedBacklogId);

            var userIds = await _context.Users
                .AsNoTracking()
                .Select(u => new { u.UserId })
                .OrderBy(u => u.UserId)
                .ToListAsync();

            ViewBag.Assignees = new SelectList(userIds, "UserId", "UserId", selectedAssignee);
        }

        // GET: /SprintBacklog
        public async Task<IActionResult> Index()
        {
            var items = await _sbService.GetAllAsync();
            await PopulateDropDownsAsync();
            return View(items.ToList());
        }

        // POST: /SprintBacklog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SprintBacklogCreateDto dto)
        {
            if (ModelState.IsValid)
            {
                var ok = await _sbService.CreateAsync(dto);
                if (ok) return RedirectToAction(nameof(Index));

                ModelState.AddModelError(string.Empty, "Could not create Sprint Backlog Item.");
            }

            await PopulateDropDownsAsync(dto.SprintId, dto.ProductBacklogItemId, dto.AssignedTo);
            var items = await _sbService.GetAllAsync();
            return View("Index", items.ToList());
        }

        // GET: /SprintBacklog/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _sbService.GetByIdAsync(id);
            if (item == null) return NotFound();

            var dto = new SprintBacklogCreateDto
            {
                SprintId = item.SprintId,
                ProductBacklogItemId = item.ProductBacklogItemId,
                AssignedTo = item.AssignedTo,
                Status = item.Status,
                EstimationHours = item.EstimationHours,
                CompletedHours = item.CompletedHours
            };

            await PopulateDropDownsAsync(dto.SprintId, dto.ProductBacklogItemId, dto.AssignedTo);
            ViewBag.ItemId = id;

            return View(dto);
        }

        // POST: /SprintBacklog/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SprintBacklogCreateDto dto)
        {
            if (ModelState.IsValid)
            {
                var ok = await _sbService.UpdateAsync(id, dto);
                if (ok) return RedirectToAction(nameof(Index));

                ModelState.AddModelError(string.Empty, "Could not update Sprint Backlog Item.");
            }

            await PopulateDropDownsAsync(dto.SprintId, dto.ProductBacklogItemId, dto.AssignedTo);
            ViewBag.ItemId = id;
            return View(dto);
        }

        // GET: /SprintBacklog/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _sbService.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item); 
        }

        // POST: /SprintBacklog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _sbService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

