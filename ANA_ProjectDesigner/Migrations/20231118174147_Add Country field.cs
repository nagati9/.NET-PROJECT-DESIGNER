using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANAProjectDesigner.Migrations
{
    /// <inheritdoc />
    public partial class AddCountryfield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Profils",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Profils");
        }
    }
}
