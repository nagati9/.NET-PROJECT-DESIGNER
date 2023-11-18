using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANAProjectDesigner.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProfilsfields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "phoneNumber",
                table: "Profils",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "firstName",
                table: "Profils",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Profils",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "lasttName",
                table: "Profils",
                newName: "Password");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Profils",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Profils");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Profils",
                newName: "phoneNumber");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Profils",
                newName: "firstName");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Profils",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Profils",
                newName: "lasttName");
        }
    }
}
