using Domain.Models.Entities;
using Infrastructure.Concrates;
using Application.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class FacultyRepository : AsyncRepository<Faculty>, IFacultyRepository
    {
        public FacultyRepository(DbContext db) : base(db)
        {
        }
    }
}