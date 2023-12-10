using ANA_ProjectDesigner.Data;
using ANA_ProjectDesigner.Models.Domain;
using ANA_ProjectDesigner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis;

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

            return RedirectToAction("ProjectDetail", "Project", new { projectId= addWorkItemRequest.projectId, selectedSprintId = addWorkItemRequest.SprintId });
        }

        [HttpPost]
        public async Task<IActionResult> Remove(AddWorkItemViewModel WorkItemModel)
        {


            var workItem = await workItemDBContext.WorkItem.FindAsync(WorkItemModel.Id);

            if (workItem != null)
            {
                workItemDBContext.WorkItem.Remove(workItem);
                await workItemDBContext.SaveChangesAsync();

                return RedirectToAction("ProjectDetail", "Project", new { WorkItemModel.projectId, selectedSprintId = WorkItemModel.SprintId });
            }
            return RedirectToAction("ProjectDetail", "Project", new { WorkItemModel.projectId, selectedSprintId = WorkItemModel.SprintId });
        }

        [HttpPost]
        public IActionResult Edit(AddWorkItemViewModel workItemUpdated)
        {
            var workItem = workItemDBContext.WorkItem.FirstOrDefault(s => s.Id == workItemUpdated.Id);

            if (workItem != null)
            {
                // Mettre à jour les propriétés du sprint existant
                workItem.TaskName = workItemUpdated.TaskName;
                workItem.TaskType = workItemUpdated.TaskType;

                workItemDBContext.SaveChanges();

                // Redirection vers la vue "ProjectDetail" du contrôleur "Project"
                return RedirectToAction("ProjectDetail", "Project", new { workItemUpdated.projectId, selectedSprintId = workItemUpdated.SprintId });
            }


            // En cas d'erreurs, retourner à la vue d'édition avec les données saisies
            return RedirectToAction("ProjectDetail", "Project", new { workItemUpdated.projectId });
        }



    }
}
