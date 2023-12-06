using ANA_ProjectDesigner.Data;
using ANA_ProjectDesigner.Models.Domain;
using ANA_ProjectDesigner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ANA_ProjectDesigner.Services.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.Build.Evaluation;

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

        public class SprintWithWorkItems
        {
            public Sprint Sprint { get; set; }
            public List<WorkItem> WorkItems { get; set; }
        }


        [HttpGet]
        public IActionResult ProjectTab(Guid profilUserId)
        {
           return View();
           // return RedirectToAction("Welcome");
        }

        //[HttpGet("{id}")] // Permet de récupérer l'ID depuis la route à tester
        [HttpGet]
        public IActionResult ProjectDetail(Guid projectID, Guid selectedSprintId /*, [FromRoute] Guid id*/)
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

            

            if (projectWithSprints != null && projectWithSprints.Sprints.Count() > 0)
            {
                var sprintsForProject = projectWithSprints.Sprints
                    .OrderBy(s => s.DateStart)
                    .ThenBy(s => s.DateEnd)
                    .ToList();

                ViewBag.listSprints = sprintsForProject;

                var backlogItems = projectDBContext.Sprint
                .Where(s => s.ProjectId == projectID)
                .Include(s => s.WorkItems)
                .ToList()
                .Select(sprint => new SprintWithWorkItems
                {
                    Sprint = sprint,
                    WorkItems = sprint.WorkItems.ToList()
                })
                .ToList();

                ViewBag.backlogItems = backlogItems;


                DateTime today = DateTime.Now.Date;
                var currentSprint = new Sprint();

                if (selectedSprintId != Guid.Empty)
                {
                    currentSprint = projectDBContext.Sprint
                         .Include(s => s.Ressources)
                    .Where(s => s.SprintId == selectedSprintId )
                    .FirstOrDefault();
                } else
                {
                    currentSprint = projectDBContext.Sprint.Include(s => s.Ressources)
                    .Where(s => s.DateStart <= today && s.DateEnd >= today && s.ProjectId == projectID)
                    .OrderBy(s => s.DateStart)
                    .FirstOrDefault();
                }

                if (currentSprint == null)
                {
                    currentSprint = projectDBContext.Sprint.Include(s => s.Ressources)
                        .Where(s => s.DateStart > today && s.ProjectId == projectID)
                        .OrderBy(s => s.DateStart)
                        .FirstOrDefault();

                    if (currentSprint == null)
                    {
                        // Si aucun sprint n'est en cours et il n'y a pas de futur sprint,
                        // alors récupérer le dernier sprint (le sprint avec la date de début la plus éloignée)
                        currentSprint = projectDBContext.Sprint
                            .Include(s => s.Ressources)
                            .Where(s => s.ProjectId == projectID)
                            .OrderByDescending(s => s.DateStart)
                            .FirstOrDefault();
                    }
                    
                }
                
                if (currentSprint != null)
                {
                    ViewBag.currentSprint = currentSprint;

                    // Supposons que sprintId soit la variable dans votre code

                    // Récupérer les ParentItem avec le sprintId correspondant
                    var workItem = projectDBContext.WorkItem
                        .Where(p => p.SprintId == currentSprint.SprintId)
                        .ToList();

                    if (workItem != null) { ViewBag.workItem = workItem; }

                    var ressources = projectDBContext.Ressource
                        .Where(p => p.SprintId == currentSprint.SprintId)
                        .ToList();

                    if (ressources.Count() > 0) { ViewBag.ressources = ressources;}

                   var WorkItemRessource = projectDBContext.WorkItemRessource
                        .Where(p =>p.SprintId == currentSprint.SprintId)
                        .ToList();
                    if (WorkItemRessource.Count() > 0) { ViewBag.WorkItemRessource = WorkItemRessource; }

                    /*if (WorkItemRessource != null)
                    {
                        var totalOriginalEstimated =  projectDBContext.WorkItemRessource
                            .Where(wir => wir.SprintId == currentSprint.SprintId)
                            .Sum(wir => wir.OriginalEstimate);
                        
                    }*/

                }

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

