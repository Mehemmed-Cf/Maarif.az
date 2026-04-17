using Domain.Models.Entities;
using Infrastructure.Abstracts;

namespace Application.Repositories
{
    public interface IAssignmentRepository : IAsyncRepository<Assignment>
    {
    }
}