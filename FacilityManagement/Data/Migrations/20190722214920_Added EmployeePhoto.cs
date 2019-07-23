using Microsoft.EntityFrameworkCore.Migrations;

namespace FacilityManagement.Web.Data.Migrations
{
    public partial class AddedEmployeePhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "AspNetUsers");
        }
    }
}
