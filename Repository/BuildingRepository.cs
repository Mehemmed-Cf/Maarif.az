using Application.Repositories;
using DataAccessLayer.Migrations;
using Domain.Models.Entities;
using Infrastructure.Concrates;

namespace Repository
{
    public class BuildingRepository : AsyncRepository<Building>, IBuildingRepository
    {
        public BuildingRepository(DataContext context) : base(context) { }
    }
}
