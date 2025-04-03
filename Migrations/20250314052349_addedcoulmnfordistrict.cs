using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JKKNIUBusBookingSystem.Migrations
{
    /// <inheritdoc />
    public partial class addedcoulmnfordistrict : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "theTownBusIsNow",
                table: "ScheduleBuses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "theTownBusIsNow",
                table: "ScheduleBuses");
        }
    }
}
