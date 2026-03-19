using Domain.Models.Entities;
using Infrastructure.Concrates;
using Application.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class TeacherRepository : AsyncRepository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(DbContext db) : base(db)
        {
        }
    }
}