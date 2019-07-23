using Microsoft.EntityFrameworkCore.Migrations;

namespace FacilityManagement.Data.Migrations
{
    public partial class InventoryObjectTypeconvertedtoEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "InventoryObjects",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "InventoryObjects",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
