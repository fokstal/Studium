using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class updateStudentToValueEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentToValueEntity_Grades_GradesEntityId",
                table: "StudentToValueEntity");

            migrationBuilder.DropIndex(
                name: "IX_StudentToValueEntity_GradesEntityId",
                table: "StudentToValueEntity");

            migrationBuilder.DropColumn(
                name: "GradesEntityId",
                table: "StudentToValueEntity");

            migrationBuilder.AddColumn<int>(
                name: "GradeId",
                table: "StudentToValueEntity",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StudentToValueEntity_GradeId",
                table: "StudentToValueEntity",
                column: "GradeId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentToValueEntity_Grades_GradeId",
                table: "StudentToValueEntity",
                column: "GradeId",
                principalTable: "Grades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentToValueEntity_Grades_GradeId",
                table: "StudentToValueEntity");

            migrationBuilder.DropIndex(
                name: "IX_StudentToValueEntity_GradeId",
                table: "StudentToValueEntity");

            migrationBuilder.DropColumn(
                name: "GradeId",
                table: "StudentToValueEntity");

            migrationBuilder.AddColumn<int>(
                name: "GradesEntityId",
                table: "StudentToValueEntity",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentToValueEntity_GradesEntityId",
                table: "StudentToValueEntity",
                column: "GradesEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentToValueEntity_Grades_GradesEntityId",
                table: "StudentToValueEntity",
                column: "GradesEntityId",
                principalTable: "Grades",
                principalColumn: "Id");
        }
    }
}
