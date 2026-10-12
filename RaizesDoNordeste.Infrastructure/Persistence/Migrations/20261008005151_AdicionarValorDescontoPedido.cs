using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaizesDoNordeste.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarValorDescontoPedido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ValorDesconto",
                table: "Pedidos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValorDesconto",
                table: "Pedidos");
        }
    }
}
