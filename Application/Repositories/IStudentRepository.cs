using Domain.Models.Entities;
using Infrastructure.Abstracts;

namespace Application.Repositories
{
    public interface IStudentRepository : IAsyncRepository<Student>
    {
    }
}