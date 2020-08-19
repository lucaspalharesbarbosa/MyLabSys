using Microsoft.EntityFrameworkCore;

namespace MyLabSys.Models {
    public class MyLabSysContext : DbContext {
        public MyLabSysContext(DbContextOptions<MyLabSysContext> options) : base(options) { }

        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Exame> Exames { get; set; }
        public DbSet<PostoColeta> PostosColetas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            
        }
    }
}