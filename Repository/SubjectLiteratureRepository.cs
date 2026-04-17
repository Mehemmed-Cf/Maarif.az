using Application.Repositories;
using DataAccessLayer.Migrations;
using Domain.Models.Entities;
using Infrastructure.Concrates;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class SubjectLiteratureRepository : AsyncRepository<SubjectLiterature>, ISubjectLiteratureRepository
    {
        private readonly DataContext context;

        public SubjectLiteratureRepository(DataContext context) : base(context)
        {
            this.context = context;
        }
    }
}
