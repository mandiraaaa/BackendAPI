using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendAPI.Migrations
{
    /// <inheritdoc />
    public partial class fifteenthmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "sprint_id",
                table: "Todos",
                newName: "sprint_Name");

            migrationBuilder.RenameColumn(
                name: "AssignedTo",
                table: "Todos",
                newName: "Assignee");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "sprint_Name",
                table: "Todos",
                newName: "sprint_id");

            migrationBuilder.RenameColumn(
                name: "Assignee",
                table: "Todos",
                newName: "AssignedTo");
        }
    }
}
