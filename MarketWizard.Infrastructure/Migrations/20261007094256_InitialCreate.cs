using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CronJobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IndicePerfomance = table.Column<string>(type: "text", nullable: true),
                    TopNews = table.Column<string>(type: "text", nullable: true),
                    SectorPerformance = table.Column<string>(type: "text", nullable: true),
                    Gainers = table.Column<string>(type: "text", nullable: true),
                    Losers = table.Column<string>(type: "text", nullable: true),
                    TopIndustries = table.Column<string>(type: "text", nullable: true),
                    WorstIndustries = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CronJobs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CronJobs");
        }
    }
}
