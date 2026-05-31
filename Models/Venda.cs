using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoGaragem.Models
{
    public class Venda
    {
        [Key]
        public int Id { get; set; }

        public int VeiculoId { get; set; }

        [ForeignKey("VeiculoId")]
        public Veiculo? Veiculo { get; set; }

        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario? Usuario { get; set; }

        public DateTime DataVenda { get; set; }

        public decimal ValorFinal { get; set; }

        public string NomeCliente { get; set; } = string.Empty;

        public string CpfCliente { get; set; } = string.Empty;
    }
}
