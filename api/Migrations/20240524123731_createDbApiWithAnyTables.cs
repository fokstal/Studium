using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class createDbApiWithAnyTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GradeTypeEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeTypeEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    CuratorId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AuditoryName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Passport",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ScanFileName = table.Column<string>(type: "TEXT", nullable: false),
                    PersonId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passport", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Login = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    MiddleName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RemovedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PersonId = table.Column<int>(type: "INTEGER", nullable: false),
                    GroupId = table.Column<int>(type: "INTEGER", nullable: true),
                    GroupEntityId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Student_Group_GroupEntityId",
                        column: x => x.GroupEntityId,
                        principalTable: "Group",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Subject",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    TeacherId = table.Column<Guid>(type: "TEXT", nullable: false),
                    GroupId = table.Column<int>(type: "INTEGER", nullable: true),
                    GroupEntityId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subject", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subject_Group_GroupEntityId",
                        column: x => x.GroupEntityId,
                        principalTable: "Group",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RolePermissionEntity",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "INTEGER", nullable: false),
                    PermissionId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissionEntity", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermissionEntity_PermissionEntity_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "PermissionEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissionEntity_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoleEntity",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoleId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoleEntity", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoleEntity_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoleEntity_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    MiddleName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Sex = table.Column<int>(type: "INTEGER", nullable: false),
                    AvatarFileName = table.Column<string>(type: "TEXT", nullable: false),
                    PassportId = table.Column<int>(type: "INTEGER", nullable: true),
                    StudentId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Person_Passport_PassportId",
                        column: x => x.PassportId,
                        principalTable: "Passport",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Person_Student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Grade",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<int>(type: "INTEGER", nullable: false),
                    SetDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    StudentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SubjectId = table.Column<int>(type: "INTEGER", nullable: false),
                    SubjectEntityId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Grade_GradeTypeEntity_Id",
                        column: x => x.Id,
                        principalTable: "GradeTypeEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Grade_Subject_SubjectEntityId",
                        column: x => x.SubjectEntityId,
                        principalTable: "Subject",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "GradeTypeEntity",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Practice" },
                    { 2, "ControlWork" },
                    { 3, "Lecture" }
                });

            migrationBuilder.InsertData(
                table: "PermissionEntity",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "ViewGradeList" },
                    { 2, "ViewGrade" },
                    { 3, "EditGrade" },
                    { 4, "ViewGroup" },
                    { 5, "EditGroup" },
                    { 6, "ViewPassportList" },
                    { 7, "ViewPassport" },
                    { 8, "EditPassport" },
                    { 9, "ViewPersonList" },
                    { 10, "ViewPerson" },
                    { 11, "EditPerson" },
                    { 12, "ViewStudentList" },
                    { 13, "ViewStudent" },
                    { 14, "EditStudent" },
                    { 15, "ViewSubjectList" },
                    { 16, "ViewSubject" },
                    { 17, "EditSubject" },
                    { 18, "ViewUser" },
                    { 19, "RegisterUser" },
                    { 20, "EditUser" }
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Secretar" },
                    { 3, "Curator" },
                    { 4, "Teacher" },
                    { 5, "Student" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "DateCreated", "FirstName", "LastName", "Login", "MiddleName", "PasswordHash" },
                values: new object[,]
                {
                    { new Guid("08bfaf3b-2a88-102d-1330-93a1b8433f50"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "CuratorBase", "", "curator", "", "tiMq2aFb7mOs35MsukDafs+e0fJVAJFcPykiCirdGYGZkqXA4uSzBYCBQk/jWGTfEgBLNCol0UcTAEdnXUEENw==" },
                    { new Guid("1144b240-4126-78dd-dd4f-93b6c9190dd4"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SecretarBase", "", "secretar", "", "ldntahdGhOPXDqs16vPWLoGYILz5TLDlTlrqlh2sI56JzrXd6MXx4a4vrD+sHSEoWZ0KdjSczeERVLKl+oY6YQ==" },
                    { new Guid("3705df06-8119-37a2-d0ed-11472fae7c94"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "StudentBase", "", "student", "", "jb7vuZcUaY7Tl3gzhaNc880qfvwoCAaGvCGevz+DEI+glffW/gH4fOpQUyJ42/r3QbOQAsqyyS85E1hBoFQG1w==" },
                    { new Guid("a34eff4d-f040-23a8-f15d-3f4f01ab62ea"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AdminBase", "", "admin", "", "wNT47n/qyBSao7WnTCiZurLCZMmiEDmOzNKe0fuAh68syTJE8Yu1/bOhE8OA+KlRzHgc4DbrAwC20oJ2ruAYRg==" },
                    { new Guid("b4d821a3-e305-26ef-0495-9847b36d171e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "TeacherBase", "", "teacher", "", "80n2TDMphhuzv9mfZnEwvnqrLn7R7ffGZt0WNGNSSuunsRpYJDy+d2/1cZDkw6POrZDyWw8Geuek9NMUoL5duw==" }
                });

            migrationBuilder.InsertData(
                table: "RolePermissionEntity",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 },
                    { 4, 1 },
                    { 5, 1 },
                    { 6, 1 },
                    { 7, 1 },
                    { 8, 1 },
                    { 9, 1 },
                    { 10, 1 },
                    { 11, 1 },
                    { 12, 1 },
                    { 13, 1 },
                    { 14, 1 },
                    { 15, 1 },
                    { 16, 1 },
                    { 17, 1 },
                    { 18, 1 },
                    { 19, 1 },
                    { 20, 1 },
                    { 1, 2 },
                    { 2, 2 },
                    { 3, 2 },
                    { 4, 2 },
                    { 5, 2 },
                    { 6, 2 },
                    { 7, 2 },
                    { 8, 2 },
                    { 9, 2 },
                    { 10, 2 },
                    { 11, 2 },
                    { 12, 2 },
                    { 13, 2 },
                    { 14, 2 },
                    { 15, 2 },
                    { 16, 2 },
                    { 17, 2 },
                    { 18, 2 },
                    { 19, 2 },
                    { 20, 2 },
                    { 2, 3 },
                    { 4, 3 },
                    { 10, 3 },
                    { 13, 3 },
                    { 16, 3 },
                    { 2, 4 },
                    { 3, 4 },
                    { 4, 4 },
                    { 10, 4 },
                    { 13, 4 },
                    { 16, 4 },
                    { 2, 5 },
                    { 4, 5 },
                    { 7, 5 },
                    { 10, 5 },
                    { 13, 5 },
                    { 16, 5 }
                });

            migrationBuilder.InsertData(
                table: "UserRoleEntity",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { 3, new Guid("08bfaf3b-2a88-102d-1330-93a1b8433f50") },
                    { 2, new Guid("1144b240-4126-78dd-dd4f-93b6c9190dd4") },
                    { 5, new Guid("3705df06-8119-37a2-d0ed-11472fae7c94") },
                    { 1, new Guid("a34eff4d-f040-23a8-f15d-3f4f01ab62ea") },
                    { 4, new Guid("b4d821a3-e305-26ef-0495-9847b36d171e") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Grade_SubjectEntityId",
                table: "Grade",
                column: "SubjectEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Person_PassportId",
                table: "Person",
                column: "PassportId");

            migrationBuilder.CreateIndex(
                name: "IX_Person_StudentId",
                table: "Person",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissionEntity_PermissionId",
                table: "RolePermissionEntity",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Student_GroupEntityId",
                table: "Student",
                column: "GroupEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Subject_GroupEntityId",
                table: "Subject",
                column: "GroupEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleEntity_RoleId",
                table: "UserRoleEntity",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Grade");

            migrationBuilder.DropTable(
                name: "Person");

            migrationBuilder.DropTable(
                name: "RolePermissionEntity");

            migrationBuilder.DropTable(
                name: "UserRoleEntity");

            migrationBuilder.DropTable(
                name: "GradeTypeEntity");

            migrationBuilder.DropTable(
                name: "Subject");

            migrationBuilder.DropTable(
                name: "Passport");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "PermissionEntity");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Group");
        }
    }
}
