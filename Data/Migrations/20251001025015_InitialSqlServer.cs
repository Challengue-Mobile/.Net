using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MottothTracking.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialSqlServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_BEACON",
                columns: table => new
                {
                    ID_BEACON = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UUID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MODELO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    STATUS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BATERIA = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_BEACON", x => x.ID_BEACON);
                });

            migrationBuilder.CreateTable(
                name: "TB_PATIO",
                columns: table => new
                {
                    ID_PATIO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ENDERECO = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PATIO", x => x.ID_PATIO);
                });

            migrationBuilder.CreateTable(
                name: "TB_USUARIO",
                columns: table => new
                {
                    ID_USUARIO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EMAIL = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    SENHA_HASH = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DATA_CADASTRO = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_USUARIO", x => x.ID_USUARIO);
                });

            migrationBuilder.CreateTable(
                name: "TB_MOTO",
                columns: table => new
                {
                    ID_MOTO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PLACA = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MODELO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    STATUS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DATA_REGISTRO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ID_BEACON = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_MOTO", x => x.ID_MOTO);
                    table.ForeignKey(
                        name: "FK_TB_MOTO_TB_BEACON_ID_BEACON",
                        column: x => x.ID_BEACON,
                        principalTable: "TB_BEACON",
                        principalColumn: "ID_BEACON");
                });

            migrationBuilder.CreateTable(
                name: "TB_REGISTRO_BATERIA",
                columns: table => new
                {
                    ID_REG = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_BEACON = table.Column<int>(type: "int", nullable: false),
                    NIVEL = table.Column<int>(type: "int", nullable: false),
                    DATA_HORA = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_REGISTRO_BATERIA", x => x.ID_REG);
                    table.ForeignKey(
                        name: "FK_TB_REGISTRO_BATERIA_TB_BEACON_ID_BEACON",
                        column: x => x.ID_BEACON,
                        principalTable: "TB_BEACON",
                        principalColumn: "ID_BEACON",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_ZONA",
                columns: table => new
                {
                    ID_ZONA = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DESCRICAO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ID_PATIO = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ZONA", x => x.ID_ZONA);
                    table.ForeignKey(
                        name: "FK_TB_ZONA_TB_PATIO_ID_PATIO",
                        column: x => x.ID_PATIO,
                        principalTable: "TB_PATIO",
                        principalColumn: "ID_PATIO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_LOG_SISTEMA",
                columns: table => new
                {
                    ID_LOG = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NIVEL = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MENSAGEM = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DATA_HORA = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ID_USUARIO = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_LOG_SISTEMA", x => x.ID_LOG);
                    table.ForeignKey(
                        name: "FK_TB_LOG_SISTEMA_TB_USUARIO_ID_USUARIO",
                        column: x => x.ID_USUARIO,
                        principalTable: "TB_USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "TB_LOCALIZACAO",
                columns: table => new
                {
                    ID_LOCALIZACAO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LATITUDE = table.Column<double>(type: "float", nullable: false),
                    LONGITUDE = table.Column<double>(type: "float", nullable: false),
                    DATA_HORA = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ID_MOTO = table.Column<int>(type: "int", nullable: false),
                    ID_ZONA = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_LOCALIZACAO", x => x.ID_LOCALIZACAO);
                    table.ForeignKey(
                        name: "FK_TB_LOCALIZACAO_TB_MOTO_ID_MOTO",
                        column: x => x.ID_MOTO,
                        principalTable: "TB_MOTO",
                        principalColumn: "ID_MOTO",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_LOCALIZACAO_TB_ZONA_ID_ZONA",
                        column: x => x.ID_ZONA,
                        principalTable: "TB_ZONA",
                        principalColumn: "ID_ZONA",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TB_MOVIMENTACAO",
                columns: table => new
                {
                    ID_MOV = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_MOTO = table.Column<int>(type: "int", nullable: false),
                    ID_ZONA_ORIGEM = table.Column<int>(type: "int", nullable: true),
                    ID_ZONA_DESTINO = table.Column<int>(type: "int", nullable: true),
                    DATA_MOVIMENTACAO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ORIGEM = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DESTINO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TIPO = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_MOVIMENTACAO", x => x.ID_MOV);
                    table.ForeignKey(
                        name: "FK_TB_MOVIMENTACAO_TB_MOTO_ID_MOTO",
                        column: x => x.ID_MOTO,
                        principalTable: "TB_MOTO",
                        principalColumn: "ID_MOTO",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_MOVIMENTACAO_TB_ZONA_ID_ZONA_DESTINO",
                        column: x => x.ID_ZONA_DESTINO,
                        principalTable: "TB_ZONA",
                        principalColumn: "ID_ZONA",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TB_MOVIMENTACAO_TB_ZONA_ID_ZONA_ORIGEM",
                        column: x => x.ID_ZONA_ORIGEM,
                        principalTable: "TB_ZONA",
                        principalColumn: "ID_ZONA",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "TB_BEACON",
                columns: new[] { "ID_BEACON", "BATERIA", "MODELO", "STATUS", "UUID" },
                values: new object[,]
                {
                    { 1, 90, "FIAP-DEV", "ATIVO", "33333333-3333-3333-3333-333333333333" },
                    { 2, 75, "FIAP-TEST", "MANUTENCAO", "44444444-4444-4444-4444-444444444444" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_BEACON_UUID",
                table: "TB_BEACON",
                column: "UUID");

            migrationBuilder.CreateIndex(
                name: "IX_TB_LOCALIZACAO_ID_MOTO",
                table: "TB_LOCALIZACAO",
                column: "ID_MOTO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_LOCALIZACAO_ID_ZONA",
                table: "TB_LOCALIZACAO",
                column: "ID_ZONA");

            migrationBuilder.CreateIndex(
                name: "IX_TB_LOG_SISTEMA_ID_USUARIO",
                table: "TB_LOG_SISTEMA",
                column: "ID_USUARIO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_MOTO_ID_BEACON",
                table: "TB_MOTO",
                column: "ID_BEACON");

            migrationBuilder.CreateIndex(
                name: "IX_TB_MOVIMENTACAO_ID_MOTO",
                table: "TB_MOVIMENTACAO",
                column: "ID_MOTO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_MOVIMENTACAO_ID_ZONA_DESTINO",
                table: "TB_MOVIMENTACAO",
                column: "ID_ZONA_DESTINO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_MOVIMENTACAO_ID_ZONA_ORIGEM",
                table: "TB_MOVIMENTACAO",
                column: "ID_ZONA_ORIGEM");

            migrationBuilder.CreateIndex(
                name: "IX_TB_REGISTRO_BATERIA_ID_BEACON",
                table: "TB_REGISTRO_BATERIA",
                column: "ID_BEACON");

            migrationBuilder.CreateIndex(
                name: "IX_TB_ZONA_ID_PATIO",
                table: "TB_ZONA",
                column: "ID_PATIO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_LOCALIZACAO");

            migrationBuilder.DropTable(
                name: "TB_LOG_SISTEMA");

            migrationBuilder.DropTable(
                name: "TB_MOVIMENTACAO");

            migrationBuilder.DropTable(
                name: "TB_REGISTRO_BATERIA");

            migrationBuilder.DropTable(
                name: "TB_USUARIO");

            migrationBuilder.DropTable(
                name: "TB_MOTO");

            migrationBuilder.DropTable(
                name: "TB_ZONA");

            migrationBuilder.DropTable(
                name: "TB_BEACON");

            migrationBuilder.DropTable(
                name: "TB_PATIO");
        }
    }
}
