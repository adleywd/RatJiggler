using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RatJiggler.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSimpleUserInterventionDetection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EnableUserInterventionDetection",
                table: "SimpleMovementSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "SimpleMovementSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "EnableUserInterventionDetection",
                value: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnableUserInterventionDetection",
                table: "SimpleMovementSettings");
        }
    }
}
