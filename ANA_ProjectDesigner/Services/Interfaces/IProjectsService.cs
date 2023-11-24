using ANA_ProjectDesigner.Models.Domain;

namespace ANA_ProjectDesigner.Services.Interfaces
{
    public interface IProjectsService
    {
        Task<Project> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
