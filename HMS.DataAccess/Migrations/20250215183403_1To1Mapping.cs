using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HMS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class _1To1Mapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalHistories_Patients_PatientId",
                table: "MedicalHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalHistories_Staff_StaffId",
                table: "MedicalHistories");

            migrationBuilder.DropIndex(
                name: "IX_MedicalHistories_PatientId",
                table: "MedicalHistories");

            migrationBuilder.DropIndex(
                name: "IX_MedicalHistories_StaffId",
                table: "MedicalHistories");

            

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "MedicalHistories");

            migrationBuilder.DropColumn(
                name: "StaffId",
                table: "MedicalHistories");

            migrationBuilder.DropColumn(
                name: "VisitedDate",
                table: "MedicalHistories");

            migrationBuilder.AlterColumn<string>(
                name: "TreatmentPlan",
                table: "MedicalHistories",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Prescription",
                table: "MedicalHistories",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Diagnosis",
                table: "MedicalHistories",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AppointmentId",
                table: "MedicalHistories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VisitDate",
                table: "MedicalHistories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistories_AppointmentId",
                table: "MedicalHistories",
                column: "AppointmentId",
                unique: true,
                filter: "[AppointmentId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalHistories_Appointments_AppointmentId",
                table: "MedicalHistories",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalHistories_Appointments_AppointmentId",
                table: "MedicalHistories");

            migrationBuilder.DropIndex(
                name: "IX_MedicalHistories_AppointmentId",
                table: "MedicalHistories");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "MedicalHistories");

            migrationBuilder.DropColumn(
                name: "VisitDate",
                table: "MedicalHistories");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "StaffSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "TreatmentPlan",
                table: "MedicalHistories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "Prescription",
                table: "MedicalHistories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Diagnosis",
                table: "MedicalHistories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<string>(
                name: "PatientId",
                table: "MedicalHistories",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StaffId",
                table: "MedicalHistories",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "VisitedDate",
                table: "MedicalHistories",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistories_PatientId",
                table: "MedicalHistories",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistories_StaffId",
                table: "MedicalHistories",
                column: "StaffId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalHistories_Patients_PatientId",
                table: "MedicalHistories",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalHistories_Staff_StaffId",
                table: "MedicalHistories",
                column: "StaffId",
                principalTable: "Staff",
                principalColumn: "Id");
        }
    }
}
