using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BonusTrackerApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Team = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MexMonthlyGoals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    Month = table.Column<int>(type: "INTEGER", nullable: false),
                    GoalAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    ActualAmount = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MexMonthlyGoals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TikTokScores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<string>(type: "TEXT", nullable: false),
                    Score = table.Column<decimal>(type: "TEXT", nullable: false),
                    Duration = table.Column<int>(type: "INTEGER", nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    CycleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TikTokScores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Appeals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AgentId = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<string>(type: "TEXT", nullable: false),
                    OrderNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Platform = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    Outcome = table.Column<string>(type: "TEXT", nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    CycleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appeals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appeals_Agents_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Agents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MexAttendances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AgentId = table.Column<int>(type: "INTEGER", nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    Month = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MexAttendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MexAttendances_Agents_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Agents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MexLiveSales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AgentId = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<string>(type: "TEXT", nullable: false),
                    SalesAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    Month = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MexLiveSales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MexLiveSales_Agents_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Agents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsaPeriodData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AgentId = table.Column<int>(type: "INTEGER", nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    CycleId = table.Column<string>(type: "TEXT", nullable: false),
                    AmazonHealth = table.Column<string>(type: "TEXT", nullable: false),
                    CsQuality = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsaPeriodData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsaPeriodData_Agents_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Agents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Agents",
                columns: new[] { "Id", "Name", "Team" },
                values: new object[,]
                {
                    { 1, "Agent 1", "USA" },
                    { 2, "Agent 2", "USA" },
                    { 3, "Agent 1", "MEX" },
                    { 4, "Agent 2", "MEX" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appeals_AgentId",
                table: "Appeals",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_MexAttendances_AgentId_Year_Month",
                table: "MexAttendances",
                columns: new[] { "AgentId", "Year", "Month" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MexLiveSales_AgentId",
                table: "MexLiveSales",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_MexMonthlyGoals_Year_Month",
                table: "MexMonthlyGoals",
                columns: new[] { "Year", "Month" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsaPeriodData_AgentId_Year_CycleId",
                table: "UsaPeriodData",
                columns: new[] { "AgentId", "Year", "CycleId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appeals");

            migrationBuilder.DropTable(
                name: "MexAttendances");

            migrationBuilder.DropTable(
                name: "MexLiveSales");

            migrationBuilder.DropTable(
                name: "MexMonthlyGoals");

            migrationBuilder.DropTable(
                name: "TikTokScores");

            migrationBuilder.DropTable(
                name: "UsaPeriodData");

            migrationBuilder.DropTable(
                name: "Agents");
        }
    }
}
