using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class changeEntityInGradeListToHashSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentToValueEntity");

            migrationBuilder.CreateTable(
                name: "GradeEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StudentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Value = table.Column<int>(type: "INTEGER", nullable: false),
                    GradeModelId = table.Column<int>(type: "INTEGER", nullable: false),
                    GradeModelEntityId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GradeEntity_Grades_GradeModelEntityId",
                        column: x => x.GradeModelEntityId,
                        principalTable: "Grades",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GradeEntity_GradeModelEntityId",
                table: "GradeEntity",
                column: "GradeModelEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GradeEntity");

            migrationBuilder.CreateTable(
                name: "StudentToValueEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GradesEntityId = table.Column<int>(type: "INTEGER", nullable: false),
                    StudentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Value = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentToValueEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentToValueEntity_Grades_GradesEntityId",
                        column: x => x.GradesEntityId,
                        principalTable: "Grades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentToValueEntity_GradesEntityId",
                table: "StudentToValueEntity",
                column: "GradesEntityId");
        }
    }
}
