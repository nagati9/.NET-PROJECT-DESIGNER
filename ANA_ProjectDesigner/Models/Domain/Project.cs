using System.ComponentModel.DataAnnotations.Schema;

namespace ANA_ProjectDesigner.Models.Domain
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        //Linking to profils 1..*
        public Guid ProfileId { get; set; }


        [ForeignKey("ProfileId")]
        public virtual Profil Profil { get; set; }

        public virtual List<Sprint> Sprints { get; set; }

    }
}
