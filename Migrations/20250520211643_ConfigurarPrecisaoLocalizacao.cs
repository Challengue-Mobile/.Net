using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_.Net.Migrations
{
    /// <inheritdoc />
    public partial class ConfigurarPrecisaoLocalizacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "POSICAO_Y",
                table: "TB_LOCALIZACAO",
                type: "DECIMAL(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "POSICAO_X",
                table: "TB_LOCALIZACAO",
                type: "DECIMAL(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(18,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "POSICAO_Y",
                table: "TB_LOCALIZACAO",
                type: "DECIMAL(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "POSICAO_X",
                table: "TB_LOCALIZACAO",
                type: "DECIMAL(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(18,6)",
                oldPrecision: 18,
                oldScale: 6);
        }
    }
}
