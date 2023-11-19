namespace ANA_ProjectDesigner.Models.Domain
{
    public class Sprints
    {
        public Guid SprintId { get; set; }
        public string SprintName { get; set; }
        

        public DateOnly DateStart { get; set; }
        public DateOnly DateEnd { get; set; }
        public string Comments { get; set; }

        //ref
       // public Guid ProjectId { get; set; }

    }
}
