using Microsoft.EntityFrameworkCore.Migrations;

namespace Imagery.Repository.Migrations
{
    public partial class AddedDimensionsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dimensions",
                table: "ExponentItems");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ExponentItems");

            migrationBuilder.CreateTable(
                name: "Dimensions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Dimension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    ExponentItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dimensions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dimensions_ExponentItems_ExponentItemId",
                        column: x => x.ExponentItemId,
                        principalTable: "ExponentItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dimensions_ExponentItemId",
                table: "Dimensions",
                column: "ExponentItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dimensions");

            migrationBuilder.AddColumn<string>(
                name: "Dimensions",
                table: "ExponentItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "ExponentItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
