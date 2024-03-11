﻿// <auto-generated />
using System;
using ArduinoWeb.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ArduinoWeb.Migrations
{
    [DbContext(typeof(ArduinoDbContext))]
    [Migration("20240310184933_firtsMigration")]
    partial class firtsMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ArduinoWeb.Models.Dispositivo", b =>
                {
                    b.Property<int>("DispositivoId")
                        .HasColumnType("int");

                    b.Property<string>("Descricao")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DispositivoId");

                    b.ToTable("Dispositivos");
                });

            modelBuilder.Entity("ArduinoWeb.Models.Localizacao", b =>
                {
                    b.Property<int>("LocalizacaoId")
                        .HasColumnType("int");

                    b.Property<string>("Descricao")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LocalizacaoId");

                    b.ToTable("Localizacoes");
                });

            modelBuilder.Entity("ArduinoWeb.Models.Medicao", b =>
                {
                    b.Property<int>("MedicaoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MedicaoId"), 1L, 1);

                    b.Property<DateTime>("DataMedicao")
                        .HasColumnType("datetime2");

                    b.Property<int>("LocalizacaoId")
                        .HasColumnType("int");

                    b.Property<int>("RelatorioDispositivoId")
                        .HasColumnType("int");

                    b.Property<int>("TipoMedicaoId")
                        .HasColumnType("int");

                    b.Property<bool>("ValorLido")
                        .HasColumnType("bit");

                    b.HasKey("MedicaoId");

                    b.HasIndex("LocalizacaoId");

                    b.HasIndex("RelatorioDispositivoId");

                    b.HasIndex("TipoMedicaoId");

                    b.ToTable("Medicoes");
                });

            modelBuilder.Entity("ArduinoWeb.Models.RelatorioDispositivo", b =>
                {
                    b.Property<int>("RelatorioDispositivoId")
                        .HasColumnType("int");

                    b.Property<string>("Descricao")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DispositivoId")
                        .HasColumnType("int");

                    b.Property<int>("LocalizacaoId")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UltimoIpAddress")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RelatorioDispositivoId");

                    b.HasIndex("DispositivoId");

                    b.HasIndex("LocalizacaoId");

                    b.ToTable("RelatorioDispositivos");
                });

            modelBuilder.Entity("ArduinoWeb.Models.TipoMedicao", b =>
                {
                    b.Property<int>("TipoMedicaoId")
                        .HasColumnType("int");

                    b.Property<string>("Descricao")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TipoMedicaoId");

                    b.ToTable("TipoMedicoes");
                });

            modelBuilder.Entity("ArduinoWeb.Models.Medicao", b =>
                {
                    b.HasOne("ArduinoWeb.Models.Localizacao", "Localizacao")
                        .WithMany("Medicoes")
                        .HasForeignKey("LocalizacaoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ArduinoWeb.Models.RelatorioDispositivo", "RelatorioDispositivo")
                        .WithMany("Medicoes")
                        .HasForeignKey("RelatorioDispositivoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ArduinoWeb.Models.TipoMedicao", "TipoMedicao")
                        .WithMany("Medicoes")
                        .HasForeignKey("TipoMedicaoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Localizacao");

                    b.Navigation("RelatorioDispositivo");

                    b.Navigation("TipoMedicao");
                });

            modelBuilder.Entity("ArduinoWeb.Models.RelatorioDispositivo", b =>
                {
                    b.HasOne("ArduinoWeb.Models.Dispositivo", "Dispositivo")
                        .WithMany("RelatorioDispositivos")
                        .HasForeignKey("DispositivoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ArduinoWeb.Models.Localizacao", "Localizacao")
                        .WithMany("RelatorioDispositivo")
                        .HasForeignKey("LocalizacaoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dispositivo");

                    b.Navigation("Localizacao");
                });

            modelBuilder.Entity("ArduinoWeb.Models.Dispositivo", b =>
                {
                    b.Navigation("RelatorioDispositivos");
                });

            modelBuilder.Entity("ArduinoWeb.Models.Localizacao", b =>
                {
                    b.Navigation("Medicoes");

                    b.Navigation("RelatorioDispositivo");
                });

            modelBuilder.Entity("ArduinoWeb.Models.RelatorioDispositivo", b =>
                {
                    b.Navigation("Medicoes");
                });

            modelBuilder.Entity("ArduinoWeb.Models.TipoMedicao", b =>
                {
                    b.Navigation("Medicoes");
                });
#pragma warning restore 612, 618
        }
    }
}