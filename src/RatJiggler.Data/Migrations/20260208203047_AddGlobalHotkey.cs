using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RatJiggler.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGlobalHotkey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HotkeyKey",
                table: "ApplicationSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HotkeyModifiers",
                table: "ApplicationSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "ApplicationSettings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "HotkeyKey", "HotkeyModifiers" },
                values: new object[] { 98, 3 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HotkeyKey",
                table: "ApplicationSettings");

            migrationBuilder.DropColumn(
                name: "HotkeyModifiers",
                table: "ApplicationSettings");
        }
    }
}
