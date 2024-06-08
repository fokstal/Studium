using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class removeConfigOnGradesEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grades_GradeTypeEntity_TypeId",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentToValueEntity_Grades_GradeId",
                table: "StudentToValueEntity");

            migrationBuilder.DropIndex(
                name: "IX_StudentToValueEntity_GradeId",
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
                name: "FK_Grades_GradeTypeEntity_TypeId",
                table: "Grades",
                column: "TypeId",
                principalTable: "GradeTypeEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentToValueEntity_Grades_GradesEntityId",
                table: "StudentToValueEntity",
                column: "GradesEntityId",
                principalTable: "Grades",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grades_GradeTypeEntity_TypeId",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentToValueEntity_Grades_GradesEntityId",
                table: "StudentToValueEntity");

            migrationBuilder.DropIndex(
                name: "IX_StudentToValueEntity_GradesEntityId",
                table: "StudentToValueEntity");

            migrationBuilder.DropColumn(
                name: "GradesEntityId",
                table: "StudentToValueEntity");

            migrationBuilder.CreateIndex(
                name: "IX_StudentToValueEntity_GradeId",
                table: "StudentToValueEntity",
                column: "GradeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_GradeTypeEntity_TypeId",
                table: "Grades",
                column: "TypeId",
                principalTable: "GradeTypeEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentToValueEntity_Grades_GradeId",
                table: "StudentToValueEntity",
                column: "GradeId",
                principalTable: "Grades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
