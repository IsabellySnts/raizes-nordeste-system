using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaizesDoNordeste.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Senha",
                value: "$2a$11$J1qBLzJNMEb8rtaY1QTB.OL5EU99QJOzMfGrMPMrsfAxMTiCoBBPO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Senha",
                value: "$2a$11$N4ZqD8f9oLQfPcmFgqj50OFvFqCjtKNBVb/YpXQZFp9qsRKFtawQi");
        }
    }
}
