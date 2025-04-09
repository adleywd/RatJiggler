using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RatJiggler.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangedNameNormalToSimpleMovement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NormalMovementSettings");

            migrationBuilder.CreateTable(
                name: "SimpleMovementSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MoveX = table.Column<int>(type: "INTEGER", nullable: false),
                    MoveY = table.Column<int>(type: "INTEGER", nullable: false),
                    Duration = table.Column<int>(type: "INTEGER", nullable: false),
                    BackAndForth = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimpleMovementSettings", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "SimpleMovementSettings",
                columns: new[] { "Id", "BackAndForth", "Duration", "MoveX", "MoveY" },
                values: new object[] { 1, true, 60, 50, 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SimpleMovementSettings");

            migrationBuilder.CreateTable(
                name: "NormalMovementSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BackAndForth = table.Column<bool>(type: "INTEGER", nullable: false),
                    Duration = table.Column<int>(type: "INTEGER", nullable: false),
                    MoveX = table.Column<int>(type: "INTEGER", nullable: false),
                    MoveY = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NormalMovementSettings", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "NormalMovementSettings",
                columns: new[] { "Id", "BackAndForth", "Duration", "MoveX", "MoveY" },
                values: new object[] { 1, true, 60, 50, 0 });
        }
    }
}
