using GestaoPedidos.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestaoPedidos.Api.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<ItemPedido> ItensPedido { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pedido>(builder =>
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.ClienteNome).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Status).HasConversion<int>();

            builder.HasMany(p => p.Itens)
                   .WithOne()
                   .HasForeignKey(i => i.PedidoId)
                   .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ItemPedido>(builder =>
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.ProdutoNome).IsRequired().HasMaxLength(150);
        });
    }
}
