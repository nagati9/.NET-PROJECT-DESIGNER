using ANA_ProjectDesigner.Data;
using ANA_ProjectDesigner.Models.Domain;
using ANA_ProjectDesigner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.CodeAnalysis;

namespace ANA_ProjectDesigner.Controllers
{
    public class SprintController : Controller
    {
        private readonly MyDBContext sprintDBContext;

        public SprintController(MyDBContext sprintDBContext)
        {
            this.sprintDBContext = sprintDBContext;
        }




       /* [HttpGet]
        public IActionResult SprintTab()
        {
            return View();
        }*/
        [HttpGet]
         public async Task<IActionResult> SprintTab()
         {
             var sprint = await sprintDBContext.Sprint.ToListAsync();
             return View(sprint);
         }

       

        [HttpPost]
        public async Task<IActionResult> Add(AddSprintViewModel addSprintRequest)
        {
            var sprintExists = sprintDBContext.Sprint
                .Any(s => s.DateStart >= addSprintRequest.DateStart && s.DateEnd <= addSprintRequest.DateEnd);

            if (sprintExists)
            {
                return RedirectToAction("ProjectDetail", "Project", new { projectId = addSprintRequest.ProjectId });
            }


            var sprint = new Sprint()
            {
                SprintId = Guid.NewGuid(),
                SprintName = addSprintRequest.SprintName,
                ProjectId = addSprintRequest.ProjectId,
                DateStart = addSprintRequest.DateStart,
                DateEnd = addSprintRequest.DateEnd,
                Comments = addSprintRequest.Comments
            };

            await sprintDBContext.Sprint.AddAsync(sprint);
            await sprintDBContext.SaveChangesAsync();

            var routeValues = new RouteValueDictionary
                {
                    { "ProjectId", sprint.ProjectId },
                    { "SprintId", sprint.SprintId },
                    // Ajoutez d'autres arguments au besoin
                };
            return RedirectToAction("ProjectDetail", "Project", routeValues);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSprint(Guid sprintId, Guid projectID)
        {

            var sprintToDelete = sprintDBContext.Sprint.Find(sprintId);

            //TO DO Remove Task when implemented


            if (sprintToDelete != null)
            {
                sprintDBContext.Sprint.Remove(sprintToDelete);
                sprintDBContext.SaveChanges();
            }
            return RedirectToAction("ProjectDetail", "Project", new { projectId = projectID });
        }


        [HttpGet]
        public IActionResult EditSprint(Guid sprintId)
        {
            var sprint = sprintDBContext.Sprint.FirstOrDefault(s => s.SprintId == sprintId);

            if (sprint == null)
            {
                return NotFound(); // Sprint non trouvé
            }

            ViewBag.SprintToEdit = sprint; // Transmet les détails du sprint à la vue pour édition
            return View();
        }


       

        [HttpPost]
        public IActionResult EditSprint(Sprint editedSprint)
        {
                var existingSprint = sprintDBContext.Sprint.FirstOrDefault(s => s.SprintId == editedSprint.SprintId);

                if (existingSprint != null)
                {
                    // Mettre à jour les propriétés du sprint existant
                    existingSprint.SprintName = editedSprint.SprintName;
                    existingSprint.DateStart = editedSprint.DateStart;
                    existingSprint.DateEnd = editedSprint.DateEnd;
                    existingSprint.Comments = editedSprint.Comments;

                    sprintDBContext.SaveChanges();

                    // Redirection vers la vue "ProjectDetail" du contrôleur "Project"
                    return RedirectToAction("ProjectDetail", "Project", new { projectId = existingSprint.ProjectId });
                }
     

            // En cas d'erreurs, retourner à la vue d'édition avec les données saisies
            return View(editedSprint);
        }



    }
}
