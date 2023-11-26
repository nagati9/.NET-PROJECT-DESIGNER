namespace ANA_ProjectDesigner.Models
{
    public class GetRessourceViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Guid sprintId { get; set; }

        public Guid projectId { get; set; }
    }
}
