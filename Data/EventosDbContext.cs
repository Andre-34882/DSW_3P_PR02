using DSW_3P_PR02.Models;
using Microsoft.EntityFrameworkCore;

namespace DSW_3P_PR02.Data;

public sealed class EventosDbContext(DbContextOptions<EventosDbContext> options)
    : DbContext(options)
{
    public DbSet<EventoTecnologico> Eventos => Set<EventoTecnologico>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EventoTecnologico>(entity =>
        {
            entity.ToTable("EventosTecnologicos");
            entity.HasKey(evento => evento.Id);
            entity.Property(evento => evento.Nombre).HasMaxLength(150).IsRequired();
            entity.Property(evento => evento.Categoria).HasMaxLength(100).IsRequired();
            entity.Property(evento => evento.Fecha).HasColumnType("datetimeoffset").IsRequired();
            entity.Property(evento => evento.Lugar).HasMaxLength(200).IsRequired();
            entity.Property(evento => evento.CupoMaximo).IsRequired();
            entity.Property(evento => evento.Activo).IsRequired();
        });
    }
}
