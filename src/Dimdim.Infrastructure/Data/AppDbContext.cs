using Dimdim.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dimdim.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<ItemPedido> ItensPedido => Set<ItemPedido>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.ToTable("Pedidos");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.ClienteNome).HasMaxLength(120).IsRequired();
            entity.Property(x => x.DataCriacao).IsRequired();
            entity.Property(x => x.ValorTotal).HasPrecision(18, 2);
            entity.HasMany(x => x.Itens)
                .WithOne()
                .HasForeignKey(x => x.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ItemPedido>(entity =>
        {
            entity.ToTable("ItensPedido");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Descricao).HasMaxLength(160).IsRequired();
            entity.Property(x => x.Quantidade).IsRequired();
            entity.Property(x => x.PrecoUnitario).HasPrecision(18, 2);
            entity.HasIndex(x => x.PedidoId);
        });
    }
}
