using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkTAP.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => new { x.PersonId, x.Name });
                    table.ForeignKey(
                        name: "FK_Skills_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "DisplayName", "Name" },
                values: new object[] { 1, "Nik", "Nikita" });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "DisplayName", "Name" },
                values: new object[] { 2, "Habib", "Alexandr" });

            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "Name", "PersonId", "Level" },
                values: new object[] { "C#", 1, (byte)6 });

            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "Name", "PersonId", "Level" },
                values: new object[] { "PHP", 2, (byte)3 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "Persons");
        }
    }
}
