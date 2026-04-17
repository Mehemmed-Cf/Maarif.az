using Application.Repositories;
using Domain.Models.Entities;
using Infrastructure.Concrates;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class AssignmentFileRepository : AsyncRepository<AssignmentFile>, IAssignmentFileRepository
    {
        public AssignmentFileRepository(DbContext context) : base(context)
        {
        }
    }
}