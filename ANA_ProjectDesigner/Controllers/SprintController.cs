using ANA_ProjectDesigner.Data;
using ANA_ProjectDesigner.Models.Domain;
using ANA_ProjectDesigner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

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
                return RedirectToAction("ProjectDetail", "Project");
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

    }
}
