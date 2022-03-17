using Microsoft.EntityFrameworkCore.Migrations;

namespace Imagery.Repository.Migrations
{
    public partial class AddedOrganizatorColumnTableCollectionItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Organizer",
                table: "CollectionItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Organizer",
                table: "CollectionItems");
        }
    }
}
