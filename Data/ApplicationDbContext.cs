using GestaoGaragem.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoGaragem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<Venda> Vendas { get; set; }
        public DbSet<Garantia> Garantias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Venda>()
                .HasOne(v => v.Veiculo)
                .WithMany()
                .HasForeignKey(v => v.VeiculoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Venda>()
                .HasOne(v => v.Usuario)
                .WithMany()
                .HasForeignKey(v => v.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Garantia>()
                .HasOne(g => g.Venda)
                .WithMany()
                .HasForeignKey(g => g.VendaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
