using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnsOfStatusForProjectAndTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TaskStatus",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProjectStatus",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_PublicId",
                table: "Tasks",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_PublicId",
                table: "Projects",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PublicId",
                table: "Comments",
                column: "PublicId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tasks_PublicId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Projects_PublicId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Comments_PublicId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "TaskStatus",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ProjectStatus",
                table: "Projects");
        }
    }
}
