namespace ANA_ProjectDesigner.Models.Domain
{
    public class WorkItem
    {
        public Guid Id { get; set; }
        public Guid SprintId { get; set; }

        public string TaskName { get; set; }
        public string TaskType { get; set; }

        public virtual List<WorkItemRessource> WorkItemRessources { get; set; }

    }
}
