using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniStay.Migrations
{
    /// <inheritdoc />
    public partial class addingAceptingstudenttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AcceptingStudentId",
                table: "Violation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AcceptingStudentId",
                table: "StudentDownloadLog",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AcceptingStudentId",
                table: "Payment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AcceptingStudentId",
                table: "Meal",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AcceptingStudentId",
                table: "MaintenanceRequest",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AcceptingStudentId",
                table: "Guardian",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AcceptingStudentId",
                table: "EvictionNotice",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AcceptingStudentId",
                table: "Document",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AcceptingStudentId",
                table: "Application",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AcceptingStudentId",
                table: "Absence",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AcceptingStudents",
                columns: table => new
                {
                    AcceptingStudentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullNameArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BirthPlace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Religion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Residence = table.Column<int>(type: "int", nullable: true),
                    ResidenceCountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResidenceGovernorate = table.Column<int>(type: "int", nullable: true),
                    ResidenceCity = table.Column<int>(type: "int", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DistanceFromHome = table.Column<double>(type: "float", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Faculty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudyType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Grade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GradePercentage = table.Column<double>(type: "float", nullable: true),
                    HousingType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NationalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FatherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FatherNationalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FatherJob = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FatherPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FamilyAbroad = table.Column<bool>(type: "bit", nullable: true),
                    SpecialNeeds = table.Column<bool>(type: "bit", nullable: true),
                    PassportNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PassportIssuePlace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HighSchoolDivision = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HighSchoolPercentage = table.Column<double>(type: "float", nullable: true),
                    HighSchoolTotal = table.Column<double>(type: "float", nullable: true),
                    HighSchoolFromAbroad = table.Column<bool>(type: "bit", nullable: true),
                    LastYearGrade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastYearPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PreviousHousingYears = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasMedicalCondition = table.Column<bool>(type: "bit", nullable: true),
                    DrugTestPassed = table.Column<bool>(type: "bit", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfilePhoto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegisteredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AdminRemarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    AllocationId = table.Column<int>(type: "int", nullable: true),
                    StudentLoginLoginId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcceptingStudents", x => x.AcceptingStudentId);
                    table.ForeignKey(
                        name: "FK_AcceptingStudents_Allocation_AllocationId",
                        column: x => x.AllocationId,
                        principalTable: "Allocation",
                        principalColumn: "AllocationID");
                    table.ForeignKey(
                        name: "FK_AcceptingStudents_StudentLogin_StudentLoginLoginId",
                        column: x => x.StudentLoginLoginId,
                        principalTable: "StudentLogin",
                        principalColumn: "LoginID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Violation_AcceptingStudentId",
                table: "Violation",
                column: "AcceptingStudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentDownloadLog_AcceptingStudentId",
                table: "StudentDownloadLog",
                column: "AcceptingStudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_AcceptingStudentId",
                table: "Payment",
                column: "AcceptingStudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Meal_AcceptingStudentId",
                table: "Meal",
                column: "AcceptingStudentId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequest_AcceptingStudentId",
                table: "MaintenanceRequest",
                column: "AcceptingStudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Guardian_AcceptingStudentId",
                table: "Guardian",
                column: "AcceptingStudentId");

            migrationBuilder.CreateIndex(
                name: "IX_EvictionNotice_AcceptingStudentId",
                table: "EvictionNotice",
                column: "AcceptingStudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Document_AcceptingStudentId",
                table: "Document",
                column: "AcceptingStudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Application_AcceptingStudentId",
                table: "Application",
                column: "AcceptingStudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Absence_AcceptingStudentId",
                table: "Absence",
                column: "AcceptingStudentId");

            migrationBuilder.CreateIndex(
                name: "IX_AcceptingStudents_AllocationId",
                table: "AcceptingStudents",
                column: "AllocationId");

            migrationBuilder.CreateIndex(
                name: "IX_AcceptingStudents_StudentLoginLoginId",
                table: "AcceptingStudents",
                column: "StudentLoginLoginId");

            migrationBuilder.AddForeignKey(
                name: "FK_Absence_AcceptingStudents_AcceptingStudentId",
                table: "Absence",
                column: "AcceptingStudentId",
                principalTable: "AcceptingStudents",
                principalColumn: "AcceptingStudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Application_AcceptingStudents_AcceptingStudentId",
                table: "Application",
                column: "AcceptingStudentId",
                principalTable: "AcceptingStudents",
                principalColumn: "AcceptingStudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Document_AcceptingStudents_AcceptingStudentId",
                table: "Document",
                column: "AcceptingStudentId",
                principalTable: "AcceptingStudents",
                principalColumn: "AcceptingStudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_EvictionNotice_AcceptingStudents_AcceptingStudentId",
                table: "EvictionNotice",
                column: "AcceptingStudentId",
                principalTable: "AcceptingStudents",
                principalColumn: "AcceptingStudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Guardian_AcceptingStudents_AcceptingStudentId",
                table: "Guardian",
                column: "AcceptingStudentId",
                principalTable: "AcceptingStudents",
                principalColumn: "AcceptingStudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceRequest_AcceptingStudents_AcceptingStudentId",
                table: "MaintenanceRequest",
                column: "AcceptingStudentId",
                principalTable: "AcceptingStudents",
                principalColumn: "AcceptingStudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meal_AcceptingStudents_AcceptingStudentId",
                table: "Meal",
                column: "AcceptingStudentId",
                principalTable: "AcceptingStudents",
                principalColumn: "AcceptingStudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_AcceptingStudents_AcceptingStudentId",
                table: "Payment",
                column: "AcceptingStudentId",
                principalTable: "AcceptingStudents",
                principalColumn: "AcceptingStudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentDownloadLog_AcceptingStudents_AcceptingStudentId",
                table: "StudentDownloadLog",
                column: "AcceptingStudentId",
                principalTable: "AcceptingStudents",
                principalColumn: "AcceptingStudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Violation_AcceptingStudents_AcceptingStudentId",
                table: "Violation",
                column: "AcceptingStudentId",
                principalTable: "AcceptingStudents",
                principalColumn: "AcceptingStudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Absence_AcceptingStudents_AcceptingStudentId",
                table: "Absence");

            migrationBuilder.DropForeignKey(
                name: "FK_Application_AcceptingStudents_AcceptingStudentId",
                table: "Application");

            migrationBuilder.DropForeignKey(
                name: "FK_Document_AcceptingStudents_AcceptingStudentId",
                table: "Document");

            migrationBuilder.DropForeignKey(
                name: "FK_EvictionNotice_AcceptingStudents_AcceptingStudentId",
                table: "EvictionNotice");

            migrationBuilder.DropForeignKey(
                name: "FK_Guardian_AcceptingStudents_AcceptingStudentId",
                table: "Guardian");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceRequest_AcceptingStudents_AcceptingStudentId",
                table: "MaintenanceRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_Meal_AcceptingStudents_AcceptingStudentId",
                table: "Meal");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_AcceptingStudents_AcceptingStudentId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentDownloadLog_AcceptingStudents_AcceptingStudentId",
                table: "StudentDownloadLog");

            migrationBuilder.DropForeignKey(
                name: "FK_Violation_AcceptingStudents_AcceptingStudentId",
                table: "Violation");

            migrationBuilder.DropTable(
                name: "AcceptingStudents");

            migrationBuilder.DropIndex(
                name: "IX_Violation_AcceptingStudentId",
                table: "Violation");

            migrationBuilder.DropIndex(
                name: "IX_StudentDownloadLog_AcceptingStudentId",
                table: "StudentDownloadLog");

            migrationBuilder.DropIndex(
                name: "IX_Payment_AcceptingStudentId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Meal_AcceptingStudentId",
                table: "Meal");

            migrationBuilder.DropIndex(
                name: "IX_MaintenanceRequest_AcceptingStudentId",
                table: "MaintenanceRequest");

            migrationBuilder.DropIndex(
                name: "IX_Guardian_AcceptingStudentId",
                table: "Guardian");

            migrationBuilder.DropIndex(
                name: "IX_EvictionNotice_AcceptingStudentId",
                table: "EvictionNotice");

            migrationBuilder.DropIndex(
                name: "IX_Document_AcceptingStudentId",
                table: "Document");

            migrationBuilder.DropIndex(
                name: "IX_Application_AcceptingStudentId",
                table: "Application");

            migrationBuilder.DropIndex(
                name: "IX_Absence_AcceptingStudentId",
                table: "Absence");

            migrationBuilder.DropColumn(
                name: "AcceptingStudentId",
                table: "Violation");

            migrationBuilder.DropColumn(
                name: "AcceptingStudentId",
                table: "StudentDownloadLog");

            migrationBuilder.DropColumn(
                name: "AcceptingStudentId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "AcceptingStudentId",
                table: "Meal");

            migrationBuilder.DropColumn(
                name: "AcceptingStudentId",
                table: "MaintenanceRequest");

            migrationBuilder.DropColumn(
                name: "AcceptingStudentId",
                table: "Guardian");

            migrationBuilder.DropColumn(
                name: "AcceptingStudentId",
                table: "EvictionNotice");

            migrationBuilder.DropColumn(
                name: "AcceptingStudentId",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "AcceptingStudentId",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "AcceptingStudentId",
                table: "Absence");
        }
    }
}
