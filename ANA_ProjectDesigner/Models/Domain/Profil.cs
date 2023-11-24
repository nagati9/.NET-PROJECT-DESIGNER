namespace ANA_ProjectDesigner.Models.Domain
{
    public class Profil
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }

        public string Country { get; set; }

        public virtual List<Project> projectList { get; set; }
    }
}
