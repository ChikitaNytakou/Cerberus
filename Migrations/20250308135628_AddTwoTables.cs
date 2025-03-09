using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ByeBye.Migrations
{
    /// <inheritdoc />
    public partial class AddTwoTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Polygons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Kod = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Polygons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SrVesGrPoezdaCoFour",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PolygonId = table.Column<int>(type: "integer", nullable: false),
                    Start = table.Column<DateTime>(type: "date", nullable: false),
                    Plan = table.Column<double>(type: "double precision", nullable: false),
                    FactTkm = table.Column<double>(type: "double precision", nullable: false),
                    FactPkm = table.Column<double>(type: "double precision", nullable: false),
                    Fact = table.Column<double>(type: "double precision", nullable: false),
                    Result = table.Column<double>(type: "double precision", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SrVesGrPoezdaCoFour", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SrVesGrPoezdaCoFour_Polygons_PolygonId",
                        column: x => x.PolygonId,
                        principalTable: "Polygons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SrVesGrPoezdaCoFour_PolygonId",
                table: "SrVesGrPoezdaCoFour",
                column: "PolygonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SrVesGrPoezdaCoFour");

            migrationBuilder.DropTable(
                name: "Polygons");
        }
    }
}
