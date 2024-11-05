using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagementSystem2.Migrations
{
    /// <inheritdoc />
    public partial class ScheduleAvailabilityAndDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                table: "Schedules");

            migrationBuilder.RenameColumn(
                name: "ShiftStart",
                table: "Schedules",
                newName: "AvailableTo");

            migrationBuilder.RenameColumn(
                name: "ShiftEnd",
                table: "Schedules",
                newName: "AvailableFrom");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Schedules",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Schedules");

            migrationBuilder.RenameColumn(
                name: "AvailableTo",
                table: "Schedules",
                newName: "ShiftStart");

            migrationBuilder.RenameColumn(
                name: "AvailableFrom",
                table: "Schedules",
                newName: "ShiftEnd");

            migrationBuilder.AddColumn<string>(
                name: "DayOfWeek",
                table: "Schedules",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
