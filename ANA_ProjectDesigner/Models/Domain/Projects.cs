using System.ComponentModel.DataAnnotations.Schema;

namespace ANA_ProjectDesigner.Models.Domain
{
    public class Projects
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        //Linking to profils 1..*
        public Guid ProfileId { get; set; }
        [ForeignKey("ProfileId")]
        public virtual Profils Profils { get; set; }
        
        //linking to sprints 1..*
        public virtual List<Sprints> Sprints { get; set; }
    }
}
