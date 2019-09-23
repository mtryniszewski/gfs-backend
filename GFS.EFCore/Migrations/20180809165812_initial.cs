using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace GFS.Data.EFCore.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Producers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    IsArchival = table.Column<bool>(nullable: false),
                    City = table.Column<string>(nullable: true),
                    Street = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true)
                  
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Surname = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Permissions = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsArchival = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: false),
                    PasswordResetToken = table.Column<string>(nullable: true),
                    AccountActivationToken = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    NormalizedUserName = table.Column<string>(nullable: true),
                    NormalizedEmail = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
             
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fabrics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: false),
                    ProducerCode = table.Column<string>(nullable: false),
                    ProducerId = table.Column<int>(nullable: false),
                   Thickness = table.Column<float>(nullable: false),
                    IsArchival = table.Column<bool>(nullable: false),
                    ImageUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fabrics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fabrics_Producers_ProducerId",
                        column: x => x.ProducerId,
                        principalTable: "Producers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    IsConfirmed = table.Column<bool>(nullable: false),
                    IsArchival = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Furnitures",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    FurnitureType = table.Column<int>(nullable: false),
                    OrderId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Furnitures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Furnitures_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PentagonFormatters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    Count = table.Column<int>(nullable: false),
                    Width = table.Column<float>(nullable: false),
                    Length = table.Column<float>(nullable: false),
                    Thickness = table.Column<float>(nullable: false),
                    FurnitureId = table.Column<int>(nullable: false),
                    FabricId = table.Column<int>(nullable: false),
                    HypotenuseLength = table.Column<float>(nullable: false),
                    TopLength = table.Column<float>(nullable: false),
                    RightLength = table.Column<float>(nullable: false),
                    IsBottomBorder = table.Column<bool>(nullable: false),
                    IsHypotenuseBorder = table.Column<bool>(nullable: false),
                    IsLeftBorder = table.Column<bool>(nullable: false),
                    IsTopBorder = table.Column<bool>(nullable: false),
                    IsRightBorder = table.Column<bool>(nullable: false),
                    BottomBorderThickness = table.Column<float>(nullable: true),
                    HypotenuseBorderThickness = table.Column<float>(nullable: true),
                    LeftBorderThickness = table.Column<float>(nullable: true),
                    TopBorderThickness = table.Column<float>(nullable: true),
                    RightBorderThickness = table.Column<float>(nullable: true),
                    IsMilling = table.Column<bool>(nullable: false),
                    Milling = table.Column<int>(nullable: true),
                    CutterLength = table.Column<float>(nullable: true),
                    CutterWidth = table.Column<float>(nullable: true),
                    CutterDepth = table.Column<float>(nullable: true),
                    TopSpace = table.Column<float>(nullable: true),
                    LeftSpace = table.Column<float>(nullable: true)
                  
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PentagonFormatters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PentagonFormatters_Fabrics_FabricId",
                        column: x => x.FabricId,
                        principalTable: "Fabrics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PentagonFormatters_Furnitures_FurnitureId",
                        column: x => x.FurnitureId,
                        principalTable: "Furnitures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RectangularFormatters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    Count = table.Column<int>(nullable: false),
                    IsMilling = table.Column<bool>(nullable: false),
                    Milling = table.Column<int>(nullable: true),
                    CutterLength = table.Column<float>(nullable: true),
                    CutterWidth = table.Column<float>(nullable: true),
                    CutterDepth = table.Column<float>(nullable: true),
                    TopSpace = table.Column<float>(nullable: true),
                    LeftSpace = table.Column<float>(nullable: true),
                    Width = table.Column<float>(nullable: false),
                    Length = table.Column<float>(nullable: false),
                    Thickness = table.Column<float>(nullable: false),
                    FurnitureId = table.Column<int>(nullable: false),
                    FabricId = table.Column<int>(nullable: false),
                    IsTopBorder = table.Column<bool>(nullable: false),
                    IsBottomBorder = table.Column<bool>(nullable: false),
                    IsRightBorder = table.Column<bool>(nullable: false),
                    IsLeftBorder = table.Column<bool>(nullable: false),
                    TopBorderThickness = table.Column<float>(nullable: true),
                    BottomBorderThickness = table.Column<float>(nullable: true),
                    RightBorderThickness = table.Column<float>(nullable: true),
                    LeftBorderThickness = table.Column<float>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RectangularFormatters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RectangularFormatters_Fabrics_FabricId",
                        column: x => x.FabricId,
                        principalTable: "Fabrics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_RectangularFormatters_Furnitures_FurnitureId",
                        column: x => x.FurnitureId,
                        principalTable: "Furnitures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TriangularFormatters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    Count = table.Column<int>(nullable: false),
                    Width = table.Column<float>(nullable: false),
                    Length = table.Column<float>(nullable: false),
                    Thickness = table.Column<float>(nullable: false),
                    FurnitureId = table.Column<int>(nullable: false),
                    FabricId = table.Column<int>(nullable: false),
                    HypotenuseLength = table.Column<float>(nullable: false),
                    IsBottomBorder = table.Column<bool>(nullable: false),
                    IsHypotenuseBorder = table.Column<bool>(nullable: false),
                    IsLeftBorder = table.Column<bool>(nullable: false),
                    BottomBorderThickness = table.Column<float>(nullable: true),
                    HypotenuseBorderThickness = table.Column<float>(nullable: true),
                    LeftBorderThickness = table.Column<float>(nullable: true),
                    IsMilling = table.Column<bool>(nullable: false),
                    Milling = table.Column<int>(nullable: true),
                    CutterLength = table.Column<float>(nullable: true),
                    CutterWidth = table.Column<float>(nullable: true),
                    CutterDepth = table.Column<float>(nullable: true),
                    TopSpace = table.Column<float>(nullable: true),
                    LeftSpace = table.Column<float>(nullable: true)
                  
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TriangularFormatters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TriangularFormatters_Fabrics_FabricId",
                        column: x => x.FabricId,
                        principalTable: "Fabrics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TriangularFormatters_Furnitures_FurnitureId",
                        column: x => x.FurnitureId,
                        principalTable: "Furnitures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fabrics_ProducerId",
                table: "Fabrics",
                column: "ProducerId");

            migrationBuilder.CreateIndex(
                name: "IX_Furnitures_OrderId",
                table: "Furnitures",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PentagonFormatters_FabricId",
                table: "PentagonFormatters",
                column: "FabricId");

            migrationBuilder.CreateIndex(
                name: "IX_PentagonFormatters_FurnitureId",
                table: "PentagonFormatters",
                column: "FurnitureId");

            migrationBuilder.CreateIndex(
                name: "IX_RectangularFormatters_FabricId",
                table: "RectangularFormatters",
                column: "FabricId");

            migrationBuilder.CreateIndex(
                name: "IX_RectangularFormatters_FurnitureId",
                table: "RectangularFormatters",
                column: "FurnitureId");

            migrationBuilder.CreateIndex(
                name: "IX_TriangularFormatters_FabricId",
                table: "TriangularFormatters",
                column: "FabricId");

            migrationBuilder.CreateIndex(
                name: "IX_TriangularFormatters_FurnitureId",
                table: "TriangularFormatters",
                column: "FurnitureId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PentagonFormatters");

            migrationBuilder.DropTable(
                name: "RectangularFormatters");

            migrationBuilder.DropTable(
                name: "TriangularFormatters");

            migrationBuilder.DropTable(
                name: "Fabrics");

            migrationBuilder.DropTable(
                name: "Furnitures");

            migrationBuilder.DropTable(
                name: "Producers");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
