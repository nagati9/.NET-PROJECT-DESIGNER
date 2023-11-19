namespace ANA_ProjectDesigner.Models
{
    public class AddSprintViewModel
    {
       
        public string SprintName { get; set; }
        public Guid ProjectId { get; set; }

        public DateOnly DateStart { get; set; }
        public DateOnly DateEnd { get; set; }
        public string Comments { get; set; }
    }
}
