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

        public DbSet<WorkItem> WorkItem { get; set; }

        public DbSet<Ressource> Ressource { get; set; }

        public DbSet<WorkItemRessource> WorkItemRessource { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkItemRessource>()
        .HasKey(wir => new { wir.WorkItemId, wir.RessourceId });

    modelBuilder.Entity<WorkItemRessource>()
        .HasOne(wir => wir.WorkItem)
        .WithMany(wi => wi.WorkItemRessources)
        .HasForeignKey(wir => wir.WorkItemId);

    modelBuilder.Entity<WorkItemRessource>()
        .HasOne(wir => wir.Ressource)
        .WithMany(r => r.WorkItemRessources)
        .HasForeignKey(wir => wir.RessourceId);

    // Les configurations supplémentaires si nécessaires...

    base.OnModelCreating(modelBuilder);
        }

    }

}
