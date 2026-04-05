using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Modules.TeachersModule.Commands.TeacherChangePasswordCommand
{
    public class TeacherChangePasswordRequest : IRequest<Unit>
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Cari şifrə tələb olunur.")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = "";

        [Required(ErrorMessage = "Yeni şifrə tələb olunur.")]
        [MinLength(3, ErrorMessage = "Şifrə ən azı 3 simvol olmalıdır.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = "";

        [Required(ErrorMessage = "Təkrar şifrə tələb olunur.")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Yeni şifrə ilə təkrar uyğun gəlmir.")]
        public string ConfirmNewPassword { get; set; } = "";
    }
}
