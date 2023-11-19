using ANA_ProjectDesigner.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ANA_ProjectDesigner.Data
{
    public class MyDBContext : DbContext
    {
        public MyDBContext(DbContextOptions options) : base(options)
        {
        }

        public  DbSet<Profils>  Profils { get; set; }
        public DbSet<Projects> Projects { get; set; }
        public DbSet<Sprints> Sprints { get; set; }
    }

}
