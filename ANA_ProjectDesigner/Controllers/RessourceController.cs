using ANA_ProjectDesigner.Data;
using ANA_ProjectDesigner.Models.Domain;
using ANA_ProjectDesigner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis;

namespace ANA_ProjectDesigner.Controllers
{
    public class RessourceController : Controller
    {

        private readonly MyDBContext ressourceDBContext;

        public RessourceController(MyDBContext ressourceDBContext)
        {
            this.ressourceDBContext = ressourceDBContext;
        }


        [HttpPost]
        public async Task<IActionResult> Add(AddRessourceViewModel addRessourceRequest)
        {
            var ressource = new Ressource()
            {
                Name = addRessourceRequest.Name,
                Capacity = addRessourceRequest.capacity,
                SprintId = addRessourceRequest.sprintId,
            };

            await ressourceDBContext.Ressource.AddAsync(ressource);
            await ressourceDBContext.SaveChangesAsync();


            return RedirectToAction("ProjectDetail", "Project", new { addRessourceRequest.projectId, selectedSprintId = addRessourceRequest.sprintId });
        }

        [HttpPost]
        public async Task<IActionResult> Remove(GetRessourceViewModel getRessourceRequest)
        {
           

            var ressource = await ressourceDBContext.Ressource.FindAsync(getRessourceRequest.Id);

            if (ressource != null)
            {
                ressourceDBContext.Ressource.Remove(ressource);
                await ressourceDBContext.SaveChangesAsync();

                if (getRessourceRequest.sprintId != Guid.Empty)
                {
                        return RedirectToAction("ProjectDetail", "Project", new { getRessourceRequest.projectId, selectedSprintId = getRessourceRequest.sprintId });

                } else
                {
                    return RedirectToAction("OverView", "Profil");
                }

            }
            return RedirectToAction("ProjectDetail", "Project", new { getRessourceRequest.projectId, selectedSprintId = getRessourceRequest.sprintId });
        }

        [HttpGet]
        public IActionResult ListResources(GetRessourceViewModel model)
        {
            var resources = ressourceDBContext.Ressource
                .Where(r => r.SprintId == model.sprintId )
                .ToList();

            return View(resources);
        }

    }
}
