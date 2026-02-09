using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RatJiggler.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddClickInterval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClickIntervalSeconds",
                table: "SimpleMovementSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClickIntervalSeconds",
                table: "RealisticMovementSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "RealisticMovementSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "ClickIntervalSeconds",
                value: 5);

            migrationBuilder.UpdateData(
                table: "SimpleMovementSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "ClickIntervalSeconds",
                value: 5);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClickIntervalSeconds",
                table: "SimpleMovementSettings");

            migrationBuilder.DropColumn(
                name: "ClickIntervalSeconds",
                table: "RealisticMovementSettings");
        }
    }
}
