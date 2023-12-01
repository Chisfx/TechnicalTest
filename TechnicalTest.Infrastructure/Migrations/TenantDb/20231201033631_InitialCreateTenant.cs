using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechnicalTest.Infrastructure.Migrations.TenantDb
{
    public partial class InitialCreateTenant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Tenat");

            migrationBuilder.CreateTable(
                name: "Organizations",
                schema: "Tenat",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Identifier = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConnectionString = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_Identifier",
                schema: "Tenat",
                table: "Organizations",
                column: "Identifier",
                unique: true,
                filter: "[Identifier] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Organizations",
                schema: "Tenat");
        }
    }
}
