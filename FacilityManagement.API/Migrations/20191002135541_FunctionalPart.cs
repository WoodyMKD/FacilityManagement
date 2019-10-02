using Microsoft.EntityFrameworkCore.Migrations;

namespace FacilityManagement.API.Migrations
{
    public partial class FunctionalPart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Functional",
                table: "InventoryObjectParts",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Functional",
                table: "InventoryObjectParts");
        }
    }
}
