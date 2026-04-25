using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConstructionERP.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProjectTaskSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_Projects_ProjectId",
                table: "ProjectTasks");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTasks_ProjectId",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "AssignedStaffId",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "SupervisorId",
                table: "ProjectTasks");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "ProjectTasks",
                newName: "TaskName");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "ProjectTasks",
                newName: "TaskDescription");

            migrationBuilder.AddColumn<string>(
                name: "AssignedTo",
                table: "ProjectTasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "ProjectTasks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompletedBy",
                table: "ProjectTasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ProjectTasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ProjectTasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProjectName",
                table: "ProjectTasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedTo",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "CompletedBy",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "ProjectName",
                table: "ProjectTasks");

            migrationBuilder.RenameColumn(
                name: "TaskName",
                table: "ProjectTasks",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "TaskDescription",
                table: "ProjectTasks",
                newName: "Description");

            migrationBuilder.AddColumn<int>(
                name: "AssignedStaffId",
                table: "ProjectTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "ProjectTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SupervisorId",
                table: "ProjectTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_ProjectId",
                table: "ProjectTasks",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_Projects_ProjectId",
                table: "ProjectTasks",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
