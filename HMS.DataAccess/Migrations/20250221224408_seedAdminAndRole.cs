using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HMS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class seedAdminAndRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MedicalHistories_AppointmentId",
                table: "MedicalHistories");

            migrationBuilder.AlterColumn<int>(
                name: "AppointmentId",
                table: "MedicalHistories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistories_AppointmentId",
                table: "MedicalHistories",
                column: "AppointmentId",
                unique: true);


            //adding Roles
            migrationBuilder.InsertData(
             table: "AspNetRoles",
             columns: new[]
            {
                    "Id","Name","NormalizedName","ConcurrencyStamp"
            },
              values: new object[]
            {
                    "0e94d03d-3f1c-444d-93c7-bfa16aef605f","Admin","Admin".ToUpper(),Guid.NewGuid().ToString()
            }
            );
            //Inserting a Librarian user
            var hasher = new PasswordHasher<IdentityUser>();
           // hasher.HashPassword(null, "This@librarian12")

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[]
                {
            "Id", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed",
            "PasswordHash", "SecurityStamp", "ConcurrencyStamp", "PhoneNumber", "PhoneNumberConfirmed",
            "TwoFactorEnabled", "LockoutEnabled", "AccessFailedCount",
            "Address", "FullName","IsDeleted"
                },
                values: new object[]
                {
            "fce1b435-c815-4fbf-aab3-b4aedf241737", "Admin1", "ADMIN1", "admin@gmail.com", "ADMIN@GMAIL.COM", true,
            hasher.HashPassword(null, "This@admin12"), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), null, false,
            false, true, 0,
            "AdminInCairo", "Admin",false
                }
            );
            // Assign Admin User to admin Role 
            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "fce1b435-c815-4fbf-aab3-b4aedf241737", "0e94d03d-3f1c-444d-93c7-bfa16aef605f" }
            );







        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MedicalHistories_AppointmentId",
                table: "MedicalHistories");

            migrationBuilder.AlterColumn<int>(
                name: "AppointmentId",
                table: "MedicalHistories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistories_AppointmentId",
                table: "MedicalHistories",
                column: "AppointmentId",
                unique: true,
                filter: "[AppointmentId] IS NOT NULL");
        }
    }
}
