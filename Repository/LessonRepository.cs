using Microsoft.EntityFrameworkCore;
using Domain.Models.Entities;
using Infrastructure.Concrates;
using Application.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class LessonRepository : AsyncRepository<Lesson>, ILessonRepository
    {
        public LessonRepository(DbContext db) : base(db)
        {
        }
    }
}