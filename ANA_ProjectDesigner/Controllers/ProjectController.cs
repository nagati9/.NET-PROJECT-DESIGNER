using ANA_ProjectDesigner.Data;
using ANA_ProjectDesigner.Models.Domain;
using ANA_ProjectDesigner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ANA_ProjectDesigner.Services.Interfaces;

namespace ANA_ProjectDesigner.Controllers
{
    public class ProjectController : Controller
    {
        private readonly MyDBContext projectDBContext;
        private readonly IProjectsService _projectsService;


        public ProjectController(MyDBContext projectDBContext, IProjectsService projectsService)
        {
       
            this.projectDBContext = projectDBContext;
            _projectsService = projectsService;
        }




        [HttpGet]
        public IActionResult ProjectTab(Guid profilUserId)
        {
           return View();
           // return RedirectToAction("Welcome");
        }

        //[HttpGet("{id}")] // Permet de récupérer l'ID depuis la route à tester
        [HttpGet]
        public IActionResult ProjectDetail(Guid projectID /*, [FromRoute] Guid id*/)
        {

            ViewBag.projectId = projectID;
            //projectDBContext.Projects.Include(project => project.Sprints).
            DateTime today = DateTime.Now.Date;

            // Récupérer le sprint en cours avec la date d'aujourd'hui entre startDate et endDate
            /*Sprints currentSprint = projectDBContext.Sprints
                .Include(s => s.Project) // Inclure les informations du projet lié au sprint si nécessaire
                .FirstOrDefault(s => s.DateStart <= today && s.DateEnd >= today);

            if (currentSprint == null)
            {
                return NotFound("Aucun sprint en cours pour aujourd'hui.");
            }*/
            return View();
            // return RedirectToAction("Welcome");
        }
        [HttpGet]
        public async Task<IActionResult> ListProjects()
        {
          
            string storedGuid = HttpContext.Session.GetString("idUser");
            if (Guid.TryParse(storedGuid, out Guid profilUserId))
            {
                var projects = await projectDBContext.Project.Where(p => p.ProfileId == profilUserId).ToListAsync();
                return View(projects);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddProjectViewModel addProfilRequest)
        {
            // Retrieve the Guid from TempData
            string storedGuid = HttpContext.Session.GetString("idUser");
            if (Guid.TryParse(storedGuid, out Guid profilUserId))
            {
                var project = new Project()
                {
                    Id = Guid.NewGuid(),
                    Name = addProfilRequest.Name,
                    Description = addProfilRequest.Description,
                    ProfileId = profilUserId
                };

                await projectDBContext.Project.AddAsync(project);
                await projectDBContext.SaveChangesAsync();
            }

            return RedirectToAction("Welcome","Profil");
        }

        [HttpPost]
        public async Task<IActionResult> GoToProject()
        {
            // Retrieve the Guid from TempData
            /*string storedGuid = HttpContext.Session.GetString("idUser");
            if (Guid.TryParse(storedGuid, out Guid profilUserId))
            {

            }*/

            return RedirectToAction("SprintTab", "Sprint");
        }
    }



}

