using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class repairRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GradeEntity_GradeModel_GradeModelId",
                table: "GradeEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_GradeModel_Subject_SubjectEntityId",
                table: "GradeModel");

            migrationBuilder.DropForeignKey(
                name: "FK_Person_Passport_PassportId",
                table: "Person");

            migrationBuilder.DropForeignKey(
                name: "FK_Person_Student_StudentId",
                table: "Person");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissionEntity_PermissionEntity_PermissionId",
                table: "RolePermissionEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissionEntity_Role_RoleId",
                table: "RolePermissionEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoleEntity_Role_RoleId",
                table: "UserRoleEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoleEntity_User_UserId",
                table: "UserRoleEntity");

            migrationBuilder.DropIndex(
                name: "IX_Person_PassportId",
                table: "Person");

            migrationBuilder.DropIndex(
                name: "IX_Person_StudentId",
                table: "Person");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GradeEntity",
                table: "GradeEntity");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Subject");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "PassportId",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "GradeModel");

            migrationBuilder.RenameTable(
                name: "GradeEntity",
                newName: "Grade");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "UserRoleEntity",
                newName: "RoleEntityId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserRoleEntity",
                newName: "UserEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoleEntity_RoleId",
                table: "UserRoleEntity",
                newName: "IX_UserRoleEntity_RoleEntityId");

            migrationBuilder.RenameColumn(
                name: "PersonId",
                table: "Student",
                newName: "PersonEntityId");

            migrationBuilder.RenameColumn(
                name: "PermissionId",
                table: "RolePermissionEntity",
                newName: "PermissionEntityId");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "RolePermissionEntity",
                newName: "RoleEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermissionEntity_PermissionId",
                table: "RolePermissionEntity",
                newName: "IX_RolePermissionEntity_PermissionEntityId");

            migrationBuilder.RenameColumn(
                name: "PersonId",
                table: "Passport",
                newName: "PersonEntityId");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Grade",
                newName: "StudentEntityId");

            migrationBuilder.RenameColumn(
                name: "GradeModelId",
                table: "Grade",
                newName: "GradeModelEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_GradeEntity_GradeModelId",
                table: "Grade",
                newName: "IX_Grade_GradeModelEntityId");

            migrationBuilder.AlterColumn<int>(
                name: "SubjectEntityId",
                table: "GradeModel",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Grade",
                table: "Grade",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Student_PersonEntityId",
                table: "Student",
                column: "PersonEntityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Passport_PersonEntityId",
                table: "Passport",
                column: "PersonEntityId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Grade_GradeModel_GradeModelEntityId",
                table: "Grade",
                column: "GradeModelEntityId",
                principalTable: "GradeModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GradeModel_Subject_SubjectEntityId",
                table: "GradeModel",
                column: "SubjectEntityId",
                principalTable: "Subject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Passport_Person_PersonEntityId",
                table: "Passport",
                column: "PersonEntityId",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissionEntity_PermissionEntity_PermissionEntityId",
                table: "RolePermissionEntity",
                column: "PermissionEntityId",
                principalTable: "PermissionEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissionEntity_Role_RoleEntityId",
                table: "RolePermissionEntity",
                column: "RoleEntityId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Person_PersonEntityId",
                table: "Student",
                column: "PersonEntityId",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoleEntity_Role_RoleEntityId",
                table: "UserRoleEntity",
                column: "RoleEntityId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoleEntity_User_UserEntityId",
                table: "UserRoleEntity",
                column: "UserEntityId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grade_GradeModel_GradeModelEntityId",
                table: "Grade");

            migrationBuilder.DropForeignKey(
                name: "FK_GradeModel_Subject_SubjectEntityId",
                table: "GradeModel");

            migrationBuilder.DropForeignKey(
                name: "FK_Passport_Person_PersonEntityId",
                table: "Passport");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissionEntity_PermissionEntity_PermissionEntityId",
                table: "RolePermissionEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissionEntity_Role_RoleEntityId",
                table: "RolePermissionEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Person_PersonEntityId",
                table: "Student");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoleEntity_Role_RoleEntityId",
                table: "UserRoleEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoleEntity_User_UserEntityId",
                table: "UserRoleEntity");

            migrationBuilder.DropIndex(
                name: "IX_Student_PersonEntityId",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Passport_PersonEntityId",
                table: "Passport");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Grade",
                table: "Grade");

            migrationBuilder.RenameTable(
                name: "Grade",
                newName: "GradeEntity");

            migrationBuilder.RenameColumn(
                name: "RoleEntityId",
                table: "UserRoleEntity",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "UserEntityId",
                table: "UserRoleEntity",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoleEntity_RoleEntityId",
                table: "UserRoleEntity",
                newName: "IX_UserRoleEntity_RoleId");

            migrationBuilder.RenameColumn(
                name: "PersonEntityId",
                table: "Student",
                newName: "PersonId");

            migrationBuilder.RenameColumn(
                name: "PermissionEntityId",
                table: "RolePermissionEntity",
                newName: "PermissionId");

            migrationBuilder.RenameColumn(
                name: "RoleEntityId",
                table: "RolePermissionEntity",
                newName: "RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermissionEntity_PermissionEntityId",
                table: "RolePermissionEntity",
                newName: "IX_RolePermissionEntity_PermissionId");

            migrationBuilder.RenameColumn(
                name: "PersonEntityId",
                table: "Passport",
                newName: "PersonId");

            migrationBuilder.RenameColumn(
                name: "StudentEntityId",
                table: "GradeEntity",
                newName: "StudentId");

            migrationBuilder.RenameColumn(
                name: "GradeModelEntityId",
                table: "GradeEntity",
                newName: "GradeModelId");

            migrationBuilder.RenameIndex(
                name: "IX_Grade_GradeModelEntityId",
                table: "GradeEntity",
                newName: "IX_GradeEntity_GradeModelId");

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Subject",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Student",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PassportId",
                table: "Person",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StudentId",
                table: "Person",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SubjectEntityId",
                table: "GradeModel",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "GradeModel",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GradeEntity",
                table: "GradeEntity",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Person_PassportId",
                table: "Person",
                column: "PassportId");

            migrationBuilder.CreateIndex(
                name: "IX_Person_StudentId",
                table: "Person",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_GradeEntity_GradeModel_GradeModelId",
                table: "GradeEntity",
                column: "GradeModelId",
                principalTable: "GradeModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GradeModel_Subject_SubjectEntityId",
                table: "GradeModel",
                column: "SubjectEntityId",
                principalTable: "Subject",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Person_Passport_PassportId",
                table: "Person",
                column: "PassportId",
                principalTable: "Passport",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Person_Student_StudentId",
                table: "Person",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissionEntity_PermissionEntity_PermissionId",
                table: "RolePermissionEntity",
                column: "PermissionId",
                principalTable: "PermissionEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissionEntity_Role_RoleId",
                table: "RolePermissionEntity",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoleEntity_Role_RoleId",
                table: "UserRoleEntity",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoleEntity_User_UserId",
                table: "UserRoleEntity",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
