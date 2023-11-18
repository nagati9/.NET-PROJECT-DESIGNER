using ANA_ProjectDesigner.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ANA_ProjectDesigner.Data
{
    public class ProfilDBContext : DbContext
    {
        public ProfilDBContext(DbContextOptions options) : base(options)
        {
        }

        public  DbSet<Profils>  Profils { get; set; }
    }
}
