using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ANA_ProjectDesigner.Models.Domain
{
    public class WorkItemRessource
    {

        [Key, Column(Order = 0)]
        public Guid WorkItemId { get; set; }
        public WorkItem WorkItem { get; set; }

        [Key, Column(Order = 1)]
        public Guid RessourceId { get; set; }
        public Ressource Ressource { get; set; }

        [Key, Column(Order = 2)]
        public Guid SprintId { get; set; }
        public Sprint Sprint { get; set; }

        public int OriginalEstimate { get; set; } = 0;
    }
}
