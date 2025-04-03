using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JKKNIUBusBookingSystem.Migrations
{
    /// <inheritdoc />
    public partial class Changesmadeforbusseats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "Buses",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfExtraSeats",
                table: "Buses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfRowInCenter",
                table: "Buses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfSeatsInCenterRight",
                table: "Buses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfSeatsInCenterleft",
                table: "Buses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfSeatsInLastSeries",
                table: "Buses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ScheduleBuses",
                columns: table => new
                {
                    ScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartingAt = table.Column<TimeSpan>(type: "time", nullable: false),
                    ScheduledDate = table.Column<DateOnly>(type: "date", nullable: false),
                    StartingPlace = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleBuses", x => x.ScheduleId);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleBusSeats",
                columns: table => new
                {
                    ScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SeatName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScheduleBuses");

            migrationBuilder.DropTable(
                name: "ScheduleBusSeats");

            migrationBuilder.DropColumn(
                name: "NumberOfExtraSeats",
                table: "Buses");

            migrationBuilder.DropColumn(
                name: "NumberOfRowInCenter",
                table: "Buses");

            migrationBuilder.DropColumn(
                name: "NumberOfSeatsInCenterRight",
                table: "Buses");

            migrationBuilder.DropColumn(
                name: "NumberOfSeatsInCenterleft",
                table: "Buses");

            migrationBuilder.DropColumn(
                name: "NumberOfSeatsInLastSeries",
                table: "Buses");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Buses",
                newName: "id");
        }
    }
}
