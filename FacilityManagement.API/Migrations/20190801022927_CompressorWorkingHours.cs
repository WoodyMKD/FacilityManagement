using Microsoft.EntityFrameworkCore.Migrations;

namespace FacilityManagement.API.Migrations
{
    public partial class CompressorWorkingHours : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkingHours",
                table: "Compressors",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkingHours",
                table: "Compressors");
        }
    }
}
