using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using UniStay.Models;

namespace UniStay.Data;

public partial class DormitoryDbContext : DbContext
{
    public DormitoryDbContext(DbContextOptions<DormitoryDbContext> options)
        : base(options)
    {
    }
    public DbSet<RegistrationDate> RegistrationDates { get; set; }
    public virtual DbSet<Absence> Absences { get; set; }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Allocation> Allocations { get; set; }

    public virtual DbSet<Announcement> Announcements { get; set; }

    public virtual DbSet<AnnouncementAttachment> AnnouncementAttachments { get; set; }

    public virtual DbSet<Application> Applications { get; set; }

    public virtual DbSet<ApplicationSchedule> ApplicationSchedules { get; set; }

    public virtual DbSet<Building> Buildings { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<Dormitory> Dormitories { get; set; }

    public virtual DbSet<EvictionNotice> EvictionNotices { get; set; }

    public virtual DbSet<Guardian> Guardians { get; set; }

    public virtual DbSet<MaintenanceRequest> MaintenanceRequests { get; set; }

    public virtual DbSet<Meal> Meals { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Student> Students { get; set; }
    public virtual DbSet<AcceptingStudent> AcceptingStudents { get; set; }

    public virtual DbSet<StudentDownloadLog> StudentDownloadLogs { get; set; }

    public virtual DbSet<StudentLogin> StudentLogins { get; set; }

    public virtual DbSet<University> Universities { get; set; }

    public virtual DbSet<Violation> Violations { get; set; }

    public virtual DbSet<VwRoomOccupancy> VwRoomOccupancies { get; set; }

    public virtual DbSet<VwStudentStatus> VwStudentStatuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Absence>(entity =>
        {
            entity.HasKey(e => e.AbsenceId).HasName("PK__Absence__3A074E471A03DB31");

            entity.ToTable("Absence");

            entity.Property(e => e.AbsenceId).HasColumnName("AbsenceID");
            entity.Property(e => e.AbsenceType).HasMaxLength(50);
            entity.Property(e => e.AllocationId).HasColumnName("AllocationID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.DaysCount).HasComputedColumnSql("(datediff(day,[FromDate],[ToDate])+(1))", true);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.IsWeekendIncluded).HasDefaultValue(true);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Allocation).WithMany(p => p.Absences)
                .HasForeignKey(d => d.AllocationId)
                .HasConstraintName("FK__Absence__Allocat__498EEC8D");

            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.Absences)
                .HasForeignKey(d => d.ApprovedBy)
                .HasConstraintName("FK__Absence__Approve__4A8310C6");

            entity.HasOne(d => d.Student).WithMany(p => p.Absences)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Absence__Student__489AC854");
        });

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admin__719FE4E880AFF735");

            entity.ToTable("Admin");

            entity.HasIndex(e => e.Username, "UQ__Admin__536C85E456A1733F").IsUnique();

            entity.Property(e => e.AdminId).HasColumnName("AdminID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.DormitoryId).HasColumnName("DormitoryID");
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.PasswordHash).HasMaxLength(512);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(100);

            entity.HasOne(d => d.Dormitory).WithMany(p => p.Admins)
                .HasForeignKey(d => d.DormitoryId)
                .HasConstraintName("FK__Admin__Dormitory__06CD04F7");
        });

        modelBuilder.Entity<Allocation>(entity =>
        {
            entity.HasKey(e => e.AllocationId).HasName("PK__Allocati__B3C6D6ABF8A35DA6");

            entity.ToTable("Allocation");

            entity.HasIndex(e => e.RoomId, "IDX_Allocation_RoomID");

            entity.HasIndex(e => e.StudentId, "UQ_ActiveAllocation")
                .IsUnique()
                .HasFilter("([IsActive]=(1) AND [IsDeleted]=(0))");

            entity.HasIndex(e => new { e.RoomId, e.BedNumber }, "UQ_BedAllocation")
                .IsUnique()
                .HasFilter("([IsActive]=(1) AND [IsDeleted]=(0))");

            entity.Property(e => e.AllocationId).HasColumnName("AllocationID");
            entity.Property(e => e.AcademicYear).HasMaxLength(20);
            entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.MealPlanAllocated).HasMaxLength(50);
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.Semester).HasMaxLength(20);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.AllocatedByNavigation).WithMany(p => p.Allocations)
                .HasForeignKey(d => d.AllocatedBy)
                .HasConstraintName("FK__Allocatio__Alloc__25518C17");

            entity.HasOne(d => d.Application).WithMany(p => p.Allocations)
                .HasForeignKey(d => d.ApplicationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Allocatio__Appli__22751F6C");

            entity.HasOne(d => d.Room).WithMany(p => p.Allocations)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Allocatio__RoomI__245D67DE");

            entity.HasOne(d => d.Student).WithOne(p => p.Allocation)
                .HasForeignKey<Allocation>(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Allocatio__Stude__236943A5");
        });

        modelBuilder.Entity<Announcement>(entity =>
        {
            entity.HasKey(e => e.AnnouncementId).HasName("PK__Announce__9DE44554A69D832F");

            entity.ToTable("Announcement");

            entity.Property(e => e.AnnouncementId).HasColumnName("AnnouncementID");
            entity.Property(e => e.AcademicYear).HasMaxLength(20);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.DormitoryId).HasColumnName("DormitoryID");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.IsPublic).HasDefaultValue(true);
            entity.Property(e => e.PublishedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Title).HasMaxLength(500);
            entity.Property(e => e.UniversityId).HasColumnName("UniversityID");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Announcements)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Announcem__Creat__6AEFE058");

            entity.HasOne(d => d.Dormitory).WithMany(p => p.Announcements)
                .HasForeignKey(d => d.DormitoryId)
                .HasConstraintName("FK__Announcem__Dormi__69FBBC1F");

            entity.HasOne(d => d.University).WithMany(p => p.Announcements)
                .HasForeignKey(d => d.UniversityId)
                .HasConstraintName("FK__Announcem__Unive__690797E6");
        });

        modelBuilder.Entity<AnnouncementAttachment>(entity =>
        {
            entity.HasKey(e => e.AttachmentId).HasName("PK__Announce__442C64DE72D109DF");

            entity.ToTable("AnnouncementAttachment");

            entity.Property(e => e.AttachmentId).HasColumnName("AttachmentID");
            entity.Property(e => e.AnnouncementId).HasColumnName("AnnouncementID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.FileName).HasMaxLength(300);
            entity.Property(e => e.FileType).HasMaxLength(20);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasOne(d => d.Announcement).WithMany(p => p.AnnouncementAttachments)
                .HasForeignKey(d => d.AnnouncementId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Announcem__Annou__6FB49575");
        });

        modelBuilder.Entity<Application>(entity =>
        {
            entity.HasKey(e => e.ApplicationId).HasName("PK__Applicat__C93A4F79D368EB6C");

            entity.ToTable("Application");

            entity.HasIndex(e => e.HousingPreference, "IDX_Application_Preference");

            entity.HasIndex(e => e.Status, "IDX_Application_Status");

            entity.HasIndex(e => new { e.StudentId, e.AcademicYear, e.Semester }, "UQ_App_Student_Year")
                .IsUnique()
                .HasFilter("([IsDeleted]=(0))");

            entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");
            entity.Property(e => e.AcademicYear).HasMaxLength(20);
            entity.Property(e => e.ApplicationDate).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.FamilyAbroad).HasDefaultValue(false);
            entity.Property(e => e.HousingPreference).HasMaxLength(20);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.MealPlanType).HasMaxLength(50);
            entity.Property(e => e.PreferredBuildingId).HasColumnName("PreferredBuildingID");
            entity.Property(e => e.PreferredRoomId).HasColumnName("PreferredRoomID");
            entity.Property(e => e.Semester).HasMaxLength(20);
            entity.Property(e => e.SpecialNeeds).HasDefaultValue(false);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.WithoutNutrition).HasDefaultValue(false);

            entity.HasOne(d => d.PreferredBuilding).WithMany(p => p.Applications)
                .HasForeignKey(d => d.PreferredBuildingId)
                .HasConstraintName("FK__Applicati__Prefe__1BC821DD");

            entity.HasOne(d => d.PreferredRoom).WithMany(p => p.Applications)
                .HasForeignKey(d => d.PreferredRoomId)
                .HasConstraintName("FK__Applicati__Prefe__1CBC4616");

            entity.HasOne(d => d.ReviewedByNavigation).WithMany(p => p.Applications)
                .HasForeignKey(d => d.ReviewedBy)
                .HasConstraintName("FK__Applicati__Revie__1AD3FDA4");

            entity.HasOne(d => d.Student).WithMany(p => p.Applications)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Applicati__Stude__19DFD96B");
        });

        modelBuilder.Entity<ApplicationSchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__Applicat__9C8A5B69868F8CF2");

            entity.ToTable("ApplicationSchedule");

            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
            entity.Property(e => e.AcademicYear).HasMaxLength(20);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.DormitoryId).HasColumnName("DormitoryID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.StudentCategory).HasMaxLength(50);

            entity.HasOne(d => d.Dormitory).WithMany(p => p.ApplicationSchedules)
                .HasForeignKey(d => d.DormitoryId)
                .HasConstraintName("FK__Applicati__Dormi__0D7A0286");
        });

        modelBuilder.Entity<Building>(entity =>
        {
            entity.HasKey(e => e.BuildingId).HasName("PK__Building__5463CDE4A6F80590");

            entity.ToTable("Building");

            entity.HasIndex(e => e.BuildingType, "IDX_Building_Type");

            entity.Property(e => e.BuildingId).HasColumnName("BuildingID");
            entity.Property(e => e.BuildingName).HasMaxLength(100);
            entity.Property(e => e.BuildingType).HasMaxLength(20);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.DormitoryId).HasColumnName("DormitoryID");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.TotalRooms).HasDefaultValue(150);

            entity.HasOne(d => d.Dormitory).WithMany(p => p.Buildings)
                .HasForeignKey(d => d.DormitoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Building__Dormit__59063A47");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.DocumentId).HasName("PK__Document__1ABEEF6F35C9E87F");

            entity.ToTable("Document");

            entity.Property(e => e.DocumentId).HasColumnName("DocumentID");
            entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.DocumentType).HasMaxLength(100);
            entity.Property(e => e.FileName).HasMaxLength(300);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.IsVerified).HasDefaultValue(false);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Application).WithMany(p => p.Documents)
                .HasForeignKey(d => d.ApplicationId)
                .HasConstraintName("FK__Document__Applic__607251E5");

            entity.HasOne(d => d.Student).WithMany(p => p.Documents)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__Document__Studen__6166761E");

            entity.HasOne(d => d.VerifiedByNavigation).WithMany(p => p.Documents)
                .HasForeignKey(d => d.VerifiedBy)
                .HasConstraintName("FK__Document__Verifi__625A9A57");
        });

        modelBuilder.Entity<Dormitory>(entity =>
        {
            entity.HasKey(e => e.DormitoryId).HasName("PK__Dormitor__14188ACE157AEAF1");

            entity.ToTable("Dormitory");

            entity.Property(e => e.DormitoryId).HasColumnName("DormitoryID");
            entity.Property(e => e.Address).HasMaxLength(300);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.DormitoryName).HasMaxLength(200);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Type).HasMaxLength(20);
            entity.Property(e => e.UniversityId).HasColumnName("UniversityID");

            entity.HasOne(d => d.University).WithMany(p => p.Dormitories)
                .HasForeignKey(d => d.UniversityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dormitory__Unive__5165187F");
        });

        modelBuilder.Entity<EvictionNotice>(entity =>
        {
            entity.HasKey(e => e.EvictionId).HasName("PK__Eviction__A889DE2E7895F278");

            entity.ToTable("EvictionNotice");

            entity.Property(e => e.EvictionId).HasColumnName("EvictionID");
            entity.Property(e => e.AllocationId).HasColumnName("AllocationID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Allocation).WithMany(p => p.EvictionNotices)
                .HasForeignKey(d => d.AllocationId)
                .HasConstraintName("FK__EvictionN__Alloc__367C1819");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.EvictionNotices)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__EvictionN__Creat__37703C52");

            entity.HasOne(d => d.Student).WithMany(p => p.EvictionNotices)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__EvictionN__Stude__3587F3E0");
        });

        modelBuilder.Entity<Guardian>(entity =>
        {
            entity.HasKey(e => e.GuardianId).HasName("PK__Guardian__0A5E1B7BA9195463");

            entity.ToTable("Guardian");

            entity.Property(e => e.GuardianId).HasColumnName("GuardianID");
            entity.Property(e => e.AlternatePhone).HasMaxLength(20);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.GuardianRole).HasMaxLength(20);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Job).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.NationalId)
                .HasMaxLength(20)
                .HasColumnName("NationalID");
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Relation).HasMaxLength(50);
            entity.Property(e => e.ResidenceAddress).HasMaxLength(300);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Student).WithMany(p => p.Guardians)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Guardian__Studen__7F2BE32F");
        });

        modelBuilder.Entity<MaintenanceRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__Maintena__33A8519A754946C8");

            entity.ToTable("MaintenanceRequest");

            entity.Property(e => e.RequestId).HasColumnName("RequestID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.IssueType).HasMaxLength(100);
            entity.Property(e => e.Priority).HasMaxLength(20);
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.AssignedToNavigation).WithMany(p => p.MaintenanceRequests)
                .HasForeignKey(d => d.AssignedTo)
                .HasConstraintName("FK__Maintenan__Assig__5AB9788F");

            entity.HasOne(d => d.Room).WithMany(p => p.MaintenanceRequests)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK__Maintenan__RoomI__58D1301D");

            entity.HasOne(d => d.Student).WithMany(p => p.MaintenanceRequests)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__Maintenan__Stude__59C55456");
        });

        modelBuilder.Entity<Meal>(entity =>
        {
            entity.HasKey(e => e.MealId).HasName("PK__Meal__ACF6A65D83EC72C7");

            entity.ToTable("Meal");

            entity.HasIndex(e => e.MealDate, "IDX_Meal_Date");

            entity.HasIndex(e => e.StudentId, "IDX_Meal_StudentID");

            entity.HasIndex(e => new { e.StudentId, e.MealDate, e.MealType }, "UQ__Meal__3449321F74EFBA23").IsUnique();

            entity.Property(e => e.MealId).HasColumnName("MealID");
            entity.Property(e => e.AllocationId).HasColumnName("AllocationID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.IsBooked).HasDefaultValue(false);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.MealType).HasMaxLength(20);
            entity.Property(e => e.MissedPenalty)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Allocation).WithMany(p => p.Meals)
                .HasForeignKey(d => d.AllocationId)
                .HasConstraintName("FK__Meal__Allocation__41EDCAC5");

            entity.HasOne(d => d.Student).WithMany(p => p.Meals)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__Meal__StudentID__40F9A68C");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__9B556A58940BC47E");

            entity.ToTable("Payment");

            entity.HasIndex(e => e.PaymentCategory, "IDX_Payment_Category");

            entity.HasIndex(e => e.StudentId, "IDX_Payment_StudentID");

            entity.HasIndex(e => e.ReceiptNumber, "UQ__Payment__C08AFDAB02FB820D").IsUnique();

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.AcademicYear).HasMaxLength(20);
            entity.Property(e => e.AllocationId).HasColumnName("AllocationID");
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.IsOverdue).HasDefaultValue(false);
            entity.Property(e => e.PaymentCategory).HasMaxLength(50);
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.PaymentType).HasMaxLength(50);
            entity.Property(e => e.ReceiptNumber).HasMaxLength(100);
            entity.Property(e => e.Semester).HasMaxLength(20);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Allocation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.AllocationId)
                .HasConstraintName("FK__Payment__Allocat__2EDAF651");

            entity.HasOne(d => d.ReceivedByNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.ReceivedBy)
                .HasConstraintName("FK__Payment__Receive__2FCF1A8A");

            entity.HasOne(d => d.Student).WithMany(p => p.Payments)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__Payment__Student__2DE6D218");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("PK__Room__328639197573C58E");

            entity.ToTable("Room");

            entity.HasIndex(e => e.CurrentOccupancy, "IDX_Room_Occupancy");

            entity.HasIndex(e => e.RoomType, "IDX_Room_Type");

            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.BuildingId).HasColumnName("BuildingID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.CurrentOccupancy).HasDefaultValue(0);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.RoomNumber).HasMaxLength(20);
            entity.Property(e => e.RoomType).HasMaxLength(20);

            entity.HasOne(d => d.Building).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.BuildingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Room__BuildingID__66603565");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Student__32C52A79F8FED0B2");

            entity.ToTable("Student");

            entity.HasIndex(e => e.Email, "IDX_Student_Email");

            entity.HasIndex(e => e.NationalId, "IDX_Student_NationalID");

            entity.HasIndex(e => e.Nationality, "IDX_Student_Nationality");

            entity.HasIndex(e => e.StudentStatus, "IDX_Student_Status");

            entity.HasIndex(e => e.NationalId, "UQ__Student__E9AA321A798CF797").IsUnique();

            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.Address).HasMaxLength(300);
            entity.Property(e => e.BirthPlace).HasMaxLength(200);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Faculty).HasMaxLength(200);
            entity.Property(e => e.FamilyAbroad).HasDefaultValue(false);
            entity.Property(e => e.FatherJob).HasMaxLength(100);
            entity.Property(e => e.FatherName).HasMaxLength(200);
            entity.Property(e => e.FatherNationalId)
                .HasMaxLength(14)
                .HasColumnName("FatherNationalID");
            entity.Property(e => e.FatherPhone).HasMaxLength(20);
            entity.Property(e => e.FullNameArabic).HasMaxLength(200);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.Grade).HasMaxLength(20);
            entity.Property(e => e.HasMedicalCondition).HasDefaultValue(false);
            entity.Property(e => e.HighSchoolDivision).HasMaxLength(100);
            entity.Property(e => e.HighSchoolFromAbroad).HasDefaultValue(false);
            entity.Property(e => e.HousingType).HasMaxLength(20);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.LastYearGrade).HasMaxLength(50);
            entity.Property(e => e.LastYearPercentage).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Mobile).HasMaxLength(20);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.NationalId)
                .HasMaxLength(20)
                .HasColumnName("NationalID");
            entity.Property(e => e.Nationality)
                .HasMaxLength(20)
                .HasDefaultValue("Egyptian");
            entity.Property(e => e.ParentStatus).HasMaxLength(50);
            entity.Property(e => e.PassportIssuePlace).HasMaxLength(100);
            entity.Property(e => e.PassportNumber).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Religion).HasMaxLength(50);
            entity.Property(e => e.Residence).HasMaxLength(200);
            entity.Property(e => e.ResidenceCity).HasMaxLength(100);
            entity.Property(e => e.ResidenceCountry)
                .HasMaxLength(100)
                .HasDefaultValue("مصر");
            entity.Property(e => e.ResidenceGovernorate).HasMaxLength(100);
            entity.Property(e => e.SpecialNeeds).HasDefaultValue(false);
            entity.Property(e => e.StudentCode).HasMaxLength(50);
            entity.Property(e => e.StudentStatus)
                .HasMaxLength(20)
                .HasDefaultValue("New");
            entity.Property(e => e.StudyType).HasMaxLength(50);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Status)
                .HasConversion<string>();
            // يخزن Pending / Approved / Rejected كنص في الداتابيز
        });

        modelBuilder.Entity<StudentDownloadLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__StudentD__5E5499A837B7D305");

            entity.ToTable("StudentDownloadLog");

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.DownloadedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.FileName).HasMaxLength(300);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentDownloadLogs)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StudentDo__Stude__7B264821");
        });

        modelBuilder.Entity<StudentLogin>(entity =>
        {
            entity.HasKey(e => e.LoginId).HasName("PK__StudentL__4DDA28384EF1ECE5");

            entity.ToTable("StudentLogin");

            entity.HasIndex(e => e.StudentId, "UQ__StudentL__32C52A78F048A48E").IsUnique();

            entity.HasIndex(e => e.NationalId, "UQ__StudentL__E9AA321A22477898").IsUnique();

            entity.Property(e => e.LoginId).HasColumnName("LoginID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.NationalId)
                .HasMaxLength(20)
                .HasColumnName("NationalID");
            entity.Property(e => e.PasswordHash).HasMaxLength(512);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Student).WithOne(p => p.StudentLogin)
                .HasForeignKey<StudentLogin>(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StudentLo__Stude__7755B73D");
        });

        modelBuilder.Entity<University>(entity =>
        {
            entity.HasKey(e => e.UniversityId).HasName("PK__Universi__9F19E19C102D5BC8");

            entity.ToTable("University");

            entity.Property(e => e.UniversityId).HasColumnName("UniversityID");
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Governorate).HasMaxLength(100);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UniversityName).HasMaxLength(200);
        });

        modelBuilder.Entity<Violation>(entity =>
        {
            entity.HasKey(e => e.ViolationId).HasName("PK__Violatio__18B6DC2893B67D33");

            entity.ToTable("Violation");

            entity.HasIndex(e => e.StudentId, "IDX_Violation_StudentID");

            entity.Property(e => e.ViolationId).HasColumnName("ViolationID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.IsPaid).HasDefaultValue(false);
            entity.Property(e => e.Penalty).HasMaxLength(50);
            entity.Property(e => e.PenaltyAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.ViolationType).HasMaxLength(100);

            entity.HasOne(d => d.RecordedByNavigation).WithMany(p => p.Violations)
                .HasForeignKey(d => d.RecordedBy)
                .HasConstraintName("FK__Violation__Recor__5224328E");

            entity.HasOne(d => d.Student).WithMany(p => p.Violations)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__Violation__Stude__51300E55");
        });

        modelBuilder.Entity<VwRoomOccupancy>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VW_RoomOccupancy");

            entity.Property(e => e.BuildingName).HasMaxLength(100);
            entity.Property(e => e.BuildingType).HasMaxLength(20);
            entity.Property(e => e.OccupancyStatus).HasMaxLength(12);
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.RoomNumber).HasMaxLength(20);
            entity.Property(e => e.RoomType).HasMaxLength(20);
        });

        modelBuilder.Entity<VwStudentStatus>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VW_StudentStatus");

            entity.Property(e => e.AcademicYear).HasMaxLength(20);
            entity.Property(e => e.Address).HasMaxLength(300);
            entity.Property(e => e.BirthPlace).HasMaxLength(200);
            entity.Property(e => e.BuildingName).HasMaxLength(100);
            entity.Property(e => e.BuildingType).HasMaxLength(20);
            entity.Property(e => e.Department).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Faculty).HasMaxLength(200);
            entity.Property(e => e.FatherName).HasMaxLength(200);
            entity.Property(e => e.FatherPhone).HasMaxLength(20);
            entity.Property(e => e.FullNameArabic).HasMaxLength(200);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.Grade).HasMaxLength(20);
            entity.Property(e => e.HighSchoolDivision).HasMaxLength(100);
            entity.Property(e => e.HousingType).HasMaxLength(20);
            entity.Property(e => e.LastYearGrade).HasMaxLength(50);
            entity.Property(e => e.LastYearPercentage).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Mobile).HasMaxLength(20);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.NationalId)
                .HasMaxLength(20)
                .HasColumnName("NationalID");
            entity.Property(e => e.Nationality).HasMaxLength(20);
            entity.Property(e => e.ParentStatus).HasMaxLength(50);
            entity.Property(e => e.PassportNumber).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Religion).HasMaxLength(50);
            entity.Property(e => e.Residence).HasMaxLength(200);
            entity.Property(e => e.RoomNumber).HasMaxLength(20);
            entity.Property(e => e.StudentCode).HasMaxLength(50);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.StudentStatus).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
