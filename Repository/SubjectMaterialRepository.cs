using Application.Repositories;
using DataAccessLayer.Migrations;
using Domain.Models.Entities;
using Infrastructure.Concrates;

namespace Repository
{
    public class SubjectMaterialRepository : AsyncRepository<SubjectMaterial>, ISubjectMaterialRepository
    {
        private readonly DataContext context;

        public SubjectMaterialRepository(DataContext context) : base(context)
        {
            this.context = context;
        }
    }
}
