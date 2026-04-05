using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Modules.TeachersModule.Commands.RecoverTeacherNumberCommand
{
    public class RecoverTeacherNumberRequest : IRequest<RecoverTeacherNumberResponseDto>
    {
        [Required(ErrorMessage = "Sənəd seriyası tələb olunur.")]
        public string SerialNumber { get; set; } = "";

        [Required(ErrorMessage = "FIN kod tələb olunur.")]
        [StringLength(7, MinimumLength = 7, ErrorMessage = "FIN kod 7 simvol olmalıdır.")]
        public string FinCode { get; set; } = "";
    }
}
