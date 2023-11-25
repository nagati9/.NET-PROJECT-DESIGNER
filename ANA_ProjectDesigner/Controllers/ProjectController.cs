using ANA_ProjectDesigner.Data;
using ANA_ProjectDesigner.Models.Domain;
using ANA_ProjectDesigner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ANA_ProjectDesigner.Services.Interfaces;
using Microsoft.CodeAnalysis;

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
        public IActionResult ProjectDetail(Guid projectID, Guid sprintID /*, [FromRoute] Guid id*/)
        {

            ViewBag.projectId = projectID;


            // GET Projects
            string profilId = HttpContext.Session.GetString("idUser");
            var listProject = projectDBContext.Project
                    .Where(p => p.Id != projectID && p.ProfileId == new Guid(profilId))
                    .ToList();
            ViewBag.listProject = listProject;

            //GET Sprints
            var projectWithSprints = projectDBContext.Project
            .Include(p => p.Sprints)
            .FirstOrDefault(p => p.Id == projectID);

            if (projectWithSprints != null)
            {
                var sprintsForProject = projectWithSprints.Sprints
                    .OrderBy(s => s.DateStart)
                    .ThenBy(s => s.DateEnd)
                    .ToList();

                ViewBag.listSprints = sprintsForProject;
            }

            return View(projectWithSprints);
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
                var project = new Models.Domain.Project()
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

