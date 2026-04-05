using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Modules.StudentsModule.Commands.ForgotStudentPasswordCommand
{
    public class ForgotStudentPasswordRequest : IRequest<Unit>
    {
        [Required(ErrorMessage = "Sənəd seriyası tələb olunur.")]
        public string SerialNumber { get; set; } = "";

        [Required(ErrorMessage = "FIN kod tələb olunur.")]
        [StringLength(7, MinimumLength = 7, ErrorMessage = "FIN kod 7 simvol olmalıdır.")]
        public string FinCode { get; set; } = "";

        [Required(ErrorMessage = "Yeni şifrə tələb olunur.")]
        [MinLength(3, ErrorMessage = "Şifrə ən azı 3 simvol olmalıdır.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = "";

        [Required(ErrorMessage = "Təkrar şifrə tələb olunur.")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Şifrələr uyğun gəlmir.")]
        public string ConfirmNewPassword { get; set; } = "";
    }
}
