using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniStay.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStudentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    StudentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StudentStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "New"),
                    Nationality = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Egyptian"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FullNameArabic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: true),
                    BirthPlace = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Religion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Residence = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ResidenceCountry = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, defaultValue: "مصر"),
                    ResidenceGovernorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ResidenceCity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    DistanceFromHome = table.Column<double>(type: "float", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Faculty = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Department = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    StudyType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Grade = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    GradePercentage = table.Column<double>(type: "float", nullable: true),
                    HousingType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    NationalID = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    FatherName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FatherNationalID = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: true),
                    FatherJob = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FatherPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ParentStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FamilyAbroad = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    SpecialNeeds = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    PassportNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PassportIssuePlace = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    HighSchoolDivision = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    HighSchoolPercentage = table.Column<double>(type: "float", nullable: true),
                    HighSchoolTotal = table.Column<double>(type: "float", nullable: true),
                    HighSchoolFromAbroad = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    LastYearGrade = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastYearPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    PreviousHousingYears = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasMedicalCondition = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DrugTestPassed = table.Column<bool>(type: "bit", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfilePhoto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysdatetime())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Student__32C52A79F8FED0B2", x => x.StudentID);
                });

            migrationBuilder.CreateTable(
                name: "University",
                columns: table => new
                {
                    UniversityID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniversityName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Governorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysdatetime())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Universi__9F19E19C102D5BC8", x => x.UniversityID);
                });

            migrationBuilder.CreateTable(
                name: "Guardian",
                columns: table => new
                {
                    GuardianID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    GuardianRole = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NationalID = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Relation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AlternatePhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Job = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ResidenceAddress = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysdatetime())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Guardian__0A5E1B7BA9195463", x => x.GuardianID);
                    table.ForeignKey(
                        name: "FK__Guardian__Studen__7F2BE32F",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "StudentID");
                });

            migrationBuilder.CreateTable(
                name: "StudentDownloadLog",
                columns: table => new
                {
                    LogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    DownloadedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__StudentD__5E5499A837B7D305", x => x.LogID);
                    table.ForeignKey(
                        name: "FK__StudentDo__Stude__7B264821",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "StudentID");
                });

            migrationBuilder.CreateTable(
                name: "StudentLogin",
                columns: table => new
                {
                    LoginID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    NationalID = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(512)", maxLength: 512, nullable: true),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysdatetime())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__StudentL__4DDA28384EF1ECE5", x => x.LoginID);
                    table.ForeignKey(
                        name: "FK__StudentLo__Stude__7755B73D",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "StudentID");
                });

            migrationBuilder.CreateTable(
                name: "Dormitory",
                columns: table => new
                {
                    DormitoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DormitoryName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    UniversityID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysdatetime())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Dormitor__14188ACE157AEAF1", x => x.DormitoryID);
                    table.ForeignKey(
                        name: "FK__Dormitory__Unive__5165187F",
                        column: x => x.UniversityID,
                        principalTable: "University",
                        principalColumn: "UniversityID");
                });

            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    AdminID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(512)", maxLength: 512, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DormitoryID = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysdatetime())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Admin__719FE4E880AFF735", x => x.AdminID);
                    table.ForeignKey(
                        name: "FK__Admin__Dormitory__06CD04F7",
                        column: x => x.DormitoryID,
                        principalTable: "Dormitory",
                        principalColumn: "DormitoryID");
                });

            migrationBuilder.CreateTable(
                name: "ApplicationSchedule",
                columns: table => new
                {
                    ScheduleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AcademicYear = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StudentCategory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FromDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ToDate = table.Column<DateOnly>(type: "date", nullable: false),
                    DormitoryID = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysdatetime())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Applicat__9C8A5B69868F8CF2", x => x.ScheduleID);
                    table.ForeignKey(
                        name: "FK__Applicati__Dormi__0D7A0286",
                        column: x => x.DormitoryID,
                        principalTable: "Dormitory",
                        principalColumn: "DormitoryID");
                });

            migrationBuilder.CreateTable(
                name: "Building",
                columns: table => new
                {
                    BuildingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuildingName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BuildingType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FloorsCount = table.Column<int>(type: "int", nullable: true),
                    TotalRooms = table.Column<int>(type: "int", nullable: true, defaultValue: 150),
                    DormitoryID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysdatetime())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Building__5463CDE4A6F80590", x => x.BuildingID);
                    table.ForeignKey(
                        name: "FK__Building__Dormit__59063A47",
                        column: x => x.DormitoryID,
                        principalTable: "Dormitory",
                        principalColumn: "DormitoryID");
                });

            migrationBuilder.CreateTable(
                name: "Announcement",
                columns: table => new
                {
                    AnnouncementID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicYear = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    UniversityID = table.Column<int>(type: "int", nullable: true),
                    DormitoryID = table.Column<int>(type: "int", nullable: true),
                    IsPublic = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysdatetime())"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysdatetime())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Announce__9DE44554A69D832F", x => x.AnnouncementID);
                    table.ForeignKey(
                        name: "FK__Announcem__Creat__6AEFE058",
                        column: x => x.CreatedBy,
                        principalTable: "Admin",
                        principalColumn: "AdminID");
                    table.ForeignKey(
                        name: "FK__Announcem__Dormi__69FBBC1F",
                        column: x => x.DormitoryID,
                        principalTable: "Dormitory",
                        principalColumn: "DormitoryID");
                    table.ForeignKey(
                        name: "FK__Announcem__Unive__690797E6",
                        column: x => x.UniversityID,
                        principalTable: "University",
                        principalColumn: "UniversityID");
                });

            migrationBuilder.CreateTable(
                name: "Violation",
                columns: table => new
                {
                    ViolationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<int>(type: "int", nullable: true),
                    ViolationType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ViolationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Penalty = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PenaltyAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    IsPaid = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    RecordedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysdatetime())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Violatio__18B6DC2893B67D33", x => x.ViolationID);
                    table.ForeignKey(
                        name: "FK__Violation__Recor__5224328E",
                        column: x => x.RecordedBy,
                        principalTable: "Admin",
                        principalColumn: "AdminID");
                    table.ForeignKey(
                        name: "FK__Violation__Stude__51300E55",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "StudentID");
                });

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    RoomID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Floor = table.Column<int>(type: "int", nullable: true),
                    RoomType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BedsCount = table.Column<int>(type: "int", nullable: true),
                    CurrentOccupancy = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    BuildingID = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    HasBalcony = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    HasPrivateBathroom = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    HasAC = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    HasFridge = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    FloorMaterial = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysdatetime())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Room__328639197573C58E", x => x.RoomID);
                    table.ForeignKey(
                        name: "FK__Room__BuildingID__66603565",
                        column: x => x.BuildingID,
                        principalTable: "Building",
                        principalColumn: "BuildingID");
                });

            migrationBuilder.CreateTable(
                name: "AnnouncementAttachment",
                columns: table => new
                {
                    AttachmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnnouncementID = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysdatetime())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Announce__442C64DE72D109DF", x => x.AttachmentID);
                    table.ForeignKey(
                        name: "FK__Announcem__Annou__6FB49575",
                        column: x => x.AnnouncementID,
                        principalTable: "Announcement",
                        principalColumn: "AnnouncementID");
                });

            migrationBuilder.CreateTable(
                name: "Application",
                columns: table => new
                {
                    ApplicationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    AcademicYear = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Semester = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    HousingPreference = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PreferredBuildingID = table.Column<int>(type: "int", nullable: true),
                    PreferredRoomID = table.Column<int>(type: "int", nullable: true),
                    PreferredBedNumber = table.Column<int>(type: "int", nullable: true),
                    MealPlanType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WithoutNutrition = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    FamilyAbroad = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    SpecialNeeds = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    ApplicationDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysdatetime())"),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Pending"),
                    Priority = table.Column<int>(type: "int", nullable: true),
                    RequiredDocuments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewedBy = table.Column<int>(type: "int", nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysdatetime())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Applicat__C93A4F79D368EB6C", x => x.ApplicationID);
                    table.ForeignKey(
                        name: "FK__Applicati__Prefe__1BC821DD",
                        column: x => x.PreferredBuildingID,
                        principalTable: "Building",
                        principalColumn: "BuildingID");
                    table.ForeignKey(
                        name: "FK__Applicati__Prefe__1CBC4616",
                        column: x => x.PreferredRoomID,
                        principalTable: "Room",
                        principalColumn: "RoomID");
                    table.ForeignKey(
                        name: "FK__Applicati__Revie__1AD3FDA4",
                        column: x => x.ReviewedBy,
                        principalTable: "Admin",
                        principalColumn: "AdminID");
                    table.ForeignKey(
                        name: "FK__Applicati__Stude__19DFD96B",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "StudentID");
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceRequest",
                columns: table => new
                {
                    RequestID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomID = table.Column<int>(type: "int", nullable: true),
                    StudentID = table.Column<int>(type: "int", nullable: true),
                    IssueType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Priority = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssignedTo = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysdatetime())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Maintena__33A8519A754946C8", x => x.RequestID);
                    table.ForeignKey(
                        name: "FK__Maintenan__Assig__5AB9788F",
                        column: x => x.AssignedTo,
                        principalTable: "Admin",
                        principalColumn: "AdminID");
                    table.ForeignKey(
                        name: "FK__Maintenan__RoomI__58D1301D",
                        column: x => x.RoomID,
                        principalTable: "Room",
                        principalColumn: "RoomID");
                    table.ForeignKey(
                        name: "FK__Maintenan__Stude__59C55456",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "StudentID");
                });

            migrationBuilder.CreateTable(
                name: "Allocation",
                columns: table => new
                {
                    AllocationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationID = table.Column<int>(type: "int", nullable: false),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    RoomID = table.Column<int>(type: "int", nullable: false),
                    BedNumber = table.Column<int>(type: "int", nullable: false),
                    AcademicYear = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Semester = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    FromDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ToDate = table.Column<DateOnly>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    AllocatedBy = table.Column<int>(type: "int", nullable: true),
                    MealPlanAllocated = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysdatetime())"),
                    EndedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Allocati__B3C6D6ABF8A35DA6", x => x.AllocationID);
                    table.ForeignKey(
                        name: "FK__Allocatio__Alloc__25518C17",
                        column: x => x.AllocatedBy,
                        principalTable: "Admin",
                        principalColumn: "AdminID");
                    table.ForeignKey(
                        name: "FK__Allocatio__Appli__22751F6C",
                        column: x => x.ApplicationID,
                        principalTable: "Application",
                        principalColumn: "ApplicationID");
                    table.ForeignKey(
                        name: "FK__Allocatio__RoomI__245D67DE",
                        column: x => x.RoomID,
                        principalTable: "Room",
                        principalColumn: "RoomID");
                    table.ForeignKey(
                        name: "FK__Allocatio__Stude__236943A5",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "StudentID");
                });

            migrationBuilder.CreateTable(
                name: "Document",
                columns: table => new
                {
                    DocumentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationID = table.Column<int>(type: "int", nullable: true),
                    StudentID = table.Column<int>(type: "int", nullable: true),
                    DocumentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VerifiedBy = table.Column<int>(type: "int", nullable: true),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysdatetime())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Document__1ABEEF6F35C9E87F", x => x.DocumentID);
                    table.ForeignKey(
                        name: "FK__Document__Applic__607251E5",
                        column: x => x.ApplicationID,
                        principalTable: "Application",
                        principalColumn: "ApplicationID");
                    table.ForeignKey(
                        name: "FK__Document__Studen__6166761E",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "StudentID");
                    table.ForeignKey(
                        name: "FK__Document__Verifi__625A9A57",
                        column: x => x.VerifiedBy,
                        principalTable: "Admin",
                        principalColumn: "AdminID");
                });

            migrationBuilder.CreateTable(
                name: "Absence",
                columns: table => new
                {
                    AbsenceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    AllocationID = table.Column<int>(type: "int", nullable: true),
                    AbsenceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FromDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ToDate = table.Column<DateOnly>(type: "date", nullable: false),
                    DaysCount = table.Column<int>(type: "int", nullable: true, computedColumnSql: "(datediff(day,[FromDate],[ToDate])+(1))", stored: true),
                    IsWeekendIncluded = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysdatetime())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Absence__3A074E471A03DB31", x => x.AbsenceID);
                    table.ForeignKey(
                        name: "FK__Absence__Allocat__498EEC8D",
                        column: x => x.AllocationID,
                        principalTable: "Allocation",
                        principalColumn: "AllocationID");
                    table.ForeignKey(
                        name: "FK__Absence__Approve__4A8310C6",
                        column: x => x.ApprovedBy,
                        principalTable: "Admin",
                        principalColumn: "AdminID");
                    table.ForeignKey(
                        name: "FK__Absence__Student__489AC854",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "StudentID");
                });

            migrationBuilder.CreateTable(
                name: "EvictionNotice",
                columns: table => new
                {
                    EvictionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<int>(type: "int", nullable: true),
                    AllocationID = table.Column<int>(type: "int", nullable: true),
                    NoticeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysdatetime())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Eviction__A889DE2E7895F278", x => x.EvictionID);
                    table.ForeignKey(
                        name: "FK__EvictionN__Alloc__367C1819",
                        column: x => x.AllocationID,
                        principalTable: "Allocation",
                        principalColumn: "AllocationID");
                    table.ForeignKey(
                        name: "FK__EvictionN__Creat__37703C52",
                        column: x => x.CreatedBy,
                        principalTable: "Admin",
                        principalColumn: "AdminID");
                    table.ForeignKey(
                        name: "FK__EvictionN__Stude__3587F3E0",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "StudentID");
                });

            migrationBuilder.CreateTable(
                name: "Meal",
                columns: table => new
                {
                    MealID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<int>(type: "int", nullable: true),
                    AllocationID = table.Column<int>(type: "int", nullable: true),
                    MealDate = table.Column<DateOnly>(type: "date", nullable: true),
                    MealType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsBooked = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TakenAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MissedPenalty = table.Column<decimal>(type: "decimal(10,2)", nullable: true, defaultValue: 0m),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysdatetime())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Meal__ACF6A65D83EC72C7", x => x.MealID);
                    table.ForeignKey(
                        name: "FK__Meal__Allocation__41EDCAC5",
                        column: x => x.AllocationID,
                        principalTable: "Allocation",
                        principalColumn: "AllocationID");
                    table.ForeignKey(
                        name: "FK__Meal__StudentID__40F9A68C",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "StudentID");
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    PaymentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<int>(type: "int", nullable: true),
                    AllocationID = table.Column<int>(type: "int", nullable: true),
                    AcademicYear = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Semester = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PaymentCategory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    PaymentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReceiptNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReceivedBy = table.Column<int>(type: "int", nullable: true),
                    IsOverdue = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FirstWarningSentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EvictionNoticeIssuedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(sysdatetime())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Payment__9B556A58940BC47E", x => x.PaymentID);
                    table.ForeignKey(
                        name: "FK__Payment__Allocat__2EDAF651",
                        column: x => x.AllocationID,
                        principalTable: "Allocation",
                        principalColumn: "AllocationID");
                    table.ForeignKey(
                        name: "FK__Payment__Receive__2FCF1A8A",
                        column: x => x.ReceivedBy,
                        principalTable: "Admin",
                        principalColumn: "AdminID");
                    table.ForeignKey(
                        name: "FK__Payment__Student__2DE6D218",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "StudentID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Absence_AllocationID",
                table: "Absence",
                column: "AllocationID");

            migrationBuilder.CreateIndex(
                name: "IX_Absence_ApprovedBy",
                table: "Absence",
                column: "ApprovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Absence_StudentID",
                table: "Absence",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_Admin_DormitoryID",
                table: "Admin",
                column: "DormitoryID");

            migrationBuilder.CreateIndex(
                name: "UQ__Admin__536C85E456A1733F",
                table: "Admin",
                column: "Username",
                unique: true,
                filter: "[Username] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IDX_Allocation_RoomID",
                table: "Allocation",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_Allocation_AllocatedBy",
                table: "Allocation",
                column: "AllocatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Allocation_ApplicationID",
                table: "Allocation",
                column: "ApplicationID");

            migrationBuilder.CreateIndex(
                name: "UQ_ActiveAllocation",
                table: "Allocation",
                column: "StudentID",
                unique: true,
                filter: "([IsActive]=(1) AND [IsDeleted]=(0))");

            migrationBuilder.CreateIndex(
                name: "UQ_BedAllocation",
                table: "Allocation",
                columns: new[] { "RoomID", "BedNumber" },
                unique: true,
                filter: "([IsActive]=(1) AND [IsDeleted]=(0))");

            migrationBuilder.CreateIndex(
                name: "IX_Announcement_CreatedBy",
                table: "Announcement",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Announcement_DormitoryID",
                table: "Announcement",
                column: "DormitoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Announcement_UniversityID",
                table: "Announcement",
                column: "UniversityID");

            migrationBuilder.CreateIndex(
                name: "IX_AnnouncementAttachment_AnnouncementID",
                table: "AnnouncementAttachment",
                column: "AnnouncementID");

            migrationBuilder.CreateIndex(
                name: "IDX_Application_Preference",
                table: "Application",
                column: "HousingPreference");

            migrationBuilder.CreateIndex(
                name: "IDX_Application_Status",
                table: "Application",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Application_PreferredBuildingID",
                table: "Application",
                column: "PreferredBuildingID");

            migrationBuilder.CreateIndex(
                name: "IX_Application_PreferredRoomID",
                table: "Application",
                column: "PreferredRoomID");

            migrationBuilder.CreateIndex(
                name: "IX_Application_ReviewedBy",
                table: "Application",
                column: "ReviewedBy");

            migrationBuilder.CreateIndex(
                name: "UQ_App_Student_Year",
                table: "Application",
                columns: new[] { "StudentID", "AcademicYear", "Semester" },
                unique: true,
                filter: "([IsDeleted]=(0))");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationSchedule_DormitoryID",
                table: "ApplicationSchedule",
                column: "DormitoryID");

            migrationBuilder.CreateIndex(
                name: "IDX_Building_Type",
                table: "Building",
                column: "BuildingType");

            migrationBuilder.CreateIndex(
                name: "IX_Building_DormitoryID",
                table: "Building",
                column: "DormitoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Document_ApplicationID",
                table: "Document",
                column: "ApplicationID");

            migrationBuilder.CreateIndex(
                name: "IX_Document_StudentID",
                table: "Document",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_Document_VerifiedBy",
                table: "Document",
                column: "VerifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Dormitory_UniversityID",
                table: "Dormitory",
                column: "UniversityID");

            migrationBuilder.CreateIndex(
                name: "IX_EvictionNotice_AllocationID",
                table: "EvictionNotice",
                column: "AllocationID");

            migrationBuilder.CreateIndex(
                name: "IX_EvictionNotice_CreatedBy",
                table: "EvictionNotice",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EvictionNotice_StudentID",
                table: "EvictionNotice",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_Guardian_StudentID",
                table: "Guardian",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequest_AssignedTo",
                table: "MaintenanceRequest",
                column: "AssignedTo");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequest_RoomID",
                table: "MaintenanceRequest",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequest_StudentID",
                table: "MaintenanceRequest",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IDX_Meal_Date",
                table: "Meal",
                column: "MealDate");

            migrationBuilder.CreateIndex(
                name: "IDX_Meal_StudentID",
                table: "Meal",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_Meal_AllocationID",
                table: "Meal",
                column: "AllocationID");

            migrationBuilder.CreateIndex(
                name: "UQ__Meal__3449321F74EFBA23",
                table: "Meal",
                columns: new[] { "StudentID", "MealDate", "MealType" },
                unique: true,
                filter: "[StudentID] IS NOT NULL AND [MealDate] IS NOT NULL AND [MealType] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IDX_Payment_Category",
                table: "Payment",
                column: "PaymentCategory");

            migrationBuilder.CreateIndex(
                name: "IDX_Payment_StudentID",
                table: "Payment",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_AllocationID",
                table: "Payment",
                column: "AllocationID");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_ReceivedBy",
                table: "Payment",
                column: "ReceivedBy");

            migrationBuilder.CreateIndex(
                name: "UQ__Payment__C08AFDAB02FB820D",
                table: "Payment",
                column: "ReceiptNumber",
                unique: true,
                filter: "[ReceiptNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IDX_Room_Occupancy",
                table: "Room",
                column: "CurrentOccupancy");

            migrationBuilder.CreateIndex(
                name: "IDX_Room_Type",
                table: "Room",
                column: "RoomType");

            migrationBuilder.CreateIndex(
                name: "IX_Room_BuildingID",
                table: "Room",
                column: "BuildingID");

            migrationBuilder.CreateIndex(
                name: "IDX_Student_Email",
                table: "Student",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IDX_Student_NationalID",
                table: "Student",
                column: "NationalID");

            migrationBuilder.CreateIndex(
                name: "IDX_Student_Nationality",
                table: "Student",
                column: "Nationality");

            migrationBuilder.CreateIndex(
                name: "IDX_Student_Status",
                table: "Student",
                column: "StudentStatus");

            migrationBuilder.CreateIndex(
                name: "UQ__Student__E9AA321A798CF797",
                table: "Student",
                column: "NationalID",
                unique: true,
                filter: "[NationalID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_StudentDownloadLog_StudentID",
                table: "StudentDownloadLog",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "UQ__StudentL__32C52A78F048A48E",
                table: "StudentLogin",
                column: "StudentID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__StudentL__E9AA321A22477898",
                table: "StudentLogin",
                column: "NationalID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IDX_Violation_StudentID",
                table: "Violation",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_Violation_RecordedBy",
                table: "Violation",
                column: "RecordedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Absence");

            migrationBuilder.DropTable(
                name: "AnnouncementAttachment");

            migrationBuilder.DropTable(
                name: "ApplicationSchedule");

            migrationBuilder.DropTable(
                name: "Document");

            migrationBuilder.DropTable(
                name: "EvictionNotice");

            migrationBuilder.DropTable(
                name: "Guardian");

            migrationBuilder.DropTable(
                name: "MaintenanceRequest");

            migrationBuilder.DropTable(
                name: "Meal");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "StudentDownloadLog");

            migrationBuilder.DropTable(
                name: "StudentLogin");

            migrationBuilder.DropTable(
                name: "Violation");

            migrationBuilder.DropTable(
                name: "Announcement");

            migrationBuilder.DropTable(
                name: "Allocation");

            migrationBuilder.DropTable(
                name: "Application");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.DropTable(
                name: "Admin");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "Building");

            migrationBuilder.DropTable(
                name: "Dormitory");

            migrationBuilder.DropTable(
                name: "University");
        }
    }
}
