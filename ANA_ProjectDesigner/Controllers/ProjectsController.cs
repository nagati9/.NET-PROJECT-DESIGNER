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

        private Guid profilId;

        public ProjectsController(MyDBContext projectDBContext)
        {
            this.projectDBContext = projectDBContext;
        }

        
        

        [HttpGet]
        public IActionResult ProjectTab(Guid profilUserId)
        {
            TempData["YourGUID"] = profilUserId;
           return View();

           // return RedirectToAction("Welcome");
        }
        [HttpGet]
        public async Task<IActionResult> ListProjects(Guid profilUserId)
        {
          
            
            TempData["YourGUID"] = profilUserId;
            var projects = await projectDBContext.Projects.Where(p => p.ProfileId == profilUserId).ToListAsync();
            return View(projects);

        }

        [HttpPost]
        public async Task<IActionResult> Add(AddProjectViewModel addProfilRequest)
        {
            // Retrieve the Guid from TempData
            if (TempData["YourGUID"] is Guid profilUserId)
            {
                var project = new Projects()
                {
                    Id = Guid.NewGuid(),
                    Name = addProfilRequest.Name,
                    Description = addProfilRequest.Description,
                    ProfileId = profilUserId
                };

                await projectDBContext.Projects.AddAsync(project);
                await projectDBContext.SaveChangesAsync();
            }

            return RedirectToAction("Welcome","Profils");
        }
    }



}

