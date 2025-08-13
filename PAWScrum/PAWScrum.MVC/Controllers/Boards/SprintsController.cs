using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PAWScrum.Business.Interfaces;
using PAWScrum.Data.Context;
using PAWScrum.Models;
using PAWScrum.Models.DTOs;
using PAWScrum.Models.DTOs.Sprints;
using PAWScrum.Services.Interfaces;

namespace PAWScrum.Mvc.Controllers
{
    public class SprintsController : Controller
    {
        private readonly ISprintService _sprintService;
        private readonly IProjectService _projectService;

        public SprintsController(ISprintService sprintService, IProjectService projectService)
        {
            _sprintService = sprintService;
            _projectService = projectService;
        }

        // GET: /Sprints
        public async Task<IActionResult> Index()
        {
            var sprints = await _sprintService.GetAllAsync();
            return View(sprints.ToList()); // Lista de SprintDto
        }

        // GET: /Sprints/Create
        public async Task<IActionResult> Create()
        {
            var projects = await _projectService.GetAllAsync();
            ViewBag.Projects = new SelectList(projects, "ProjectId", "ProjectName");
            return View(new SprintCreateDto());
        }

        // POST: /Sprints/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SprintCreateDto sprint)
        {
            if (ModelState.IsValid)
            {
                bool created = await _sprintService.CreateAsync(sprint);
                if (created) return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", "No se pudo crear el sprint. Intenta de nuevo.");
            }

            var projects = await _projectService.GetAllAsync();
            ViewBag.Projects = new SelectList(projects, "ProjectId", "ProjectName", sprint.ProjectId);
            return View(sprint);
        }

        // GET: /Sprints/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var sprint = await _sprintService.GetByIdAsync(id);
            if (sprint == null) return NotFound();

            var dto = new SprintCreateDto
            {
                ProjectId = sprint.ProjectId,
                Name = sprint.Name,
                StartDate = (DateOnly)sprint.StartDate,
                EndDate = (DateOnly)sprint.EndDate,
                Goal = sprint.Goal
            };

            var projects = await _projectService.GetAllAsync();
            ViewBag.Projects = new SelectList(projects, "ProjectId", "ProjectName", dto.ProjectId);
            return View(dto);
        }

        // POST: /Sprints/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SprintCreateDto sprint)
        {
            if (ModelState.IsValid)
            {
                bool updated = await _sprintService.UpdateAsync(id, sprint);
                if (updated) return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", "No se pudo actualizar el sprint. Intenta de nuevo.");
            }

            var projects = await _projectService.GetAllAsync();
            ViewBag.Projects = new SelectList(projects, "ProjectId", "ProjectName", sprint.ProjectId);
            return View(sprint);
        }

        // GET: /Sprints/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var sprint = await _sprintService.GetByIdAsync(id);
            if (sprint == null) return NotFound();
            return View(sprint); // SprintDto
        }

        // POST: /Sprints/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _sprintService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
