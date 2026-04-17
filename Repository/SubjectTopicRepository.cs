using Application.Repositories;
using DataAccessLayer.Migrations;
using Domain.Models.Entities;
using Infrastructure.Concrates;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class SubjectTopicRepository : AsyncRepository<SubjectTopic>, ISubjectTopicRepository
    {
        private readonly DataContext context;

        public SubjectTopicRepository(DataContext context) : base(context)
        {
            this.context = context;
        }
    }
}
