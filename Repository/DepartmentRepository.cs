using Domain.Models.Entities;
using Infrastructure.Concrates;
using Application.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class DepartmentRepository : AsyncRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(DbContext db) : base(db)
        {
            
        }
    }
}
