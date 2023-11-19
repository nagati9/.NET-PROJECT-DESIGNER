namespace ANA_ProjectDesigner.Models.Domain
{
    public class Projects
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ProfileId { get; set; }

        public List<Sprints> Sprints { get; set; }
    }
}
