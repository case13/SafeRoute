using SafeRoute.Shared.Enums.Status;
using SafeRoute.Shared.Enums.Tipos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SafeRoute.Shared.Dtos.User
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "O Id da Empresa é obrigatório.")]
        public int CompanyId { get; set; } = 0;
        [Required(ErrorMessage = "O Nome é obrigatório.")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "O Documento é obrigatório.")]
        public string Document { get; set; } = string.Empty;
        [Required(ErrorMessage = "O Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O Email informado não é válido.")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "A Senha é obrigatória.")]
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "O Tipo de Usuário é obrigatório.")]
        public UserTypeEnum UserType { get; set; } = UserTypeEnum.UsuarioComum;
        [Required(ErrorMessage = "O status do usuário é obrigatório")]
        public StatusBasicEnum StatusUser { get; set; } = StatusBasicEnum.Ativo;
    }
}
