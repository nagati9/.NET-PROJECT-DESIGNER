using ANA_ProjectDesigner.Data;
using ANA_ProjectDesigner.Models.Domain;
using ANA_ProjectDesigner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ANA_ProjectDesigner.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly MyDBContext projectDBContext;

        public ProjectsController(MyDBContext projectDBContext)
        {
            this.projectDBContext = projectDBContext;
        }

        
        

        [HttpGet]
        public IActionResult ProjectTab()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ListProjects()
        {
            var projects = await projectDBContext.Projects.ToListAsync();
            return View(projects);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddProjectViewModel addProfilRequest)
        {
            var project = new Projects()
            {
                Id = Guid.NewGuid(),
                Name = addProfilRequest.Name,
               Description = addProfilRequest.Description
               

            };

            await projectDBContext.Projects.AddAsync(project);
            await projectDBContext.SaveChangesAsync();
            return RedirectToAction("ListUserProfils");
        }




    }
}
