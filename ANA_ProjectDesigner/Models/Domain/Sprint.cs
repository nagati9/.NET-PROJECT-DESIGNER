using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANA_ProjectDesigner.Models.Domain
{
    public class Sprint
    {
        [Key] public Guid SprintId { get; set; }
        public string SprintName { get; set; }


        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string Comments { get; set; }

        public Guid ProjectId { get; set; }

        public virtual Project Projects { get; set; }

        public virtual List<Ressource> Ressources { get; set; }

        public virtual List<WorkItem> WorkItems { get; set; }

    }
}
