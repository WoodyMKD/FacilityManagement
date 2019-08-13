using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FacilityManagement.API.Migrations
{
    public partial class ModelChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parts");

            migrationBuilder.DropTable(
                name: "CompressorSystems");

            migrationBuilder.DropTable(
                name: "CompressorSubTypes");

            migrationBuilder.DropTable(
                name: "Compressors");

            migrationBuilder.CreateTable(
                name: "InventoryObjects",
                columns: table => new
                {
                    InventoryObjectId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Category = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Manufacturer = table.Column<string>(nullable: true),
                    Model = table.Column<string>(nullable: true),
                    WorkingHours = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryObjects", x => x.InventoryObjectId);
                });

            migrationBuilder.CreateTable(
                name: "InventoryObjectTypes",
                columns: table => new
                {
                    InventoryObjectTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    InventoryObjectId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryObjectTypes", x => x.InventoryObjectTypeId);
                    table.ForeignKey(
                        name: "FK_InventoryObjectTypes_InventoryObjects_InventoryObjectId",
                        column: x => x.InventoryObjectId,
                        principalTable: "InventoryObjects",
                        principalColumn: "InventoryObjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventoryObjectSystems",
                columns: table => new
                {
                    InventoryObjectSystemId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    InventoryObjectTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryObjectSystems", x => x.InventoryObjectSystemId);
                    table.ForeignKey(
                        name: "FK_InventoryObjectSystems_InventoryObjectTypes_InventoryObjectTypeId",
                        column: x => x.InventoryObjectTypeId,
                        principalTable: "InventoryObjectTypes",
                        principalColumn: "InventoryObjectTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventoryObjectParts",
                columns: table => new
                {
                    InventoryObjectPartId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    InventoryObjectSystemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryObjectParts", x => x.InventoryObjectPartId);
                    table.ForeignKey(
                        name: "FK_InventoryObjectParts_InventoryObjectSystems_InventoryObjectSystemId",
                        column: x => x.InventoryObjectSystemId,
                        principalTable: "InventoryObjectSystems",
                        principalColumn: "InventoryObjectSystemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryObjectParts_InventoryObjectSystemId",
                table: "InventoryObjectParts",
                column: "InventoryObjectSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryObjectSystems_InventoryObjectTypeId",
                table: "InventoryObjectSystems",
                column: "InventoryObjectTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryObjectTypes_InventoryObjectId",
                table: "InventoryObjectTypes",
                column: "InventoryObjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryObjectParts");

            migrationBuilder.DropTable(
                name: "InventoryObjectSystems");

            migrationBuilder.DropTable(
                name: "InventoryObjectTypes");

            migrationBuilder.DropTable(
                name: "InventoryObjects");

            migrationBuilder.CreateTable(
                name: "Compressors",
                columns: table => new
                {
                    CompressorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    Manufacturer = table.Column<string>(nullable: true),
                    Model = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    WorkingHours = table.Column<int>(nullable: false)
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
                    CompressorId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true)
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
                    CompressorSubTypeId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true)
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
                    CompressorSystemId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true)
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
    }
}
