using Domain.Models.Entities;
using Infrastructure.Concrates;
using Application.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class StudentRepository : AsyncRepository<Student>, IStudentRepository
    {
        public StudentRepository(DbContext db) : base(db)
        {
        }
    }
}