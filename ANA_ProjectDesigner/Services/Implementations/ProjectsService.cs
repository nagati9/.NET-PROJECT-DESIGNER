using ANA_ProjectDesigner.Data;
using ANA_ProjectDesigner.Models.Domain;
using ANA_ProjectDesigner.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ANA_ProjectDesigner.Services.Implementations
{
    public class ProjectsService : IProjectsService
    {
        private readonly MyDBContext _context;

        public ProjectsService(MyDBContext context)
        {
            _context = context;
        }

        public async Task<Project> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var project = await _context.Project.FirstOrDefaultAsync(project => project.Id == id, cancellationToken);

            return project;
        }
    }
}
