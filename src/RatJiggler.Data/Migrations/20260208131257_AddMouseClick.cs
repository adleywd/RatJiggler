using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RatJiggler.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMouseClick : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClickButton",
                table: "SimpleMovementSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "EnableClick",
                table: "SimpleMovementSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ClickButton",
                table: "RealisticMovementSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "EnableClick",
                table: "RealisticMovementSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "RealisticMovementSettings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ClickButton", "EnableClick" },
                values: new object[] { 1, false });

            migrationBuilder.UpdateData(
                table: "SimpleMovementSettings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ClickButton", "EnableClick" },
                values: new object[] { 1, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClickButton",
                table: "SimpleMovementSettings");

            migrationBuilder.DropColumn(
                name: "EnableClick",
                table: "SimpleMovementSettings");

            migrationBuilder.DropColumn(
                name: "ClickButton",
                table: "RealisticMovementSettings");

            migrationBuilder.DropColumn(
                name: "EnableClick",
                table: "RealisticMovementSettings");
        }
    }
}
