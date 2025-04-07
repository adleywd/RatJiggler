using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RatJiggler.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SelectedTabIndex = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NormalMovementSettings",
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
                    table.PrimaryKey("PK_NormalMovementSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RealisticMovementSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MinSpeed = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxSpeed = table.Column<int>(type: "INTEGER", nullable: false),
                    EnableStepPauses = table.Column<bool>(type: "INTEGER", nullable: false),
                    StepPauseMin = table.Column<int>(type: "INTEGER", nullable: false),
                    StepPauseMax = table.Column<int>(type: "INTEGER", nullable: false),
                    EnableRandomPauses = table.Column<bool>(type: "INTEGER", nullable: false),
                    RandomPauseProbability = table.Column<int>(type: "INTEGER", nullable: false),
                    RandomPauseMin = table.Column<int>(type: "INTEGER", nullable: false),
                    RandomPauseMax = table.Column<int>(type: "INTEGER", nullable: false),
                    HorizontalBias = table.Column<float>(type: "REAL", nullable: false),
                    VerticalBias = table.Column<float>(type: "REAL", nullable: false),
                    PaddingPercentage = table.Column<float>(type: "REAL", nullable: false),
                    RandomSeed = table.Column<int>(type: "INTEGER", nullable: true),
                    EnableUserInterventionDetection = table.Column<bool>(type: "INTEGER", nullable: false),
                    MovementThresholdInPixels = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealisticMovementSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MoveX = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 50),
                    MoveY = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    Duration = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 60),
                    BackForth = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    MinSpeed = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 3),
                    MaxSpeed = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 7),
                    EnableStepPauses = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    StepPauseMin = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 20),
                    StepPauseMax = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 50),
                    EnableRandomPauses = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    RandomPauseProbability = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 10),
                    RandomPauseMin = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 100),
                    RandomPauseMax = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 500),
                    HorizontalBias = table.Column<float>(type: "REAL", nullable: false, defaultValue: 0f),
                    VerticalBias = table.Column<float>(type: "REAL", nullable: false, defaultValue: 0f),
                    PaddingPercentage = table.Column<float>(type: "REAL", nullable: false, defaultValue: 0.1f),
                    SelectedMouseMovementModeIndex = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    RandomSeed = table.Column<int>(type: "INTEGER", nullable: true),
                    EnableUserInterventionDetection = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    MovementThresholdInPixels = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 10)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettings", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ApplicationSettings",
                columns: new[] { "Id", "SelectedTabIndex" },
                values: new object[] { 1, 0 });

            migrationBuilder.InsertData(
                table: "NormalMovementSettings",
                columns: new[] { "Id", "BackAndForth", "Duration", "MoveX", "MoveY" },
                values: new object[] { 1, true, 60, 50, 0 });

            migrationBuilder.InsertData(
                table: "RealisticMovementSettings",
                columns: new[] { "Id", "EnableRandomPauses", "EnableStepPauses", "EnableUserInterventionDetection", "HorizontalBias", "MaxSpeed", "MinSpeed", "MovementThresholdInPixels", "PaddingPercentage", "RandomPauseMax", "RandomPauseMin", "RandomPauseProbability", "RandomSeed", "StepPauseMax", "StepPauseMin", "VerticalBias" },
                values: new object[] { 1, true, true, true, 0f, 7, 3, 10, 0.1f, 500, 100, 10, null, 50, 20, 0f });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationSettings");

            migrationBuilder.DropTable(
                name: "NormalMovementSettings");

            migrationBuilder.DropTable(
                name: "RealisticMovementSettings");

            migrationBuilder.DropTable(
                name: "UserSettings");
        }
    }
}
