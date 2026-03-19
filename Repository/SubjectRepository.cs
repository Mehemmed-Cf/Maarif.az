using Domain.Models.Entities;
using Infrastructure.Concrates;
using Application.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class SubjectRepository : AsyncRepository<Subject>, ISubjectRepository
    {
        public SubjectRepository(DbContext db) : base(db)
        {
        }
    }
}