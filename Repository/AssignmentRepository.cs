using Application.Repositories;
using Domain.Models.Entities;
using Infrastructure.Concrates;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class AssignmentRepository : AsyncRepository<Assignment>, IAssignmentRepository
    {
        public AssignmentRepository(DbContext context) : base(context)
        {
        }
    }
}