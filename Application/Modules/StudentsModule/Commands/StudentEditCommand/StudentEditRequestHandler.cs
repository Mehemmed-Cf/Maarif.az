using Application.Modules.StudentsModule.Queries.StudentGetByIdQuery;
using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.StudentsModule.Commands.StudentEditCommand
{
    public class StudentEditRequestHandler : IRequestHandler<StudentEditRequest, StudentEditResponseDto>
    {
        private readonly IStudentRepository studentRepository;
        private readonly IDepartmentRepository departmentRepository;
        private readonly IMapper mapper;

        public StudentEditRequestHandler(
            IStudentRepository studentRepository,
            IDepartmentRepository departmentRepository,
            IMapper mapper)
        {
            this.studentRepository = studentRepository;
            this.departmentRepository = departmentRepository;
            this.mapper = mapper;
        }

        public async Task<StudentEditResponseDto> Handle(
            StudentEditRequest request,
            CancellationToken cancellationToken)
        {
            var entity = await studentRepository.GetAsync(s => s.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException($"Tələbə tapılmadı (Id: {request.Id})");

            // If department is changing, verify the new department exists first.
            if (entity.DepartmentId != request.DepartmentId)
                _ = await departmentRepository.GetAsync(d => d.Id == request.DepartmentId, cancellationToken);


            mapper.Map(request, entity);

            //entity.FullName = request.FullName;
            //entity.FatherName = request.FatherName;
            //entity.StudentNumber = request.StudentNumber;
            //entity.Gender = request.Gender;
            //entity.MobileNumber = request.MobileNumber;
            //entity.BirthDate = request.BirthDate;
            //entity.EducationType = request.EducationType;
            //entity.Status = request.Status;
            //entity.Year = request.Year;
            //entity.Grade = request.Grade;
            //entity.DepartmentId = request.DepartmentId;
            // LastModifiedAt / LastModifiedBy set automatically by SaveChangesAsync.

            await studentRepository.EditAsync(entity);
            await studentRepository.SaveAsync(cancellationToken);

            return mapper.Map<StudentEditResponseDto>(entity);
        }
    }
}