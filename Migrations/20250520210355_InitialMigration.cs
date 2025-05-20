using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_.Net.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_CLIENTE",
                columns: table => new
                {
                    ID_CLIENTE = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    CPF = table.Column<string>(type: "NVARCHAR2(14)", maxLength: 14, nullable: false),
                    DATA_CADASTRO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    TELEFONE = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_CLIENTE", x => x.ID_CLIENTE);
                });

            migrationBuilder.CreateTable(
                name: "TB_MODELO_BEACON",
                columns: table => new
                {
                    ID_MODELO_BEACON = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    FABRICANTE = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_MODELO_BEACON", x => x.ID_MODELO_BEACON);
                });

            migrationBuilder.CreateTable(
                name: "TB_MODELO_MOTO",
                columns: table => new
                {
                    ID_MODELO_MOTO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    FABRICANTE = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_MODELO_MOTO", x => x.ID_MODELO_MOTO);
                });

            migrationBuilder.CreateTable(
                name: "TB_PAIS",
                columns: table => new
                {
                    ID_PAIS = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PAIS", x => x.ID_PAIS);
                });

            migrationBuilder.CreateTable(
                name: "TB_TIPO_MOVIMENTACAO",
                columns: table => new
                {
                    ID_TIPO_MOVIMENTACAO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    DESCRICAO = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_TIPO_MOVIMENTACAO", x => x.ID_TIPO_MOVIMENTACAO);
                });

            migrationBuilder.CreateTable(
                name: "TB_TIPO_USUARIO",
                columns: table => new
                {
                    ID_TIPO_USUARIO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    DESCRICAO = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_TIPO_USUARIO", x => x.ID_TIPO_USUARIO);
                });

            migrationBuilder.CreateTable(
                name: "TB_MOTO",
                columns: table => new
                {
                    ID_MOTO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PLACA = table.Column<string>(type: "NVARCHAR2(10)", maxLength: 10, nullable: false),
                    DATA_REGISTRO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    ID_CLIENTE = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    ID_MODELO_MOTO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_MOTO", x => x.ID_MOTO);
                    table.ForeignKey(
                        name: "FK_TB_MOTO_TB_CLIENTE_ID_CLIENTE",
                        column: x => x.ID_CLIENTE,
                        principalTable: "TB_CLIENTE",
                        principalColumn: "ID_CLIENTE");
                    table.ForeignKey(
                        name: "FK_TB_MOTO_TB_MODELO_MOTO_ID_MODELO_MOTO",
                        column: x => x.ID_MODELO_MOTO,
                        principalTable: "TB_MODELO_MOTO",
                        principalColumn: "ID_MODELO_MOTO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_ESTADO",
                columns: table => new
                {
                    ID_ESTADO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    ID_PAIS = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ESTADO", x => x.ID_ESTADO);
                    table.ForeignKey(
                        name: "FK_TB_ESTADO_TB_PAIS_ID_PAIS",
                        column: x => x.ID_PAIS,
                        principalTable: "TB_PAIS",
                        principalColumn: "ID_PAIS",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_USUARIO",
                columns: table => new
                {
                    ID_USUARIO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    SENHA = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    DATA_CADASTRO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    ID_TIPO_USUARIO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_USUARIO", x => x.ID_USUARIO);
                    table.ForeignKey(
                        name: "FK_TB_USUARIO_TB_TIPO_USUARIO_ID_TIPO_USUARIO",
                        column: x => x.ID_TIPO_USUARIO,
                        principalTable: "TB_TIPO_USUARIO",
                        principalColumn: "ID_TIPO_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_BEACON",
                columns: table => new
                {
                    ID_BEACON = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    UUID = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    BATERIA = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ID_MOTO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ID_MODELO_BEACON = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_BEACON", x => x.ID_BEACON);
                    table.ForeignKey(
                        name: "FK_TB_BEACON_TB_MODELO_BEACON_ID_MODELO_BEACON",
                        column: x => x.ID_MODELO_BEACON,
                        principalTable: "TB_MODELO_BEACON",
                        principalColumn: "ID_MODELO_BEACON",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_BEACON_TB_MOTO_ID_MOTO",
                        column: x => x.ID_MOTO,
                        principalTable: "TB_MOTO",
                        principalColumn: "ID_MOTO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_CIDADE",
                columns: table => new
                {
                    ID_CIDADE = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    ID_ESTADO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_CIDADE", x => x.ID_CIDADE);
                    table.ForeignKey(
                        name: "FK_TB_CIDADE_TB_ESTADO_ID_ESTADO",
                        column: x => x.ID_ESTADO,
                        principalTable: "TB_ESTADO",
                        principalColumn: "ID_ESTADO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_LOG_SISTEMA",
                columns: table => new
                {
                    ID_LOG = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ACAO = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    DATA_HORA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    ID_USUARIO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_LOG_SISTEMA", x => x.ID_LOG);
                    table.ForeignKey(
                        name: "FK_TB_LOG_SISTEMA_TB_USUARIO_ID_USUARIO",
                        column: x => x.ID_USUARIO,
                        principalTable: "TB_USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_MOVIMENTACAO",
                columns: table => new
                {
                    ID_MOVIMENTACAO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    DATA_MOVIMENTACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    OBSERVACAO = table.Column<string>(type: "CLOB", nullable: false),
                    ID_USUARIO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ID_MOTO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ID_TIPO_MOVIMENTACAO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_MOVIMENTACAO", x => x.ID_MOVIMENTACAO);
                    table.ForeignKey(
                        name: "FK_TB_MOVIMENTACAO_TB_MOTO_ID_MOTO",
                        column: x => x.ID_MOTO,
                        principalTable: "TB_MOTO",
                        principalColumn: "ID_MOTO",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_MOVIMENTACAO_TB_TIPO_MOVIMENTACAO_ID_TIPO_MOVIMENTACAO",
                        column: x => x.ID_TIPO_MOVIMENTACAO,
                        principalTable: "TB_TIPO_MOVIMENTACAO",
                        principalColumn: "ID_TIPO_MOVIMENTACAO",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_MOVIMENTACAO_TB_USUARIO_ID_USUARIO",
                        column: x => x.ID_USUARIO,
                        principalTable: "TB_USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_REGISTRO_BATERIA",
                columns: table => new
                {
                    ID_REGISTRO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    DATA_HORA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    NIVEL_BATERIA = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ID_BEACON = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_REGISTRO_BATERIA", x => x.ID_REGISTRO);
                    table.ForeignKey(
                        name: "FK_TB_REGISTRO_BATERIA_TB_BEACON_ID_BEACON",
                        column: x => x.ID_BEACON,
                        principalTable: "TB_BEACON",
                        principalColumn: "ID_BEACON",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_BAIRRO",
                columns: table => new
                {
                    ID_BAIRRO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    ID_CIDADE = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_BAIRRO", x => x.ID_BAIRRO);
                    table.ForeignKey(
                        name: "FK_TB_BAIRRO_TB_CIDADE_ID_CIDADE",
                        column: x => x.ID_CIDADE,
                        principalTable: "TB_CIDADE",
                        principalColumn: "ID_CIDADE",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_LOGRADOURO",
                columns: table => new
                {
                    ID_LOGRADOURO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    ID_BAIRRO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_LOGRADOURO", x => x.ID_LOGRADOURO);
                    table.ForeignKey(
                        name: "FK_TB_LOGRADOURO_TB_BAIRRO_ID_BAIRRO",
                        column: x => x.ID_BAIRRO,
                        principalTable: "TB_BAIRRO",
                        principalColumn: "ID_BAIRRO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_PATIO",
                columns: table => new
                {
                    ID_PATIO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    ID_LOGRADOURO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PATIO", x => x.ID_PATIO);
                    table.ForeignKey(
                        name: "FK_TB_PATIO_TB_LOGRADOURO_ID_LOGRADOURO",
                        column: x => x.ID_LOGRADOURO,
                        principalTable: "TB_LOGRADOURO",
                        principalColumn: "ID_LOGRADOURO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_FILIAL",
                columns: table => new
                {
                    ID_FILIAL = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    ID_PATIO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_FILIAL", x => x.ID_FILIAL);
                    table.ForeignKey(
                        name: "FK_TB_FILIAL_TB_PATIO_ID_PATIO",
                        column: x => x.ID_PATIO,
                        principalTable: "TB_PATIO",
                        principalColumn: "ID_PATIO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_LOCALIZACAO",
                columns: table => new
                {
                    ID_LOCALIZACAO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    POSICAO_X = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false),
                    POSICAO_Y = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false),
                    DATA_HORA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    ID_MOTO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ID_PATIO = table.Column<int>(type: "NUMBER(10)", nullable: true)
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
                        name: "FK_TB_LOCALIZACAO_TB_PATIO_ID_PATIO",
                        column: x => x.ID_PATIO,
                        principalTable: "TB_PATIO",
                        principalColumn: "ID_PATIO");
                });

            migrationBuilder.CreateTable(
                name: "TB_DEPARTAMENTO",
                columns: table => new
                {
                    ID_DEPARTAMENTO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    ID_FILIAL = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_DEPARTAMENTO", x => x.ID_DEPARTAMENTO);
                    table.ForeignKey(
                        name: "FK_TB_DEPARTAMENTO_TB_FILIAL_ID_FILIAL",
                        column: x => x.ID_FILIAL,
                        principalTable: "TB_FILIAL",
                        principalColumn: "ID_FILIAL",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_FUNCIONARIO",
                columns: table => new
                {
                    ID_FUNCIONARIO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    CPF = table.Column<string>(type: "NVARCHAR2(14)", maxLength: 14, nullable: false),
                    CARGO = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    DATA_ADMISSAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    ID_USUARIO = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    ID_DEPARTAMENTO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_FUNCIONARIO", x => x.ID_FUNCIONARIO);
                    table.ForeignKey(
                        name: "FK_TB_FUNCIONARIO_TB_DEPARTAMENTO_ID_DEPARTAMENTO",
                        column: x => x.ID_DEPARTAMENTO,
                        principalTable: "TB_DEPARTAMENTO",
                        principalColumn: "ID_DEPARTAMENTO",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_FUNCIONARIO_TB_USUARIO_ID_USUARIO",
                        column: x => x.ID_USUARIO,
                        principalTable: "TB_USUARIO",
                        principalColumn: "ID_USUARIO");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_BAIRRO_ID_CIDADE",
                table: "TB_BAIRRO",
                column: "ID_CIDADE");

            migrationBuilder.CreateIndex(
                name: "IX_TB_BEACON_ID_MODELO_BEACON",
                table: "TB_BEACON",
                column: "ID_MODELO_BEACON");

            migrationBuilder.CreateIndex(
                name: "IX_TB_BEACON_ID_MOTO",
                table: "TB_BEACON",
                column: "ID_MOTO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_CIDADE_ID_ESTADO",
                table: "TB_CIDADE",
                column: "ID_ESTADO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_DEPARTAMENTO_ID_FILIAL",
                table: "TB_DEPARTAMENTO",
                column: "ID_FILIAL");

            migrationBuilder.CreateIndex(
                name: "IX_TB_ESTADO_ID_PAIS",
                table: "TB_ESTADO",
                column: "ID_PAIS");

            migrationBuilder.CreateIndex(
                name: "IX_TB_FILIAL_ID_PATIO",
                table: "TB_FILIAL",
                column: "ID_PATIO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_FUNCIONARIO_ID_DEPARTAMENTO",
                table: "TB_FUNCIONARIO",
                column: "ID_DEPARTAMENTO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_FUNCIONARIO_ID_USUARIO",
                table: "TB_FUNCIONARIO",
                column: "ID_USUARIO",
                unique: true,
                filter: "\"ID_USUARIO\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TB_LOCALIZACAO_ID_MOTO",
                table: "TB_LOCALIZACAO",
                column: "ID_MOTO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_LOCALIZACAO_ID_PATIO",
                table: "TB_LOCALIZACAO",
                column: "ID_PATIO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_LOG_SISTEMA_ID_USUARIO",
                table: "TB_LOG_SISTEMA",
                column: "ID_USUARIO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_LOGRADOURO_ID_BAIRRO",
                table: "TB_LOGRADOURO",
                column: "ID_BAIRRO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_MOTO_ID_CLIENTE",
                table: "TB_MOTO",
                column: "ID_CLIENTE");

            migrationBuilder.CreateIndex(
                name: "IX_TB_MOTO_ID_MODELO_MOTO",
                table: "TB_MOTO",
                column: "ID_MODELO_MOTO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_MOVIMENTACAO_ID_MOTO",
                table: "TB_MOVIMENTACAO",
                column: "ID_MOTO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_MOVIMENTACAO_ID_TIPO_MOVIMENTACAO",
                table: "TB_MOVIMENTACAO",
                column: "ID_TIPO_MOVIMENTACAO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_MOVIMENTACAO_ID_USUARIO",
                table: "TB_MOVIMENTACAO",
                column: "ID_USUARIO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PATIO_ID_LOGRADOURO",
                table: "TB_PATIO",
                column: "ID_LOGRADOURO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_REGISTRO_BATERIA_ID_BEACON",
                table: "TB_REGISTRO_BATERIA",
                column: "ID_BEACON");

            migrationBuilder.CreateIndex(
                name: "IX_TB_USUARIO_ID_TIPO_USUARIO",
                table: "TB_USUARIO",
                column: "ID_TIPO_USUARIO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_FUNCIONARIO");

            migrationBuilder.DropTable(
                name: "TB_LOCALIZACAO");

            migrationBuilder.DropTable(
                name: "TB_LOG_SISTEMA");

            migrationBuilder.DropTable(
                name: "TB_MOVIMENTACAO");

            migrationBuilder.DropTable(
                name: "TB_REGISTRO_BATERIA");

            migrationBuilder.DropTable(
                name: "TB_DEPARTAMENTO");

            migrationBuilder.DropTable(
                name: "TB_TIPO_MOVIMENTACAO");

            migrationBuilder.DropTable(
                name: "TB_USUARIO");

            migrationBuilder.DropTable(
                name: "TB_BEACON");

            migrationBuilder.DropTable(
                name: "TB_FILIAL");

            migrationBuilder.DropTable(
                name: "TB_TIPO_USUARIO");

            migrationBuilder.DropTable(
                name: "TB_MODELO_BEACON");

            migrationBuilder.DropTable(
                name: "TB_MOTO");

            migrationBuilder.DropTable(
                name: "TB_PATIO");

            migrationBuilder.DropTable(
                name: "TB_CLIENTE");

            migrationBuilder.DropTable(
                name: "TB_MODELO_MOTO");

            migrationBuilder.DropTable(
                name: "TB_LOGRADOURO");

            migrationBuilder.DropTable(
                name: "TB_BAIRRO");

            migrationBuilder.DropTable(
                name: "TB_CIDADE");

            migrationBuilder.DropTable(
                name: "TB_ESTADO");

            migrationBuilder.DropTable(
                name: "TB_PAIS");
        }
    }
}
