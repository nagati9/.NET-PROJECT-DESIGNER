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

        public IActionResult ProjectDetail(Guid projectID, Guid selectedSprintId /*, [FromRoute] Guid id*/)
        {
            ViewBag.projectId = projectID;

            string profilId = HttpContext.Session.GetString("idUser");
            var listProject = projectDBContext.Project
                .Where(p => p.Id != projectID && p.ProfileId == new Guid(profilId))
                .ToList();
            ViewBag.listProject = listProject;

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

                DateTime today = DateTime.Now.Date;
                var currentSprint = new Sprint();

                if (selectedSprintId != Guid.Empty)
                {
                    currentSprint = projectDBContext.Sprint
                        .Include(s => s.Ressources)
                        .Where(s => s.SprintId == selectedSprintId)
                        .FirstOrDefault();
                }
                else
                {
                    currentSprint = projectDBContext.Sprint
                        .Include(s => s.Ressources)
                        .FirstOrDefault(s => s.DateStart <= today && s.DateEnd >= today && s.ProjectId == projectID);

                    if (currentSprint == null)
                    {
                        currentSprint = projectDBContext.Sprint
                            .Include(s => s.Ressources)
                            .FirstOrDefault(s => s.DateStart > today && s.ProjectId == projectID);

                        if (currentSprint == null)
                        {
                            currentSprint = projectDBContext.Sprint
                                .Include(s => s.Ressources)
                                .Where(s => s.ProjectId == projectID)
                                .OrderByDescending(s => s.DateStart)
                                .FirstOrDefault();
                        }
                    }
                }

                if (currentSprint != null)
                {
                    ViewBag.currentSprint = currentSprint;

                    var allSprints = selectedSprintId == Guid.Empty;

                    if (allSprints)
                    {
                        var allWorkItems = projectDBContext.WorkItem.ToList();
                        ViewBag.workItem = allWorkItems;
                    }
                    else
                    {
                        var workItemsForSprint = projectDBContext.WorkItem
                            .Where(p => p.SprintId == currentSprint.SprintId)
                            .ToList();

                        ViewBag.workItem = workItemsForSprint;
                    }

                    var ressources = projectDBContext.Ressource
                        .Where(p => p.SprintId == currentSprint.SprintId)
                        .ToList();

                    if (ressources.Count() > 0) { ViewBag.ressources = ressources; }

                    var WorkItemRessource = projectDBContext.WorkItemRessource
                        .Where(p => p.SprintId == currentSprint.SprintId)
                        .ToList();

                    if (WorkItemRessource.Count() > 0) { ViewBag.WorkItemRessource = WorkItemRessource; }

                    if (WorkItemRessource != null)
                    {
                        var sommeParWorkItemId = projectDBContext.WorkItemRessource
                            .GroupBy(wir => wir.WorkItemId)
                            .ToDictionary(
                                group => group.Key,
                                group => group.Sum(item => item.OriginalEstimate)
                            );

                        ViewBag.sumTime = sommeParWorkItemId;
                    }
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

            return RedirectToAction("Welcome", "Profil");
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


 
        [HttpGet]
        public IActionResult Backlog(Guid projectId)
        {
            // Fetch the project including sprints
            var project = projectDBContext.Project
                .Include(p => p.Sprints)
                .FirstOrDefault(p => p.Id == projectId);

            return View(project);
        }

    }


    }

