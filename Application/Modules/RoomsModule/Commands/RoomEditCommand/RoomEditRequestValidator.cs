using FluentValidation;

namespace Application.Modules.RoomsModule.Commands.RoomEditCommand
{
    public class RoomEditRequestValidator : AbstractValidator<RoomEditRequest>
    {
        public RoomEditRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Room ID is required.");

            RuleFor(x => x.Number)
                .GreaterThan(0).WithMessage("Number must be greater than 0.");
                
            RuleFor(x => x.BuildingId)
                .NotEmpty().WithMessage("Building is required.");
        }
    }
}