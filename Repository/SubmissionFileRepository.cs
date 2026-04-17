using Application.Repositories;
using Domain.Models.Entities;
using Infrastructure.Concrates;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class SubmissionFileRepository : AsyncRepository<SubmissionFile>, ISubmissionFileRepository
    {
        public SubmissionFileRepository(DbContext context) : base(context)
        {
        }
    }
}