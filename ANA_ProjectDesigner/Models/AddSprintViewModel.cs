using System.ComponentModel.DataAnnotations;

namespace ANA_ProjectDesigner.Models
{
    public class AddSprintViewModel
    {
       
        public string SprintName { get; set; }
        public Guid ProjectId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string Comments { get; set; }
    }
}
