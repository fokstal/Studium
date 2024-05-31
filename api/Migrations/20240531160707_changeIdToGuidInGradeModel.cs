using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class changeIdToGuidInGradeModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GradeEntity_Grades_GradeModelEntityId",
                table: "GradeEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_GradeTypeEntity_TypeId",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Subject_SubjectEntityId",
                table: "Grades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GradeTypeEntity",
                table: "GradeTypeEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Grades",
                table: "Grades");

            migrationBuilder.RenameTable(
                name: "GradeTypeEntity",
                newName: "GradeType");

            migrationBuilder.RenameTable(
                name: "Grades",
                newName: "GradeModel");

            migrationBuilder.RenameIndex(
                name: "IX_Grades_TypeId",
                table: "GradeModel",
                newName: "IX_GradeModel_TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Grades_SubjectEntityId",
                table: "GradeModel",
                newName: "IX_GradeModel_SubjectEntityId");

            migrationBuilder.AlterColumn<Guid>(
                name: "GradeModelId",
                table: "GradeEntity",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<Guid>(
                name: "GradeModelEntityId",
                table: "GradeEntity",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "GradeModel",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GradeType",
                table: "GradeType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GradeModel",
                table: "GradeModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GradeEntity_GradeModel_GradeModelEntityId",
                table: "GradeEntity",
                column: "GradeModelEntityId",
                principalTable: "GradeModel",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GradeModel_GradeType_TypeId",
                table: "GradeModel",
                column: "TypeId",
                principalTable: "GradeType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GradeModel_Subject_SubjectEntityId",
                table: "GradeModel",
                column: "SubjectEntityId",
                principalTable: "Subject",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GradeEntity_GradeModel_GradeModelEntityId",
                table: "GradeEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_GradeModel_GradeType_TypeId",
                table: "GradeModel");

            migrationBuilder.DropForeignKey(
                name: "FK_GradeModel_Subject_SubjectEntityId",
                table: "GradeModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GradeType",
                table: "GradeType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GradeModel",
                table: "GradeModel");

            migrationBuilder.RenameTable(
                name: "GradeType",
                newName: "GradeTypeEntity");

            migrationBuilder.RenameTable(
                name: "GradeModel",
                newName: "Grades");

            migrationBuilder.RenameIndex(
                name: "IX_GradeModel_TypeId",
                table: "Grades",
                newName: "IX_Grades_TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_GradeModel_SubjectEntityId",
                table: "Grades",
                newName: "IX_Grades_SubjectEntityId");

            migrationBuilder.AlterColumn<int>(
                name: "GradeModelId",
                table: "GradeEntity",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "GradeModelEntityId",
                table: "GradeEntity",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Grades",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GradeTypeEntity",
                table: "GradeTypeEntity",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Grades",
                table: "Grades",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GradeEntity_Grades_GradeModelEntityId",
                table: "GradeEntity",
                column: "GradeModelEntityId",
                principalTable: "Grades",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_GradeTypeEntity_TypeId",
                table: "Grades",
                column: "TypeId",
                principalTable: "GradeTypeEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Subject_SubjectEntityId",
                table: "Grades",
                column: "SubjectEntityId",
                principalTable: "Subject",
                principalColumn: "Id");
        }
    }
}
