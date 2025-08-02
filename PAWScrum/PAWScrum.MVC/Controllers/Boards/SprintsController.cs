using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PAWScrum.Business.Interfaces;
using PAWScrum.Data.Context;
using PAWScrum.Models;
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
            return View(sprints);
        }

        // GET: /Sprints/Create
        public async Task<IActionResult> Create()
        {
            var projects = await _projectService.GetAllAsync();
            ViewBag.Projects = new SelectList(projects, "ProjectId", "ProjectName");
            return View();
        }

        // POST: /Sprints/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Sprints sprint)
        {
            if (ModelState.IsValid)
            {
                await _sprintService.CreateAsync(sprint);
                return RedirectToAction(nameof(Index));
            }

            var projects = await _projectService.GetAllAsync();
            ViewBag.Projects = new SelectList(projects, "ProjectId", "ProjectName", sprint.ProjectId);
            return View(sprint);
        }

        // GET: /Sprints/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var sprint = await _sprintService.GetByIdAsync(id);
            if (sprint == null)
                return NotFound();

            var projects = await _projectService.GetAllAsync();
            ViewBag.Projects = new SelectList(projects, "ProjectId", "ProjectName", sprint.ProjectId);
            return View(sprint);
        }

        // POST: /Sprints/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Sprints sprint)
        {
            if (ModelState.IsValid)
            {
                await _sprintService.UpdateAsync(sprint);
                return RedirectToAction(nameof(Index));
            }

            var projects = await _projectService.GetAllAsync();
            ViewBag.Projects = new SelectList(projects, "ProjectId", "ProjectName", sprint.ProjectId);
            return View(sprint);
        }

        // GET: /Sprints/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var sprint = await _sprintService.GetByIdAsync(id);
            if (sprint == null)
                return NotFound();

            return View(sprint);
        }

        // POST: /Sprints/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _sprintService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
