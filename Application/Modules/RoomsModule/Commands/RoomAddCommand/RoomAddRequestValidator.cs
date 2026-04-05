using FluentValidation;

namespace Application.Modules.RoomsModule.Commands.RoomAddCommand
{
    public class RoomAddRequestValidator : AbstractValidator<RoomAddRequest>
    {
        public RoomAddRequestValidator()
        {
            RuleFor(x => x.Number)
                .GreaterThan(0).WithMessage("Number must be greater than 0.");
                
            RuleFor(x => x.BuildingId)
                .NotEmpty().WithMessage("Building is required.");
        }
    }
}