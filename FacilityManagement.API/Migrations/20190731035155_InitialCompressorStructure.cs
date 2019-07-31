using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FacilityManagement.API.Migrations
{
    public partial class InitialCompressorStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Compressors",
                columns: table => new
                {
                    CompressorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Manufacturer = table.Column<string>(nullable: true),
                    Model = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compressors", x => x.CompressorId);
                });

            migrationBuilder.CreateTable(
                name: "CompressorSubTypes",
                columns: table => new
                {
                    CompressorSubTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    CompressorId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompressorSubTypes", x => x.CompressorSubTypeId);
                    table.ForeignKey(
                        name: "FK_CompressorSubTypes_Compressors_CompressorId",
                        column: x => x.CompressorId,
                        principalTable: "Compressors",
                        principalColumn: "CompressorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompressorSystems",
                columns: table => new
                {
                    CompressorSystemId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    CompressorSubTypeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompressorSystems", x => x.CompressorSystemId);
                    table.ForeignKey(
                        name: "FK_CompressorSystems_CompressorSubTypes_CompressorSubTypeId",
                        column: x => x.CompressorSubTypeId,
                        principalTable: "CompressorSubTypes",
                        principalColumn: "CompressorSubTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Parts",
                columns: table => new
                {
                    PartId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    CompressorSystemId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parts", x => x.PartId);
                    table.ForeignKey(
                        name: "FK_Parts_CompressorSystems_CompressorSystemId",
                        column: x => x.CompressorSystemId,
                        principalTable: "CompressorSystems",
                        principalColumn: "CompressorSystemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompressorSubTypes_CompressorId",
                table: "CompressorSubTypes",
                column: "CompressorId");

            migrationBuilder.CreateIndex(
                name: "IX_CompressorSystems_CompressorSubTypeId",
                table: "CompressorSystems",
                column: "CompressorSubTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Parts_CompressorSystemId",
                table: "Parts",
                column: "CompressorSystemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parts");

            migrationBuilder.DropTable(
                name: "CompressorSystems");

            migrationBuilder.DropTable(
                name: "CompressorSubTypes");

            migrationBuilder.DropTable(
                name: "Compressors");
        }
    }
}
