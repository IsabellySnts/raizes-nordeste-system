using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaizesDoNordeste.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CorrigirSenhaAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Senha",
                value: "$2a$11$aWrp/ZZRphVGWbFrKyQdaORQcQJckoioQ224hS0UN/qfzljlzZyDG");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Senha",
                value: "$2a$11$J1qBLzJNMEb8rtaY1QTB.OL5EU99QJOzMfGrMPMrsfAxMTiCoBBPO");
        }
    }
}
