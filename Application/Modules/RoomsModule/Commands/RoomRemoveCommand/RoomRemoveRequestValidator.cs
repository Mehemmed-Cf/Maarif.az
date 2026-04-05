using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modules.RoomsModule.Commands.RoomRemoveCommand
{
    public class RoomRemoveRequestValidator : AbstractValidator<RoomRemoveRequest>
    {
        public RoomRemoveRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Room ID is required.");
        }
    }
}
