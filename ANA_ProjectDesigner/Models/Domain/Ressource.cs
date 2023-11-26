namespace ANA_ProjectDesigner.Models.Domain
{
    public class Ressource
    {

        public Guid Id { get; set; }
        public string Name { get; set; }

        public int Capacity { get; set; }

        public Guid SprintId { get; set; }

        public virtual List<WorkItemRessource> WorkItemRessources { get; set; }


    }
}
