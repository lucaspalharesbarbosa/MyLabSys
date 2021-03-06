﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyLabSys.Models;

namespace MyLabSys.Migrations
{
    [DbContext(typeof(MyLabSysContext))]
    partial class MyLabSysContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MyLabSys.Models.Convenio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nome")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<decimal>("PercentualDesconto")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("Id");

                    b.ToTable("Convenios");
                });

            modelBuilder.Entity("MyLabSys.Models.Exame", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Descricao")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<decimal>("Preco")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("Id");

                    b.ToTable("Exames");
                });

            modelBuilder.Entity("MyLabSys.Models.ExameOrdemServico", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IdExame")
                        .HasColumnType("int");

                    b.Property<int>("IdOrdemServico")
                        .HasColumnType("int");

                    b.Property<decimal>("Preco")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("Id");

                    b.HasIndex("IdExame");

                    b.HasIndex("IdOrdemServico");

                    b.ToTable("ExamesOrdensServicos");
                });

            modelBuilder.Entity("MyLabSys.Models.Medico", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Especialidade")
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

                    b.Property<string>("Nome")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Medicos");
                });

            modelBuilder.Entity("MyLabSys.Models.OrdemServico", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CodigoPedidoMedico")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("CodigoProtocolo")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<DateTime>("DataEmissao")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataPrevisaoEntrega")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdMedico")
                        .HasColumnType("int");

                    b.Property<int>("IdPaciente")
                        .HasColumnType("int");

                    b.Property<int>("IdPostoColeta")
                        .HasColumnType("int");

                    b.Property<string>("SenhaPaciente")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte>("Status")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.HasIndex("IdMedico");

                    b.HasIndex("IdPaciente");

                    b.HasIndex("IdPostoColeta");

                    b.ToTable("OrdensServicos");
                });

            modelBuilder.Entity("MyLabSys.Models.Paciente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DataNascimento")
                        .HasColumnType("datetime2");

                    b.Property<string>("Endereco")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<int?>("IdConvenio")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<byte>("Sexo")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.HasIndex("IdConvenio");

                    b.ToTable("Pacientes");
                });

            modelBuilder.Entity("MyLabSys.Models.PostoColeta", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Descricao")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Endereco")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("PostosColetas");
                });

            modelBuilder.Entity("MyLabSys.Models.ResultadoExameOrdemServico", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DescricaoResultado")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IdExameOrdemServico")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdExameOrdemServico")
                        .IsUnique();

                    b.ToTable("ResultadosExamesOrdensServicos");
                });

            modelBuilder.Entity("MyLabSys.Models.ExameOrdemServico", b =>
                {
                    b.HasOne("MyLabSys.Models.Exame", "Exame")
                        .WithMany("OrdensServicos")
                        .HasForeignKey("IdExame")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MyLabSys.Models.OrdemServico", "OrdemServico")
                        .WithMany("Exames")
                        .HasForeignKey("IdOrdemServico")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MyLabSys.Models.OrdemServico", b =>
                {
                    b.HasOne("MyLabSys.Models.Medico", "Medico")
                        .WithMany("OrdensServicos")
                        .HasForeignKey("IdMedico")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MyLabSys.Models.Paciente", "Paciente")
                        .WithMany("OrdensServicos")
                        .HasForeignKey("IdPaciente")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MyLabSys.Models.PostoColeta", "PostoColeta")
                        .WithMany("OrdensServicos")
                        .HasForeignKey("IdPostoColeta")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("MyLabSys.Models.Paciente", b =>
                {
                    b.HasOne("MyLabSys.Models.Convenio", "Convenio")
                        .WithMany("Pacientes")
                        .HasForeignKey("IdConvenio")
                        .OnDelete(DeleteBehavior.NoAction);
                });

            modelBuilder.Entity("MyLabSys.Models.ResultadoExameOrdemServico", b =>
                {
                    b.HasOne("MyLabSys.Models.ExameOrdemServico", "ExameOrdemServico")
                        .WithOne("ResultadoExame")
                        .HasForeignKey("MyLabSys.Models.ResultadoExameOrdemServico", "IdExameOrdemServico")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
