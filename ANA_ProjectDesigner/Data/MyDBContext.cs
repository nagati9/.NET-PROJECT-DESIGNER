using ANA_ProjectDesigner.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ANA_ProjectDesigner.Data
{
    public class MyDBContext : DbContext
    {
        public MyDBContext(DbContextOptions options) : base(options)
        {
        }

        public  DbSet<Profil>  Profil { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<Sprint> Sprint { get; set; }
    }

}
