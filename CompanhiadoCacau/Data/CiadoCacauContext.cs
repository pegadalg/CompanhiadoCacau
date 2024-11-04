using CompanhiadoCacau.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanhiadoCacau.Data
{
    public class CiadoCacauContext : DbContext
    {
        public CiadoCacauContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<PedidoProduto> PedidoProdutos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração para PedidoProduto como chave composta
            modelBuilder.Entity<PedidoProduto>()
                .HasKey(pp => new { pp.PedidoId, pp.ProdutoId });

            // Relacionamento entre PedidoProduto e Pedido com restrição de exclusão
            modelBuilder.Entity<PedidoProduto>()
                .HasOne(pp => pp.Pedido)
                .WithMany(p => p.PedidoProdutos)
                .HasForeignKey(pp => pp.PedidoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento entre PedidoProduto e Produto com restrição de exclusão
            modelBuilder.Entity<PedidoProduto>()
                .HasOne(pp => pp.Produto)
                .WithMany(p => p.PedidoProdutos)
                .HasForeignKey(pp => pp.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento entre Pedido e Cliente com restrição de exclusão
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Cliente)
                .WithMany(c => c.PedidosCliente)
                .OnDelete(DeleteBehavior.Restrict);
     
        }
    }
}
