using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class removeNullInSubjectGradeEntityId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GradeModel_GradeType_TypeId",
                table: "GradeModel");

            migrationBuilder.DropForeignKey(
                name: "FK_Subject_Group_GroupEntityId",
                table: "Subject");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "GradeModel",
                newName: "TypeEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_GradeModel_TypeId",
                table: "GradeModel",
                newName: "IX_GradeModel_TypeEntityId");

            migrationBuilder.AlterColumn<int>(
                name: "GroupEntityId",
                table: "Subject",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GradeModel_GradeType_TypeEntityId",
                table: "GradeModel",
                column: "TypeEntityId",
                principalTable: "GradeType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subject_Group_GroupEntityId",
                table: "Subject",
                column: "GroupEntityId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GradeModel_GradeType_TypeEntityId",
                table: "GradeModel");

            migrationBuilder.DropForeignKey(
                name: "FK_Subject_Group_GroupEntityId",
                table: "Subject");

            migrationBuilder.RenameColumn(
                name: "TypeEntityId",
                table: "GradeModel",
                newName: "TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_GradeModel_TypeEntityId",
                table: "GradeModel",
                newName: "IX_GradeModel_TypeId");

            migrationBuilder.AlterColumn<int>(
                name: "GroupEntityId",
                table: "Subject",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_GradeModel_GradeType_TypeId",
                table: "GradeModel",
                column: "TypeId",
                principalTable: "GradeType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subject_Group_GroupEntityId",
                table: "Subject",
                column: "GroupEntityId",
                principalTable: "Group",
                principalColumn: "Id");
        }
    }
}
