using ANA_ProjectDesigner.Data;
using ANA_ProjectDesigner.Models.Domain;
using ANA_ProjectDesigner.Models;
using Microsoft.AspNetCore.Mvc;

namespace ANA_ProjectDesigner.Controllers
{
    public class SprintsController : Controller
    {
        private readonly MyDBContext sprintDBContext;

        public SprintsController(MyDBContext sprintDBContext)
        {
            this.sprintDBContext = sprintDBContext;
        }




        [HttpGet]
        public IActionResult SprintTab()
        {
            return View();
        }
        [HttpGet]
        /* public async Task<IActionResult> ListSprints()
         {
             var sprints = await DBContext.Sprints.ToListAsync();
             return View(sprints);
         }*/

        [HttpPost]
        public async Task<IActionResult> Add(AddSprintViewModel addSprintRequest)
        {
            var sprint = new Sprints()
            {
                SprintId = Guid.NewGuid(),
                SprintName = addSprintRequest.SprintName,
                 ProjectId = addSprintRequest.ProjectId,
                DateStart = addSprintRequest.DateStart,
                DateEnd = addSprintRequest.DateEnd,
                Comments = addSprintRequest.Comments


            };

            await sprintDBContext.Sprints.AddAsync(sprint);
            await sprintDBContext.SaveChangesAsync();
            return RedirectToAction("ListUserProfils");
        }

    }
}
