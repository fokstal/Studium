using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class updateFieldGradeIdInModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentToValueEntity_Grades_GradesEntityId",
                table: "StudentToValueEntity");

            migrationBuilder.DropColumn(
                name: "GradeId",
                table: "StudentToValueEntity");

            migrationBuilder.AlterColumn<int>(
                name: "GradesEntityId",
                table: "StudentToValueEntity",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentToValueEntity_Grades_GradesEntityId",
                table: "StudentToValueEntity",
                column: "GradesEntityId",
                principalTable: "Grades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentToValueEntity_Grades_GradesEntityId",
                table: "StudentToValueEntity");

            migrationBuilder.AlterColumn<int>(
                name: "GradesEntityId",
                table: "StudentToValueEntity",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "GradeId",
                table: "StudentToValueEntity",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentToValueEntity_Grades_GradesEntityId",
                table: "StudentToValueEntity",
                column: "GradesEntityId",
                principalTable: "Grades",
                principalColumn: "Id");
        }
    }
}
