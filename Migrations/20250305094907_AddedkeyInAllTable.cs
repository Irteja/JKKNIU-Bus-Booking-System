using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JKKNIUBusBookingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddedkeyInAllTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SeatName",
                table: "ScheduleBusSeats",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScheduleBusSeats",
                table: "ScheduleBusSeats",
                columns: new[] { "ScheduleId", "SeatName" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ScheduleBusSeats",
                table: "ScheduleBusSeats");

            migrationBuilder.AlterColumn<string>(
                name: "SeatName",
                table: "ScheduleBusSeats",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
