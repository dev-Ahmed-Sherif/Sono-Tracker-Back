using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SonoTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StartProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "TicketReference");

            migrationBuilder.CreateTable(
                name: "Actions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByEmployeeEn = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedByEmployeeAr = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ModifiedByEmployeeEn = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ModifiedByEmployeeAr = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedByEmployeeId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedByEmployeeId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditTrails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OldValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AffectedColumns = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByEmployeeEn = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedByEmployeeAr = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ModifiedByEmployeeEn = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ModifiedByEmployeeAr = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedByEmployeeId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedByEmployeeId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditTrails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByEmployeeEn = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedByEmployeeAr = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ModifiedByEmployeeEn = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ModifiedByEmployeeAr = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedByEmployeeId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedByEmployeeId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByEmployeeEn = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedByEmployeeAr = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ModifiedByEmployeeEn = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ModifiedByEmployeeAr = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedByEmployeeId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedByEmployeeId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CssClass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByEmployeeEn = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedByEmployeeAr = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ModifiedByEmployeeEn = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ModifiedByEmployeeAr = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedByEmployeeId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedByEmployeeId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    NameAr = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByEmployeeEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedByEmployeeAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ModifiedByEmployeeEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ModifiedByEmployeeAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedByEmployeeId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ModifiedByEmployeeId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestAttachment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Extension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    AttachmentDisplaySize = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByEmployeeEn = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedByEmployeeAr = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ModifiedByEmployeeEn = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ModifiedByEmployeeAr = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedByEmployeeId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedByEmployeeId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestAttachment_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Actions",
                columns: new[] { "Id", "Code", "CreatedByEmployeeAr", "CreatedByEmployeeEn", "CreatedByEmployeeId", "CreatedById", "CreatedDate", "IpAddress", "IsDeleted", "ModifiedByEmployeeAr", "ModifiedByEmployeeEn", "ModifiedByEmployeeId", "ModifiedById", "ModifiedDate", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1, "ADD", null, null, null, "1", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, null, null, null, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "إضافة", "Add" },
                    { 2, "APPROVE", null, null, null, "1", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, null, null, null, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "موافقة", "Approve" },
                    { 3, "REJECT", null, null, null, "1", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, null, null, null, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "رفض", "Reject" }
                });

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Id", "Code", "CreatedByEmployeeAr", "CreatedByEmployeeEn", "CreatedByEmployeeId", "CreatedById", "CreatedDate", "CssClass", "EntityName", "IpAddress", "IsDeleted", "ModifiedByEmployeeAr", "ModifiedByEmployeeEn", "ModifiedByEmployeeId", "ModifiedById", "ModifiedDate", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1, "NEW", null, null, null, "1", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "status-success", null, null, false, null, null, null, null, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "جديدة", "New" },
                    { 2, "PENDING", null, null, null, "1", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "status-info", null, null, false, null, null, null, null, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "قيد الإنتظار", "Pending" },
                    { 3, "CLOSED", null, null, null, "1", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "status-warning", null, null, false, null, null, null, null, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "مغلقة", "Closed" },
                    { 4, "COMPLETED", null, null, null, "1", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "status-warning", null, null, false, null, null, null, null, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "مكتملة", "Completed" },
                    { 5, "REJECTED", null, null, null, "1", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "status-warning", null, null, false, null, null, null, null, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "مرفوض", "Rejected" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestAttachment_TestId",
                table: "TestAttachment",
                column: "TestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Actions");

            migrationBuilder.DropTable(
                name: "AuditTrails");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropTable(
                name: "TestAttachment");

            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.DropSequence(
                name: "TicketReference");
        }
    }
}
