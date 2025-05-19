using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Challengers.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UseFirstNameAndLastName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Surname",
                table: "Players",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Players",
                newName: "FirstName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Players",
                newName: "Surname");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Players",
                newName: "Name");
        }
    }
}
