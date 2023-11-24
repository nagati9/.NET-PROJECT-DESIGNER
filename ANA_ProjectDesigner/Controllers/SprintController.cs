using ANA_ProjectDesigner.Data;
using ANA_ProjectDesigner.Models.Domain;
using ANA_ProjectDesigner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            return RedirectToAction("ListUserProfils");
        }

    }
}
