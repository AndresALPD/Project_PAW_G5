using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAWScrum.Data.Context;
using PAWScrum.Models;
using PAWScrum.Models.DTOs.ProjectMembers;
using PAWScrum.Repositories.Interfaces;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;


namespace PAWScrum.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly PAWScrumDbContext _context;
        private readonly IProjectRepository _projectRepository;

        public ProjectsController(PAWScrumDbContext context, IProjectRepository projectRepository)
        {
            _context = context;
            _projectRepository = projectRepository;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            var projects = await _context.Projects
                                         .Where(p => !p.IsArchived) 
                                         .Include(p => p.Owner)
                                         .ToListAsync();
            return View(projects);
        }


        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var project = await _context.Projects
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(m => m.ProjectId == id);

            if (project == null) return NotFound();

            return View(project);
        }

        // ---------- CREAR ----------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectName,ProjectKey,Description,OwnerId,Visibility,Status,StartDate,EndDate,SprintDuration,RepositoryUrl,IsArchived")] Project project)
        {
            if (ModelState.IsValid)
            {
                project.CreatedDate = DateTime.UtcNow;
                _context.Add(project);
                await _context.SaveChangesAsync();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true });
                }

                return RedirectToAction(nameof(Index));
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

                return Json(new { success = false, errors });
            }

            TempData["Error"] = "Por favor corrige los errores en el formulario";
            return RedirectToAction(nameof(Index));
        }


        // ---------- EDITAR ----------

        [HttpGet]
        public async Task<IActionResult> GetProject(int id)
        {
            try
            {
                var project = await _context.Projects
                    .Where(p => p.ProjectId == id)
                    .Select(p => new {
                        p.ProjectId,
                        p.ProjectName,
                        p.ProjectKey,
                        p.OwnerId,
                        p.Visibility,
                        p.Status,
                        p.SprintDuration,
                        p.Description,
                        p.StartDate,
                        p.EndDate,
                        p.RepositoryUrl,
                        p.IsArchived
                    })
                    .FirstOrDefaultAsync();

                if (project == null)
                    return NotFound();

                return Json(project);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener el proyecto.", detail = ex.Message });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Project model)
        {
            if (!ModelState.IsValid)
            {
                // Puedes devolver errores aquí para mostrar en el modal
                var errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );
                return Json(new { success = false, errors });
            }

            var existing = await _projectRepository.GetByIdAsync(model.ProjectId);
            if (existing == null)
                return Json(new { success = false, message = "Proyecto no encontrado." });

            existing.ProjectName = model.ProjectName;
            existing.ProjectKey = model.ProjectKey;
            existing.Description = model.Description;
            existing.OwnerId = model.OwnerId;
            existing.Visibility = model.Visibility;
            existing.Status = model.Status;
            existing.StartDate = model.StartDate;
            existing.EndDate = model.EndDate;
            existing.SprintDuration = model.SprintDuration;
            existing.RepositoryUrl = model.RepositoryUrl;
            existing.IsArchived = model.IsArchived;

            await _projectRepository.UpdateAsync(existing);

            return Json(new { success = true, message = "Proyecto actualizado correctamente." });
        }


        // ---------- ELIMINAR ----------


        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var proyecto = _context.Projects.Find(id);
                if (proyecto == null)
                {
                    return Json(new { success = false, message = "Proyecto no encontrado." });
                }

                
                proyecto.IsArchived = true;
                _context.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { success = false, message = ex.Message });
            }
        }


























        [HttpGet]
        public IActionResult GetUsers()
        {
            try
            {
                var users = _context.Users
                    .Select(u => new
                    {
                        userId = u.UserId,
                        username = u.Username,
                        firstName = u.FirstName,
                        lastName = u.LastName,
                        role = u.Role
                    })
                    .ToList();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener los usuarios: " + ex.Message });
            }
        }


        [HttpGet]
        public IActionResult GetProjectMembers(int id)
        {
            try
            {
                var members = _context.ProjectMembers
                    .Where(m => m.ProjectId == id)
                    .Select(m => new {
                        UserId=m.User.UserId,
                        firstName = m.User.FirstName,
                        lastName = m.User.LastName,
                    })
                    .ToList();

                return Ok(members);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener miembros: " + ex.Message });
            }
        }
        //ADD MEMBERS

        [HttpPost]
        public IActionResult AddMember([FromBody] AddMemberDto dto)
        {
            if (dto == null || dto.ProjectId <= 0 || dto.UserId <= 0)
            {
                return BadRequest("Datos incompletos o incorrectos.");
            }

            var exists = _context.ProjectMembers.Any(pm => pm.ProjectId == dto.ProjectId && pm.UserId == dto.UserId);
            if (exists)
            {
                return Conflict("Este usuario ya es miembro del proyecto.");
            }

            _context.ProjectMembers.Add(new ProjectMember
            {
                ProjectId = dto.ProjectId,
                UserId = dto.UserId,
            });
            _context.SaveChanges();

            var user = _context.Users.FirstOrDefault(u => u.UserId == dto.UserId);
            var project = _context.Projects.FirstOrDefault(p => p.ProjectId == dto.ProjectId);

            if (user != null && !string.IsNullOrEmpty(user.Email) && project != null)
            {
                try
                {
                    var fromAddress = new MailAddress("bricenoc506@gmail.com", "Gestor de Proyectos PAWSCRUM");
                    var toAddress = new MailAddress(user.Email, $"{user.FirstName} {user.LastName}");

                    string subject = $"Has sido añadido al proyecto {project.ProjectName}";
                    string body = $@"
                    <table style='width: 100%; background-color: #f4f4f4; padding: 20px; font-family: Arial, sans-serif;'>
                        <tr>
                            <td align='center'>
                                <table style='max-width: 600px; background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0px 2px 6px rgba(0,0,0,0.1);'>
                                    <tr>
                                        <td style='background-color: #007bff; padding: 15px; text-align: center; color: white; font-size: 22px; font-weight: bold;'>
                                            Te damos la bienvenida a este nuevo proyecto
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style='padding: 20px; color: #333333; font-size: 16px;'>
                                            <p>Hola <b>{user.FirstName}</b>,</p>
                                            <p>Te informamos que has sido agregado como miembro al proyecto:</p>
                                            <p style='font-size: 18px; font-weight: bold; color: #007bff;'>
                                                {project.ProjectName}
                                            </p>
                                            <p>Ahora podrás colaborar y participar en las tareas de este proyecto junto con el resto del equipo.</p>
                                            <p style='margin-top: 25px;'>¡Bienvenido!</p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style='background-color: #f8f9fa; padding: 15px; text-align: center; font-size: 12px; color: #666666;'>
                                            Equipo de Gestión de Proyectos PAWSCRUM<br>
                                            © {DateTime.Now.Year} - Todos los derechos reservados
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    ";


                    using var smtp = new SmtpClient("smtp.gmail.com")
                    {
                        Port = 587,
                        Credentials = new NetworkCredential("bricenoc506@gmail.com", "lxte nsud zhch yrmc"),
                        EnableSsl = true
                    };

                    using var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true 
                    };

                    smtp.Send(message);
                }
                catch (Exception ex)
                {
                    
                    Console.WriteLine("Error enviando correo: " + ex.Message);
                }
            }

            return Ok("Miembro agregado correctamente.");
        }

        [HttpPost]
        public JsonResult DeleteMember(int projectId, int userId)
        {
            try
            {
                if (projectId <= 0 || userId <= 0)
                    return Json(new { success = false, message = "Datos incompletos o incorrectos." });

                var member = _context.ProjectMembers
                    .FirstOrDefault(pm => pm.ProjectId == projectId && pm.UserId == userId);

                if (member == null)
                    return Json(new { success = false, message = "Miembro no encontrado en el proyecto." });

                _context.ProjectMembers.Remove(member);
                _context.SaveChanges();

                return Json(new { success = true, message = "Miembro eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error en los datos recibidos: " + ex.Message });
            }
        }

        public async Task<IActionResult> List(string filter)
        {
            
            var projects = await _context.Projects
                .Where(p => !p.IsArchived)   
                .ToListAsync();

            if (!string.IsNullOrEmpty(filter))
            {
                var search = filter.Trim().ToLower();

                projects = projects
                    .Where(p =>
                        (int.TryParse(search, out int id) && p.ProjectId == id) ||
                        p.ProjectName.ToLower().Contains(search) ||
                        p.ProjectKey.ToLower().Contains(search)
                    )
                    .ToList();
            }

            return View("Index", projects);
        }









        public class MemberDto
        {
            public int ProjectId { get; set; }
            public int UserId { get; set; }
        }




    }
}













    
  