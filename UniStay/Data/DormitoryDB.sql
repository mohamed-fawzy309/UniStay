-- =============================================
-- University Dormitory Management System
-- Enhanced Version - Assiut University
-- Updated: 2026-02-24
-- =============================================

USE master;
GO

IF EXISTS (SELECT * FROM sys.databases WHERE name = 'DormitoryDB')
BEGIN
    ALTER DATABASE DormitoryDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE DormitoryDB;
END
GO

CREATE DATABASE DormitoryDB;
GO
USE DormitoryDB;
GO

-- =============================================
-- 1. UNIVERSITY TABLE
-- =============================================
CREATE TABLE University(
    UniversityID       INT IDENTITY PRIMARY KEY,
    UniversityName     NVARCHAR(200) NOT NULL,
    City               NVARCHAR(100),
    Governorate        NVARCHAR(100),
    CreatedAt          DATETIME2 DEFAULT SYSDATETIME(),
    IsDeleted          BIT DEFAULT 0
);
GO

-- =============================================
-- 2. DORMITORY TABLE
-- =============================================
CREATE TABLE Dormitory(
    DormitoryID        INT IDENTITY PRIMARY KEY,
    DormitoryName      NVARCHAR(200) NOT NULL,
    Type               NVARCHAR(20) CHECK(Type IN ('Male','Female')),
    Address            NVARCHAR(300),
    UniversityID       INT NOT NULL,
    CreatedAt          DATETIME2 DEFAULT SYSDATETIME(),
    IsDeleted          BIT DEFAULT 0,
    FOREIGN KEY(UniversityID) REFERENCES University(UniversityID)
);
GO

-- =============================================
-- 3. BUILDING TABLE
-- =============================================
CREATE TABLE Building(
    BuildingID         INT IDENTITY PRIMARY KEY,
    BuildingName       NVARCHAR(100) NOT NULL,
    BuildingType       NVARCHAR(20) NOT NULL CHECK(BuildingType IN ('Standard','Premium')),
    FloorsCount        INT CHECK(FloorsCount > 0),
    TotalRooms         INT DEFAULT 150,
    DormitoryID        INT NOT NULL,
    CreatedAt          DATETIME2 DEFAULT SYSDATETIME(),
    IsDeleted          BIT DEFAULT 0,
    FOREIGN KEY(DormitoryID) REFERENCES Dormitory(DormitoryID)
);
GO

CREATE INDEX IDX_Building_Type ON Building(BuildingType);
GO

-- =============================================
-- 4. ROOM TABLE
-- =============================================
CREATE TABLE Room(
    RoomID             INT IDENTITY PRIMARY KEY,
    RoomNumber         NVARCHAR(20) NOT NULL,
    Floor              INT,
    RoomType           NVARCHAR(20) NOT NULL CHECK(RoomType IN ('Standard','Premium')),
    BedsCount          INT CHECK(BedsCount IN (2, 3, 6)),
    CurrentOccupancy   INT DEFAULT 0 CHECK(CurrentOccupancy >= 0),
    BuildingID         INT NOT NULL,
    IsActive           BIT DEFAULT 1,
    HasBalcony         BIT DEFAULT 0,
    HasPrivateBathroom BIT DEFAULT 0,
    HasAC              BIT DEFAULT 0,
    HasFridge          BIT DEFAULT 0,
    FloorMaterial      NVARCHAR(50),
    CreatedAt          DATETIME2 DEFAULT SYSDATETIME(),
    IsDeleted          BIT DEFAULT 0,
    FOREIGN KEY(BuildingID) REFERENCES Building(BuildingID),
    CONSTRAINT CHK_Room_Occupancy CHECK(CurrentOccupancy <= BedsCount),
    CONSTRAINT CHK_Room_Type_Beds CHECK(
        (RoomType = 'Premium' AND BedsCount = 2) OR
        (RoomType = 'Standard' AND BedsCount IN (3, 6))
    )
);
GO

CREATE INDEX IDX_Room_Type      ON Room(RoomType);
CREATE INDEX IDX_Room_Occupancy ON Room(CurrentOccupancy);
GO

-- =============================================
-- 5. STUDENT TABLE
-- =============================================
CREATE TABLE Student(
    StudentID             INT IDENTITY PRIMARY KEY,

    -- بيانات التسجيل (لكل الطلاب)
    StudentCode           NVARCHAR(50)  NULL,
    StudentStatus         NVARCHAR(20)  NOT NULL DEFAULT 'New'
                          CHECK(StudentStatus IN ('New','Returning')),
    Nationality           NVARCHAR(20)  NOT NULL DEFAULT 'Egyptian'
                          CHECK(Nationality IN ('Egyptian','Foreign')),

    -- بيانات شخصية (لكل الطلاب)
    Name                  NVARCHAR(200) NULL,
    FullNameArabic        NVARCHAR(200) NULL,
    Gender                NVARCHAR(10)  NULL CHECK(Gender IN ('Male','Female')),
    BirthDate             DATE          NULL,
    BirthPlace            NVARCHAR(200) NULL,
    Religion              NVARCHAR(50)  NULL,

    -- عنوان الإقامة (لكل الطلاب)
    Residence             NVARCHAR(200) NULL,
    ResidenceCountry      NVARCHAR(100) DEFAULT N'مصر',
    ResidenceGovernorate  NVARCHAR(100) NULL,
    ResidenceCity         NVARCHAR(100) NULL,
    Address               NVARCHAR(300) NULL,
    DistanceFromHome      FLOAT         NULL,

    -- بيانات الاتصال (لكل الطلاب)
    Email                 NVARCHAR(200) NULL,
    Phone                 NVARCHAR(20)  NULL,
    Mobile                NVARCHAR(20)  NULL,

    -- البيانات الأكاديمية (لكل الطلاب)
    Faculty               NVARCHAR(200) NULL,
    Department            NVARCHAR(200) NULL,
    StudyType             NVARCHAR(50)  NULL,
    Grade                 NVARCHAR(20)  NULL,
    GradePercentage       FLOAT         NULL CHECK(GradePercentage BETWEEN 0 AND 100),

    -- تفضيل السكن (لكل الطلاب)
    HousingType           NVARCHAR(20)  NULL
                          CHECK(HousingType IN ('Standard','Premium')),

    -- بيانات المصري فقط
    NationalID            NVARCHAR(20)  UNIQUE NULL,
    FatherName            NVARCHAR(200) NULL,
    FatherNationalID      NVARCHAR(14)  NULL,
    FatherJob             NVARCHAR(100) NULL,
    FatherPhone           NVARCHAR(20)  NULL,
    ParentStatus          NVARCHAR(50)  NULL
                          CHECK(ParentStatus IN ('BothAlive','FatherOnly','MotherOnly','Orphan','Divorced')),
    FamilyAbroad          BIT           DEFAULT 0,
    SpecialNeeds          BIT           DEFAULT 0,

    -- بيانات الوافد فقط
    PassportNumber        NVARCHAR(50)  NULL,
    PassportIssuePlace    NVARCHAR(100) NULL,

    -- بيانات المستجد فقط
    HighSchoolDivision    NVARCHAR(100) NULL,
    HighSchoolPercentage  FLOAT         NULL,
    HighSchoolTotal       FLOAT         NULL,
    HighSchoolFromAbroad  BIT           DEFAULT 0,

    -- بيانات القديم فقط
    LastYearGrade         NVARCHAR(50)  NULL,
    LastYearPercentage    DECIMAL(5,2)  NULL,
    PreviousHousingYears  NVARCHAR(MAX) NULL,

    -- بيانات طبية
    HasMedicalCondition   BIT           DEFAULT 0,
    DrugTestPassed        BIT           NULL,

    -- بيانات النظام
    Notes                 NVARCHAR(MAX) NULL,
    ProfilePhoto          NVARCHAR(MAX) NULL,
    CreatedAt             DATETIME2     DEFAULT SYSDATETIME(),
    IsDeleted             BIT           DEFAULT 0
);
GO

CREATE INDEX IDX_Student_NationalID  ON Student(NationalID);
CREATE INDEX IDX_Student_Email       ON Student(Email);
CREATE INDEX IDX_Student_Status      ON Student(StudentStatus);
CREATE INDEX IDX_Student_Nationality ON Student(Nationality);
GO

-- =============================================
-- 6. GUARDIAN TABLE
-- =============================================
CREATE TABLE Guardian(
    GuardianID       INT IDENTITY PRIMARY KEY,
    StudentID        INT NOT NULL,
    GuardianRole     NVARCHAR(20) CHECK(GuardianRole IN ('Father','Mother','Other')),
    Name             NVARCHAR(200) NULL,
    NationalID       NVARCHAR(20)  NULL,
    Relation         NVARCHAR(50)  NULL,
    Phone            NVARCHAR(20)  NULL,
    AlternatePhone   NVARCHAR(20)  NULL,
    Job              NVARCHAR(100) NULL,
    ResidenceAddress NVARCHAR(300) NULL,
    CreatedAt        DATETIME2 DEFAULT SYSDATETIME(),
    IsDeleted        BIT DEFAULT 0,
    FOREIGN KEY(StudentID) REFERENCES Student(StudentID)
);
GO

-- =============================================
-- 7. ADMIN TABLE
-- =============================================
CREATE TABLE Admin(
    AdminID      INT IDENTITY PRIMARY KEY,
    Name         NVARCHAR(200),
    Role         NVARCHAR(50) CHECK(Role IN ('SuperAdmin','DormManager','ApplicationReviewer','DataEntry')),
    Username     NVARCHAR(100) UNIQUE,
    PasswordHash VARBINARY(512),
    Email        NVARCHAR(200),
    Phone        NVARCHAR(20),
    DormitoryID  INT NULL,
    IsActive     BIT DEFAULT 1,
    CreatedAt    DATETIME2 DEFAULT SYSDATETIME(),
    IsDeleted    BIT DEFAULT 0,
    FOREIGN KEY(DormitoryID) REFERENCES Dormitory(DormitoryID)
);
GO

-- =============================================
-- 8. APPLICATION SCHEDULE TABLE
-- =============================================
CREATE TABLE ApplicationSchedule(
    ScheduleID      INT IDENTITY PRIMARY KEY,
    AcademicYear    NVARCHAR(20) NOT NULL,
    StudentCategory NVARCHAR(50) NOT NULL
                    CHECK(StudentCategory IN (
                        'MaleNew','MaleReturning',
                        'FemaleNew','FemaleReturning'
                    )),
    FromDate        DATE NOT NULL,
    ToDate          DATE NOT NULL,
    DormitoryID     INT NULL,
    IsActive        BIT DEFAULT 1,
    CreatedAt       DATETIME2 DEFAULT SYSDATETIME(),
    IsDeleted       BIT DEFAULT 0,
    FOREIGN KEY(DormitoryID) REFERENCES Dormitory(DormitoryID),
    CONSTRAINT CHK_Schedule_Dates CHECK(ToDate >= FromDate)
);
GO

-- =============================================
-- 9. APPLICATION TABLE
-- =============================================
CREATE TABLE Application(
    ApplicationID       INT IDENTITY PRIMARY KEY,
    StudentID           INT NOT NULL,
    AcademicYear        NVARCHAR(20),
    Semester            NVARCHAR(20),
    HousingPreference   NVARCHAR(20) NOT NULL
                        CHECK(HousingPreference IN ('Standard','Premium')),
    PreferredBuildingID INT NULL,
    PreferredRoomID     INT NULL,
    PreferredBedNumber  INT NULL,
    MealPlanType        NVARCHAR(50),
    WithoutNutrition    BIT DEFAULT 0,
    FamilyAbroad        BIT DEFAULT 0,
    SpecialNeeds        BIT DEFAULT 0,
    ApplicationDate     DATETIME2 DEFAULT SYSDATETIME(),
    Status              NVARCHAR(20) DEFAULT 'Pending'
                        CHECK(Status IN ('Pending','UnderReview','Accepted','Rejected','Cancelled')),
    Priority            INT,
    RequiredDocuments   NVARCHAR(MAX),
    RejectionReason     NVARCHAR(MAX),
    ReviewedBy          INT,
    ReviewedAt          DATETIME2,
    CreatedAt           DATETIME2 DEFAULT SYSDATETIME(),
    UpdatedAt           DATETIME2,
    IsDeleted           BIT DEFAULT 0,
    FOREIGN KEY(StudentID)           REFERENCES Student(StudentID),
    FOREIGN KEY(ReviewedBy)          REFERENCES Admin(AdminID),
    FOREIGN KEY(PreferredBuildingID) REFERENCES Building(BuildingID),
    FOREIGN KEY(PreferredRoomID)     REFERENCES Room(RoomID)
);
GO

CREATE UNIQUE INDEX UQ_App_Student_Year
    ON Application(StudentID, AcademicYear, Semester)
    WHERE IsDeleted = 0;

CREATE INDEX IDX_Application_Status     ON Application(Status);
CREATE INDEX IDX_Application_Preference ON Application(HousingPreference);
GO

-- =============================================
-- 10. ALLOCATION TABLE
-- =============================================
CREATE TABLE Allocation(
    AllocationID      INT IDENTITY PRIMARY KEY,
    ApplicationID     INT NOT NULL,
    StudentID         INT NOT NULL,
    RoomID            INT NOT NULL,
    BedNumber         INT NOT NULL,
    AcademicYear      NVARCHAR(20),
    Semester          NVARCHAR(20),
    FromDate          DATE,
    ToDate            DATE,
    IsActive          BIT DEFAULT 1,
    AllocatedBy       INT,
    MealPlanAllocated NVARCHAR(50),
    CreatedAt         DATETIME2 DEFAULT SYSDATETIME(),
    EndedAt           DATETIME2,
    IsDeleted         BIT DEFAULT 0,
    FOREIGN KEY(ApplicationID) REFERENCES Application(ApplicationID),
    FOREIGN KEY(StudentID)     REFERENCES Student(StudentID),
    FOREIGN KEY(RoomID)        REFERENCES Room(RoomID),
    FOREIGN KEY(AllocatedBy)   REFERENCES Admin(AdminID),
    CONSTRAINT CHK_AllocationDates CHECK(ToDate >= FromDate)
);
GO

CREATE UNIQUE INDEX UQ_ActiveAllocation
    ON Allocation(StudentID)
    WHERE IsActive = 1 AND IsDeleted = 0;

CREATE UNIQUE INDEX UQ_BedAllocation
    ON Allocation(RoomID, BedNumber)
    WHERE IsActive = 1 AND IsDeleted = 0;

CREATE INDEX IDX_Allocation_RoomID ON Allocation(RoomID);
GO

-- =============================================
-- 11. PAYMENT TABLE
-- =============================================
CREATE TABLE Payment(
    PaymentID              INT IDENTITY PRIMARY KEY,
    StudentID              INT,
    AllocationID           INT,
    AcademicYear           NVARCHAR(20),
    Semester               NVARCHAR(20),
    PaymentCategory        NVARCHAR(50) NOT NULL
                           CHECK(PaymentCategory IN (
                               'HousingFee','MealFee','MealRefund',
                               'Discount','Correspondence','Fine','Other'
                           )),
    Amount                 DECIMAL(10, 2),
    PaymentType            NVARCHAR(50),
    PaymentMethod          NVARCHAR(50),
    PaymentDate            DATETIME2,
    ReceiptNumber          NVARCHAR(100) UNIQUE,
    ReceivedBy             INT,
    IsOverdue              BIT DEFAULT 0,
    DueDate                DATETIME2 NULL,
    FirstWarningSentAt     DATETIME2 NULL,
    EvictionNoticeIssuedAt DATETIME2 NULL,
    CreatedAt              DATETIME2 DEFAULT SYSDATETIME(),
    IsDeleted              BIT DEFAULT 0,
    FOREIGN KEY(StudentID)    REFERENCES Student(StudentID),
    FOREIGN KEY(AllocationID) REFERENCES Allocation(AllocationID),
    FOREIGN KEY(ReceivedBy)   REFERENCES Admin(AdminID)
);
GO

CREATE INDEX IDX_Payment_StudentID ON Payment(StudentID);
CREATE INDEX IDX_Payment_Category  ON Payment(PaymentCategory);
GO

-- =============================================
-- 12. EVICTION NOTICE TABLE
-- =============================================
CREATE TABLE EvictionNotice(
    EvictionID   INT IDENTITY PRIMARY KEY,
    StudentID    INT,
    AllocationID INT,
    NoticeDate   DATETIME2,
    DueDate      DATETIME2,
    Reason       NVARCHAR(MAX),
    Status       NVARCHAR(20) CHECK(Status IN ('Pending','Executed','Cancelled')),
    CreatedBy    INT,
    CreatedAt    DATETIME2 DEFAULT SYSDATETIME(),
    IsDeleted    BIT DEFAULT 0,
    FOREIGN KEY(StudentID)    REFERENCES Student(StudentID),
    FOREIGN KEY(AllocationID) REFERENCES Allocation(AllocationID),
    FOREIGN KEY(CreatedBy)    REFERENCES Admin(AdminID)
);
GO

-- =============================================
-- 13. MEAL TABLE
-- =============================================
CREATE TABLE Meal(
    MealID        INT IDENTITY PRIMARY KEY,
    StudentID     INT,
    AllocationID  INT NULL,
    MealDate      DATE,
    MealType      NVARCHAR(20)
                  CHECK(MealType IN ('Breakfast','Lunch','Dinner','Daily','MuslimMeal')),
    IsBooked      BIT DEFAULT 0,
    Status        NVARCHAR(20)
                  CHECK(Status IN ('Taken','NotTaken','Cancelled')),
    TakenAt       DATETIME2,
    MissedPenalty DECIMAL(10, 2) DEFAULT 0,
    Notes         NVARCHAR(MAX),
    CreatedAt     DATETIME2 DEFAULT SYSDATETIME(),
    IsDeleted     BIT DEFAULT 0,
    FOREIGN KEY(StudentID)    REFERENCES Student(StudentID),
    FOREIGN KEY(AllocationID) REFERENCES Allocation(AllocationID),
    UNIQUE(StudentID, MealDate, MealType)
);
GO

CREATE INDEX IDX_Meal_Date      ON Meal(MealDate);
CREATE INDEX IDX_Meal_StudentID ON Meal(StudentID);
GO

-- =============================================
-- 14. ABSENCE TABLE
-- =============================================
CREATE TABLE Absence(
    AbsenceID         INT IDENTITY PRIMARY KEY,
    StudentID         INT NOT NULL,
    AllocationID      INT NULL,
    AbsenceType       NVARCHAR(50)
                      CHECK(AbsenceType IN ('Permission','Absence')),
    FromDate          DATE NOT NULL,
    ToDate            DATE NOT NULL,
    DaysCount         AS DATEDIFF(DAY, FromDate, ToDate) + 1 PERSISTED,
    IsWeekendIncluded BIT DEFAULT 1,
    Reason            NVARCHAR(MAX),
    ApprovedBy        INT NULL,
    CreatedAt         DATETIME2 DEFAULT SYSDATETIME(),
    IsDeleted         BIT DEFAULT 0,
    FOREIGN KEY(StudentID)    REFERENCES Student(StudentID),
    FOREIGN KEY(AllocationID) REFERENCES Allocation(AllocationID),
    FOREIGN KEY(ApprovedBy)   REFERENCES Admin(AdminID),
    CONSTRAINT CHK_Absence_Dates CHECK(ToDate >= FromDate)
);
GO

-- =============================================
-- 15. VIOLATION TABLE
-- =============================================
CREATE TABLE Violation(
    ViolationID   INT IDENTITY PRIMARY KEY,
    StudentID     INT,
    ViolationType NVARCHAR(100),
    Description   NVARCHAR(MAX),
    ViolationDate DATE,
    Penalty       NVARCHAR(50),
    PenaltyAmount DECIMAL(10, 2),
    IsPaid        BIT DEFAULT 0,
    RecordedBy    INT,
    CreatedAt     DATETIME2 DEFAULT SYSDATETIME(),
    IsDeleted     BIT DEFAULT 0,
    FOREIGN KEY(StudentID)  REFERENCES Student(StudentID),
    FOREIGN KEY(RecordedBy) REFERENCES Admin(AdminID)
);
GO

CREATE INDEX IDX_Violation_StudentID ON Violation(StudentID);
GO

-- =============================================
-- 16. MAINTENANCE REQUEST TABLE
-- =============================================
CREATE TABLE MaintenanceRequest(
    RequestID     INT IDENTITY PRIMARY KEY,
    RoomID        INT,
    StudentID     INT,
    IssueType     NVARCHAR(100),
    Description   NVARCHAR(MAX),
    Priority      NVARCHAR(20) CHECK(Priority IN ('Low','Medium','High','Urgent')),
    Status        NVARCHAR(20) CHECK(Status IN ('Pending','InProgress','Completed','Cancelled')),
    RequestDate   DATETIME2,
    CompletedDate DATETIME2 NULL,
    AssignedTo    INT,
    Notes         NVARCHAR(MAX),
    CreatedAt     DATETIME2 DEFAULT SYSDATETIME(),
    UpdatedAt     DATETIME2,
    IsDeleted     BIT DEFAULT 0,
    FOREIGN KEY(RoomID)     REFERENCES Room(RoomID),
    FOREIGN KEY(StudentID)  REFERENCES Student(StudentID),
    FOREIGN KEY(AssignedTo) REFERENCES Admin(AdminID)
);
GO

-- =============================================
-- 17. DOCUMENT TABLE
-- =============================================
CREATE TABLE Document(
    DocumentID    INT IDENTITY PRIMARY KEY,
    ApplicationID INT,
    StudentID     INT NULL,
    DocumentType  NVARCHAR(100),
    FileName      NVARCHAR(300),
    FilePath      NVARCHAR(MAX),
    UploadedAt    DATETIME2,
    VerifiedBy    INT,
    VerifiedAt    DATETIME2,
    IsVerified    BIT DEFAULT 0,
    CreatedAt     DATETIME2 DEFAULT SYSDATETIME(),
    IsDeleted     BIT DEFAULT 0,
    FOREIGN KEY(ApplicationID) REFERENCES Application(ApplicationID),
    FOREIGN KEY(StudentID)     REFERENCES Student(StudentID),
    FOREIGN KEY(VerifiedBy)    REFERENCES Admin(AdminID)
);
GO

-- =============================================
-- 18. ANNOUNCEMENT TABLE
-- =============================================
CREATE TABLE Announcement(
    AnnouncementID INT IDENTITY PRIMARY KEY,
    Title          NVARCHAR(500) NOT NULL,
    Content        NVARCHAR(MAX),
    AcademicYear   NVARCHAR(20),
    UniversityID   INT NULL,
    DormitoryID    INT NULL,
    IsPublic       BIT DEFAULT 1,
    PublishedAt    DATETIME2 DEFAULT SYSDATETIME(),
    CreatedBy      INT,
    CreatedAt      DATETIME2 DEFAULT SYSDATETIME(),
    IsDeleted      BIT DEFAULT 0,
    FOREIGN KEY(UniversityID) REFERENCES University(UniversityID),
    FOREIGN KEY(DormitoryID)  REFERENCES Dormitory(DormitoryID),
    FOREIGN KEY(CreatedBy)    REFERENCES Admin(AdminID)
);
GO

-- =============================================
-- 19. ANNOUNCEMENT ATTACHMENT TABLE
-- =============================================
CREATE TABLE AnnouncementAttachment(
    AttachmentID   INT IDENTITY PRIMARY KEY,
    AnnouncementID INT NOT NULL,
    FileName       NVARCHAR(300),
    FilePath       NVARCHAR(MAX),
    FileType       NVARCHAR(20),
    CreatedAt      DATETIME2 DEFAULT SYSDATETIME(),
    IsDeleted      BIT DEFAULT 0,
    FOREIGN KEY(AnnouncementID) REFERENCES Announcement(AnnouncementID)
);
GO

-- =============================================
-- 20. STUDENT LOGIN TABLE
-- =============================================
CREATE TABLE StudentLogin(
    LoginID      INT IDENTITY PRIMARY KEY,
    StudentID    INT NOT NULL UNIQUE,
    NationalID   NVARCHAR(20) NOT NULL UNIQUE,
    PasswordHash VARBINARY(512),
    LastLoginAt  DATETIME2,
    IsActive     BIT DEFAULT 1,
    CreatedAt    DATETIME2 DEFAULT SYSDATETIME(),
    IsDeleted    BIT DEFAULT 0,
    FOREIGN KEY(StudentID) REFERENCES Student(StudentID)
);
GO

-- =============================================
-- 21. STUDENT DOWNLOAD LOG TABLE
-- =============================================
CREATE TABLE StudentDownloadLog(
    LogID        INT IDENTITY PRIMARY KEY,
    StudentID    INT NOT NULL,
    FileName     NVARCHAR(300),
    DownloadedAt DATETIME2 DEFAULT SYSDATETIME(),
    FOREIGN KEY(StudentID) REFERENCES Student(StudentID)
);
GO

-- =============================================
-- STORED PROCEDURE: إنشاء غرف تلقائياً
-- =============================================
CREATE PROCEDURE SP_GenerateRooms
    @BuildingID INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @BuildingType  NVARCHAR(20);
    DECLARE @FloorsCount   INT;
    DECLARE @RoomsPerFloor INT = 30;
    DECLARE @BedsCount     INT;

    SELECT @BuildingType = BuildingType, @FloorsCount = FloorsCount
    FROM Building WHERE BuildingID = @BuildingID AND IsDeleted = 0;

    IF @BuildingType IS NULL
    BEGIN
        RAISERROR('Building not found.', 16, 1);
        RETURN;
    END

    SET @BedsCount = CASE WHEN @BuildingType = 'Premium' THEN 2 ELSE 3 END;

    DECLARE @Floor   INT = 1;
    DECLARE @RoomNum INT;

    WHILE @Floor <= @FloorsCount
    BEGIN
        SET @RoomNum = 1;
        WHILE @RoomNum <= @RoomsPerFloor
        BEGIN
            INSERT INTO Room(
                RoomNumber, Floor, RoomType, BedsCount, BuildingID,
                HasBalcony, HasPrivateBathroom, HasAC, HasFridge
            )
            VALUES (
                CAST(@Floor AS NVARCHAR) + '-' + RIGHT('00' + CAST(@RoomNum AS NVARCHAR), 2),
                @Floor,
                @BuildingType,
                @BedsCount,
                @BuildingID,
                CASE WHEN @BuildingType = 'Premium' THEN 1 ELSE 0 END,
                CASE WHEN @BuildingType = 'Premium' THEN 1 ELSE 0 END,
                CASE WHEN @BuildingType = 'Premium' THEN 1 ELSE 0 END,
                CASE WHEN @BuildingType = 'Premium' THEN 1 ELSE 0 END
            );
            SET @RoomNum = @RoomNum + 1;
        END
        SET @Floor = @Floor + 1;
    END
END
GO

-- =============================================
-- STORED PROCEDURE: تقرير الوجبات للطالب
-- =============================================
CREATE PROCEDURE SP_GetMealReport
    @StudentID INT,
    @FromDate  DATE,
    @ToDate    DATE
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        m.MealDate    AS [اليوم],
        m.MealType    AS [يستحق],
        CASE WHEN m.Status = 'Taken' THEN N'✓' ELSE N'✗' END AS [استلم]
    FROM Meal m
    WHERE m.StudentID = @StudentID
      AND m.MealDate BETWEEN @FromDate AND @ToDate
      AND m.IsDeleted = 0
    ORDER BY m.MealDate;
END
GO

-- =============================================
-- STORED PROCEDURE: تقرير الرسوم للطالب
-- =============================================
CREATE PROCEDURE SP_GetPaymentReport
    @StudentID    INT,
    @AcademicYear NVARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        p.PaymentCategory AS [الرسم],
        p.Amount          AS [المبلغ],
        p.PaymentDate     AS [تاريخ السداد],
        p.Semester        AS [شهر السداد]
    FROM Payment p
    WHERE p.StudentID = @StudentID
      AND p.IsDeleted  = 0
      AND (@AcademicYear IS NULL OR p.AcademicYear = @AcademicYear)
    ORDER BY p.PaymentDate DESC;
END
GO

-- =============================================
-- VIEW: بيان حالة الطالب
-- =============================================
CREATE VIEW VW_StudentStatus AS
SELECT
    s.StudentID,
    s.StudentCode,
    s.Name,
    s.FullNameArabic,
    s.NationalID,
    s.PassportNumber,
    s.Gender,
    s.BirthDate,
    s.BirthPlace,
    s.Religion,
    s.Residence,
    s.Address,
    s.Email,
    s.Phone,
    s.Mobile,
    s.Faculty,
    s.Department,
    s.Grade,
    s.GradePercentage,
    s.StudentStatus,
    s.Nationality,
    s.HousingType,
    s.DistanceFromHome,
    s.FatherName,
    s.FatherPhone,
    s.ParentStatus,
    s.FamilyAbroad,
    s.SpecialNeeds,
    s.HighSchoolDivision,
    s.HighSchoolPercentage,
    s.LastYearGrade,
    s.LastYearPercentage,
    b.BuildingName,
    b.BuildingType,
    r.RoomNumber,
    al.BedNumber,
    al.FromDate  AS CheckInDate,
    al.ToDate    AS CheckOutDate,
    al.AcademicYear,
    al.IsActive  AS IsCurrentlyResident
FROM Student s
LEFT JOIN Allocation al ON al.StudentID = s.StudentID AND al.IsActive = 1 AND al.IsDeleted = 0
LEFT JOIN Room r        ON r.RoomID     = al.RoomID
LEFT JOIN Building b    ON b.BuildingID = r.BuildingID
WHERE s.IsDeleted = 0;
GO

-- =============================================
-- VIEW: إحصائيات إشغال الغرف
-- =============================================
CREATE VIEW VW_RoomOccupancy AS
SELECT
    b.BuildingName,
    b.BuildingType,
    r.RoomID,
    r.RoomNumber,
    r.Floor,
    r.RoomType,
    r.BedsCount,
    r.CurrentOccupancy,
    (r.BedsCount - r.CurrentOccupancy) AS AvailableBeds,
    CASE
        WHEN r.CurrentOccupancy = 0           THEN N'فارغة'
        WHEN r.CurrentOccupancy = r.BedsCount THEN N'ممتلئة'
        ELSE N'متاحة جزئياً'
    END AS OccupancyStatus
FROM Room r
JOIN Building b ON b.BuildingID = r.BuildingID
WHERE r.IsDeleted = 0 AND r.IsActive = 1 AND b.IsDeleted = 0;
GO

PRINT '================================================';
PRINT 'DormitoryDB - Assiut University';
PRINT 'Database Created Successfully - Empty Version';
PRINT 'Tables: 21 | Views: 2 | Procedures: 3';
PRINT '================================================';
GO
