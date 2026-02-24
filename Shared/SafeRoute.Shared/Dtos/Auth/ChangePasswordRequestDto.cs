using System.ComponentModel.DataAnnotations;

namespace SafeRoute.Shared.Dtos.Auth
{
    public class ChangePasswordRequestDto
    {
        [Required(ErrorMessage = "Senha atual é obrigatória.")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nova senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "Nova senha deve ter no mínimo 6 caracteres.")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirmação é obrigatória.")]
        [Compare(nameof(NewPassword), ErrorMessage = "As senhas não conferem.")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}