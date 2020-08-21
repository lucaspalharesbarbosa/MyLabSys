using Microsoft.EntityFrameworkCore;

namespace MyLabSys.Models {
    public class MyLabSysContext : DbContext {
        public MyLabSysContext(DbContextOptions<MyLabSysContext> options) : base(options) { }

        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Exame> Exames { get; set; }
        public DbSet<OrdemServico> OrdensServicos { get; set; }
        public DbSet<ExameOrdemServico> ExamesOrdensServicos { get; set; }
        public DbSet<PostoColeta> PostosColetas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<OrdemServico>()
                .HasOne(ordemServico => ordemServico.Paciente)
                .WithMany(paciente => paciente.OrdensServicos)
                .HasForeignKey(ordemServico => ordemServico.IdPaciente)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<OrdemServico>()
                .HasOne(ordemServico => ordemServico.PostoColeta)
                .WithMany(postoColeta => postoColeta.OrdensServicos)
                .HasForeignKey(ordemServico => ordemServico.IdPostoColeta)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<OrdemServico>()
                .HasOne(ordemServico => ordemServico.Medico)
                .WithMany(medico => medico.OrdensServicos)
                .HasForeignKey(ordemServico => ordemServico.IdMedico)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ExameOrdemServico>()
                .HasOne(exameOrdem => exameOrdem.Exame)
                .WithMany(exame => exame.OrdensServicos)
                .HasForeignKey(exameOrdem => exameOrdem.IdExame)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ExameOrdemServico>()
                .HasOne(exameOrdem => exameOrdem.OrdemServico)
                .WithMany(ordemServico => ordemServico.Exames)
                .HasForeignKey(exameOrdem => exameOrdem.IdOrdemServico)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}