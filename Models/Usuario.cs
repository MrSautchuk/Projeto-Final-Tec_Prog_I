using System.ComponentModel.DataAnnotations;

namespace GestaoGaragem.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome de usuário é obrigatório.")]
        public string NomeUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        public string Senha { get; set; } = string.Empty;

        [Required]
        public string Perfil { get; set; } = string.Empty; // 'Admin', 'Gerente', 'Vendedor'
        public string? FotoBase64 { get; set; }
    }
}
