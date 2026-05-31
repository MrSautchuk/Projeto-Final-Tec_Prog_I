using System.ComponentModel.DataAnnotations;

namespace GestaoGaragem.Models
{
    public class Veiculo
    {
        [Key]
        public int Id { get; set; }

        public string Marca { get; set; } = string.Empty;

        public string Modelo { get; set; } = string.Empty;

        public int AnoFabricacao { get; set; }

        public int AnoModelo { get; set; }

        public string Placa { get; set; } = string.Empty;

        public decimal Valor { get; set; }

        public string Status { get; set; } = "Disponivel";
        public string? FotoBase64 { get; set; }
    }
}
