namespace ANA_ProjectDesigner.Models
{
    public class AddRessourceViewModel
    {
        public string Name { get; set; }
        public int capacity { get; set; }
        public Guid sprintId { get; set; }

        public Guid projectId { get; set; }
    }
}
