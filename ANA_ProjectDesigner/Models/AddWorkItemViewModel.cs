namespace ANA_ProjectDesigner.Models
{
    public class AddWorkItemViewModel
    {
        public Guid Id { get; set; }

        public string TaskName { get; set; }
        public string TaskType { get; set; }

        public Guid SprintId { get; set; }
        public Guid projectId { get; set; }

    }
}
