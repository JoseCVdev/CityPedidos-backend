using CityPedidos.Domain.Entities;
using CityPedidos.Domain.Entities.Base;
using CityPedidos.Domain.Entities.Core;
using CityPedidos.Domain.Entities.Seguridad;
using Microsoft.EntityFrameworkCore;

namespace CityPedidos.Infrastructure.Data;

public class PedidosDbContext : DbContext
{
    public PedidosDbContext(DbContextOptions<PedidosDbContext> options)
        : base(options) { }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<Menu> Menus => Set<Menu>();
    public DbSet<RolMenu> RolMenus => Set<RolMenu>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<Cliente> Clientes => Set<Cliente>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.Property(p => p.decTotal)
                .HasPrecision(10, 2);

            entity.HasOne(p => p.Cliente)
                .WithMany()
                .HasForeignKey(p => p.nIdCliente)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<RolMenu>()
            .HasKey(x => new { x.NIdRol, x.nIdMenu });

        modelBuilder.Entity<Usuario>()
            .HasOne(u => u.Rol)
            .WithMany()
            .HasForeignKey(u => u.nIdRol)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.dtFechaCrea = now;
                    break;

                case EntityState.Modified:
                    entry.Entity.dtFechaMod = now;
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.dtFechaEli = now;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
