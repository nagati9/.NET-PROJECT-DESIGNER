using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANA_ProjectDesigner.Models.Domain
{
    public class Sprints
    {
[Key] public Guid SprintId { get; set; }
        public string SprintName { get; set; }
        

        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string Comments { get; set; }

        //ref
        public Guid ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public virtual Projects Projects { get; set; }

    }
}
