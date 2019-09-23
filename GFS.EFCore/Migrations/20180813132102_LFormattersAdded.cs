using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace GFS.Data.EFCore.Migrations
{
    public partial class LFormattersAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HypotenuseLength",
                table: "PentagonFormatters");

            migrationBuilder.RenameColumn(
                name: "Width",
                table: "PentagonFormatters",
                newName: "Width2");

            migrationBuilder.RenameColumn(
                name: "TopLength",
                table: "PentagonFormatters",
                newName: "Width1");

            migrationBuilder.RenameColumn(
                name: "RightLength",
                table: "PentagonFormatters",
                newName: "Depth2");

            migrationBuilder.RenameColumn(
                name: "Length",
                table: "PentagonFormatters",
                newName: "Depth1");

            migrationBuilder.CreateTable(
                name: "LFormatters",
                columns: table => new
                {
                    Count = table.Column<int>(nullable: false),
                    CutterDepth = table.Column<float>(nullable: true),
                    CutterLength = table.Column<float>(nullable: true),
                    CutterWidth = table.Column<float>(nullable: true),
                    FabricId = table.Column<int>(nullable: false),
                    FurnitureId = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsMilling = table.Column<bool>(nullable: false),
                    LeftSpace = table.Column<float>(nullable: true),
                    Milling = table.Column<int>(nullable: true),
                    Thickness = table.Column<float>(nullable: false),
                    TopSpace = table.Column<float>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Width1 = table.Column<float>(nullable: false),
                    Width2 = table.Column<float>(nullable: false),
                    Depth1 = table.Column<float>(nullable: false),
                    Depth2 = table.Column<float>(nullable: false),
                    Indentation1 = table.Column<float>(nullable: false),
                    Indentation2 = table.Column<float>(nullable: false),
                    IsWidth1Border = table.Column<bool>(nullable: false),
                    IsWidth2Border = table.Column<bool>(nullable: false),
                    IsDepth1Border = table.Column<bool>(nullable: false),
                    IsDepth2Border = table.Column<bool>(nullable: false),
                    IsIndentation1Border = table.Column<bool>(nullable: false),
                    IsIndentation2Border = table.Column<bool>(nullable: false),
                    Width1BorderThickness = table.Column<float>(nullable: true),
                    Width2BorderThickness = table.Column<float>(nullable: true),
                    Depth1BorderThickness = table.Column<float>(nullable: true),
                    Depth2BorderThickness = table.Column<float>(nullable: true),
                    Indentation1BorderThickness = table.Column<float>(nullable: true),
                    Indentation2BorderThickness = table.Column<float>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LFormatters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LFormatters_Fabrics_FabricId",
                        column: x => x.FabricId,
                        principalTable: "Fabrics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LFormatters_Furnitures_FurnitureId",
                        column: x => x.FurnitureId,
                        principalTable: "Furnitures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LFormatters_FabricId",
                table: "LFormatters",
                column: "FabricId");

            migrationBuilder.CreateIndex(
                name: "IX_LFormatters_FurnitureId",
                table: "LFormatters",
                column: "FurnitureId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LFormatters");

            migrationBuilder.RenameColumn(
                name: "Width2",
                table: "PentagonFormatters",
                newName: "Width");

            migrationBuilder.RenameColumn(
                name: "Width1",
                table: "PentagonFormatters",
                newName: "TopLength");

            migrationBuilder.RenameColumn(
                name: "Depth2",
                table: "PentagonFormatters",
                newName: "RightLength");

            migrationBuilder.RenameColumn(
                name: "Depth1",
                table: "PentagonFormatters",
                newName: "Length");

            migrationBuilder.AddColumn<float>(
                name: "HypotenuseLength",
                table: "PentagonFormatters",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
