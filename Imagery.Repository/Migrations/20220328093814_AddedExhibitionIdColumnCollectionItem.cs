using Microsoft.EntityFrameworkCore.Migrations;

namespace Imagery.Repository.Migrations
{
    public partial class AddedExhibitionIdColumnCollectionItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExhibitionId",
                table: "CollectionItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExhibitionId",
                table: "CollectionItems");
        }
    }
}
