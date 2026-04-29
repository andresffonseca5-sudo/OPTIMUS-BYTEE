using Microsoft.EntityFrameworkCore;
using OPTIMUS_BYTEE.Models;
using System.Collections.Generic;

namespace UPTIMUS.Data
{
    public class UptimusDBContext : DbContext
    {
        public UptimusDBContext(DbContextOptions<UptimusDBContext> options)
            : base(options) { }

        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Sesion> Sesiones { get; set; }
        public DbSet<LogAuditoria> LogAuditoria { get; set; }
        public DbSet<IntentoFallido> IntentosFallidos { get; set; }
    }
}