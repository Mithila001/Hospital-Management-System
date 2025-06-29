using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagementSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ModifyAllTableField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "YearsOfExperience",
                table: "Nurses",
                newName: "YearsOfExperience_Nurce");

            migrationBuilder.RenameColumn(
                name: "Certifications",
                table: "Nurses",
                newName: "Certifications_Nurce");

            migrationBuilder.RenameColumn(
                name: "YearsOfExperience",
                table: "Doctors",
                newName: "YearsOfExperience_Doc");

            migrationBuilder.RenameColumn(
                name: "LicenseExpiryDate",
                table: "Doctors",
                newName: "LicenseExpiryDate_Doc");

            migrationBuilder.RenameColumn(
                name: "Certifications",
                table: "Doctors",
                newName: "Certifications_Doc");

            migrationBuilder.AddColumn<int>(
                name: "StaffRole",
                table: "StaffMembers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StaffRole",
                table: "StaffMembers");

            migrationBuilder.RenameColumn(
                name: "YearsOfExperience_Nurce",
                table: "Nurses",
                newName: "YearsOfExperience");

            migrationBuilder.RenameColumn(
                name: "Certifications_Nurce",
                table: "Nurses",
                newName: "Certifications");

            migrationBuilder.RenameColumn(
                name: "YearsOfExperience_Doc",
                table: "Doctors",
                newName: "YearsOfExperience");

            migrationBuilder.RenameColumn(
                name: "LicenseExpiryDate_Doc",
                table: "Doctors",
                newName: "LicenseExpiryDate");

            migrationBuilder.RenameColumn(
                name: "Certifications_Doc",
                table: "Doctors",
                newName: "Certifications");
        }
    }
}
