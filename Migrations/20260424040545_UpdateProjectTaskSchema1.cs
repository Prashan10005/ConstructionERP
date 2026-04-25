using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConstructionERP.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProjectTaskSchema1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedTo",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "CompletedBy",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "ProjectName",
                table: "ProjectTasks");

            migrationBuilder.AddColumn<int>(
                name: "AssignedToId",
                table: "ProjectTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompletedById",
                table: "ProjectTasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectID",
                table: "ProjectTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedToId",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "CompletedById",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "ProjectID",
                table: "ProjectTasks");

            migrationBuilder.AddColumn<string>(
                name: "AssignedTo",
                table: "ProjectTasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompletedBy",
                table: "ProjectTasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectName",
                table: "ProjectTasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
