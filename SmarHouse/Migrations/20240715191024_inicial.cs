using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHouse.Migrations
{
    public partial class inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Medicoes",
                columns: table => new
                {
                    MedicaoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Porta = table.Column<int>(type: "int", nullable: false),
                    Movimento = table.Column<int>(type: "int", nullable: false),
                    DataMedicao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicoes", x => x.MedicaoId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Medicoes");
        }
    }
}
