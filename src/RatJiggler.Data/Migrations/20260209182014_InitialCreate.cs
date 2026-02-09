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
                    SelectedTabIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    AutoStartMovement = table.Column<bool>(type: "INTEGER", nullable: false),
                    MinimizeToTray = table.Column<bool>(type: "INTEGER", nullable: false),
                    StartMinimizedToTray = table.Column<bool>(type: "INTEGER", nullable: false),
                    HotkeyModifiers = table.Column<int>(type: "INTEGER", nullable: false),
                    HotkeyKey = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationSettings", x => x.Id);
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
                    MovementThresholdInPixels = table.Column<int>(type: "INTEGER", nullable: false),
                    EnableClick = table.Column<bool>(type: "INTEGER", nullable: false),
                    ClickButton = table.Column<int>(type: "INTEGER", nullable: false),
                    ClickIntervalSeconds = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealisticMovementSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SimpleMovementSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MoveX = table.Column<int>(type: "INTEGER", nullable: false),
                    MoveY = table.Column<int>(type: "INTEGER", nullable: false),
                    Duration = table.Column<int>(type: "INTEGER", nullable: false),
                    BackAndForth = table.Column<bool>(type: "INTEGER", nullable: false),
                    EnableClick = table.Column<bool>(type: "INTEGER", nullable: false),
                    ClickButton = table.Column<int>(type: "INTEGER", nullable: false),
                    ClickIntervalSeconds = table.Column<int>(type: "INTEGER", nullable: false),
                    EnableUserInterventionDetection = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimpleMovementSettings", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ApplicationSettings",
                columns: new[] { "Id", "AutoStartMovement", "HotkeyKey", "HotkeyModifiers", "MinimizeToTray", "SelectedTabIndex", "StartMinimizedToTray" },
                values: new object[] { 1, false, 98, 3, false, 0, false });

            migrationBuilder.InsertData(
                table: "RealisticMovementSettings",
                columns: new[] { "Id", "ClickButton", "ClickIntervalSeconds", "EnableClick", "EnableRandomPauses", "EnableStepPauses", "EnableUserInterventionDetection", "HorizontalBias", "MaxSpeed", "MinSpeed", "MovementThresholdInPixels", "PaddingPercentage", "RandomPauseMax", "RandomPauseMin", "RandomPauseProbability", "RandomSeed", "StepPauseMax", "StepPauseMin", "VerticalBias" },
                values: new object[] { 1, 1, 5, false, true, true, true, 0f, 7, 3, 10, 0.1f, 500, 100, 10, null, 50, 20, 0f });

            migrationBuilder.InsertData(
                table: "SimpleMovementSettings",
                columns: new[] { "Id", "BackAndForth", "ClickButton", "ClickIntervalSeconds", "Duration", "EnableClick", "EnableUserInterventionDetection", "MoveX", "MoveY" },
                values: new object[] { 1, true, 1, 5, 60, false, true, 50, 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationSettings");

            migrationBuilder.DropTable(
                name: "RealisticMovementSettings");

            migrationBuilder.DropTable(
                name: "SimpleMovementSettings");
        }
    }
}
