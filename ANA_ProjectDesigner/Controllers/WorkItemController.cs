using ANA_ProjectDesigner.Data;
using ANA_ProjectDesigner.Models.Domain;
using ANA_ProjectDesigner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ANA_ProjectDesigner.Controllers
{
    public class WorkItemController : Controller
    {
        private readonly MyDBContext workItemDBContext;

        public WorkItemController(MyDBContext workItemDBContext)
        {
            this.workItemDBContext = workItemDBContext;
        }


        [HttpPost]
        public async Task<IActionResult> UpdateTaskTime(Dictionary<Guid, Dictionary<Guid, int>> timeValues, Guid sprintId, Guid projectId)
        {
            foreach (var taskId in timeValues.Keys)
            {
                foreach (var resourceId in timeValues[taskId].Keys)
                {
                    var value = timeValues[taskId][resourceId];

                    // Vérifiez si une entrée avec ces clés existe déjà
                    var existingEntry = await workItemDBContext.WorkItemRessource
                        .FirstOrDefaultAsync(wir => wir.WorkItemId == taskId && wir.RessourceId == resourceId && wir.SprintId == sprintId);

                    // Vérifiez la capacité avant d'ajouter une nouvelle entrée
                    var resourceCapacity = await workItemDBContext.Ressource
                        .Where(r => r.Id == resourceId)
                        .Select(r => r.Capacity)
                        .FirstOrDefaultAsync();

                    var totalCapacityUsed = await workItemDBContext.WorkItemRessource
                        .Where(wir => wir.RessourceId == resourceId && wir.SprintId == sprintId)
                        .SumAsync(wir => wir.OriginalEstimate);

                    // Vérifiez si l'ajout ou la mise à jour dépasse la capacité totale restante
                    if (totalCapacityUsed + value - (existingEntry?.OriginalEstimate ?? 0) > resourceCapacity)
                    {
                        // Capacité dépassée, ne procédez pas à l'ajout ou à la mise à jour
                        continue;
                    }

                    if (existingEntry != null)
                    {
                        // Si elle existe, mettez à jour la valeur
                        existingEntry.OriginalEstimate = value;
                        workItemDBContext.Update(existingEntry);
                    }
                    else
                    {
                        // Ajoutez une nouvelle entrée
                        var workItemRessource = new WorkItemRessource()
                        {
                            SprintId = sprintId,
                            WorkItemId = taskId,
                            RessourceId = resourceId,
                            OriginalEstimate = value,
                        };
                        await workItemDBContext.WorkItemRessource.AddAsync(workItemRessource);
                    }
                }
            }

            await workItemDBContext.SaveChangesAsync();

            return RedirectToAction("ProjectDetail", "Project", new { projectId, selectedSprintId = sprintId });
        }


        [HttpPost]
        public async Task<IActionResult> Add(AddWorkItemViewModel addWorkItemRequest)
        {

            var workItem = new WorkItem()
            {
                Id = Guid.NewGuid(),
                TaskName = addWorkItemRequest.TaskName,
                TaskType = addWorkItemRequest.TaskType,
                SprintId = addWorkItemRequest.SprintId,
            };



            await workItemDBContext.WorkItem.AddAsync(workItem);
            await workItemDBContext.SaveChangesAsync();

            return RedirectToAction("ProjectDetail", "Project");
        }

        [HttpPost]
        public async Task<IActionResult> Remove(GetRessourceViewModel getRessourceRequest)
        {


            /*var ressource = await ressourceDBContext.Ressource.FindAsync(getRessourceRequest.Id);

            if (ressource != null)
            {
                ressourceDBContext.Ressource.Remove(ressource);
                await ressourceDBContext.SaveChangesAsync();

                return RedirectToAction("ProjcetDetail", "Project");
            }*/
            return RedirectToAction("ProjectDetail", "Project");
        }

    }
}
