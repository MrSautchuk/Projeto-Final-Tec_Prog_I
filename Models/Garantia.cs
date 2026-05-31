using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoGaragem.Models
{
    public class Garantia
    {
        [Key]
        public int Id { get; set; }

        public int VendaId { get; set; }

        [ForeignKey("VendaId")]
        public Venda? Venda { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime DataFim { get; set; }

        public string Status { get; set; } = "Ativa";
    }
}
