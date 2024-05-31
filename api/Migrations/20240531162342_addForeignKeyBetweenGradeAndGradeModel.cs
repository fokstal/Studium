using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class addForeignKeyBetweenGradeAndGradeModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GradeEntity_GradeModel_GradeModelEntityId",
                table: "GradeEntity");

            migrationBuilder.DropIndex(
                name: "IX_GradeEntity_GradeModelEntityId",
                table: "GradeEntity");

            migrationBuilder.DropColumn(
                name: "GradeModelEntityId",
                table: "GradeEntity");

            migrationBuilder.CreateIndex(
                name: "IX_GradeEntity_GradeModelId",
                table: "GradeEntity",
                column: "GradeModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_GradeEntity_GradeModel_GradeModelId",
                table: "GradeEntity",
                column: "GradeModelId",
                principalTable: "GradeModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GradeEntity_GradeModel_GradeModelId",
                table: "GradeEntity");

            migrationBuilder.DropIndex(
                name: "IX_GradeEntity_GradeModelId",
                table: "GradeEntity");

            migrationBuilder.AddColumn<Guid>(
                name: "GradeModelEntityId",
                table: "GradeEntity",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GradeEntity_GradeModelEntityId",
                table: "GradeEntity",
                column: "GradeModelEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_GradeEntity_GradeModel_GradeModelEntityId",
                table: "GradeEntity",
                column: "GradeModelEntityId",
                principalTable: "GradeModel",
                principalColumn: "Id");
        }
    }
}
