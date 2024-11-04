using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanhiadoCacau.Migrations
{
    /// <inheritdoc />
    public partial class RetirandoEnderecoEntrega : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Enderecos_EnderecoId",
                table: "Pedidos");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_EnderecoId",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "EnderecoId",
                table: "Pedidos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EnderecoId",
                table: "Pedidos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_EnderecoId",
                table: "Pedidos",
                column: "EnderecoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Enderecos_EnderecoId",
                table: "Pedidos",
                column: "EnderecoId",
                principalTable: "Enderecos",
                principalColumn: "IdEndereco");
        }
    }
}
