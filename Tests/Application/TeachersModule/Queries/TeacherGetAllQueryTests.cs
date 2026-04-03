using Application.Modules.TeachersModule.Queries.TeacherGetAllQuery;
using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MockQueryable.NSubstitute;
using NSubstitute;
using Xunit;
using Application.Modules.TeachersModule;

namespace Tests.Application.TeachersModule.Queries
{
    public class TeacherGetAllQueryTests
    {
        private readonly ITeacherRepository _teacherRepositoryMock;
        private readonly IMapper _mapper;
        private readonly TeacherGetAllRequestHandler _handler;

        public TeacherGetAllQueryTests()
        {
            _teacherRepositoryMock = Substitute.For<ITeacherRepository>();

            var services = new ServiceCollection();
            services.AddLogging();
            services.AddAutoMapper(cfg =>
            {
                cfg.CreateMap<Teacher, TeacherResponseDto>();
            });
            var serviceProvider = services.BuildServiceProvider();
            _mapper = serviceProvider.GetRequiredService<IMapper>();

            _handler = new TeacherGetAllRequestHandler(_teacherRepositoryMock, _mapper);
        }

        [Fact]
        public async Task Handle_ReturnsMappedList()
        {
            var existingTeacher = new Teacher { Id = 1, FullName = "Admin" };
            var sourceList = new List<Teacher> { existingTeacher };
            var mockQueryable = sourceList.AsQueryable().BuildMock();

            _teacherRepositoryMock.GetAll().Returns(mockQueryable);

            var result = await _handler.Handle(new TeacherGetAllRequest(), CancellationToken.None);

            result.Should().HaveCount(1);
        }
    }
}
