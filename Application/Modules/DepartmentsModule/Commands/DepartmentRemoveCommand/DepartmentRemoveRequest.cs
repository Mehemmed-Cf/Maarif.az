using MediatR;

namespace Application.Modules.DepartmentsModule.Commands.DepartmentsRemoveCommand
{
    public class DepartmentRemoveRequest : IRequest
    {
        public int Id { get; set; }
    }
}
