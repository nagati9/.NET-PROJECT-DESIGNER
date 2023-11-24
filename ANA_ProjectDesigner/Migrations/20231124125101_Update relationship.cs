using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANAProjectDesigner.Migrations
{
    /// <inheritdoc />
    public partial class Updaterelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sprints_Projects_ProjectId",
                table: "Sprints");

            migrationBuilder.DropIndex(
                name: "IX_Sprints_ProjectId",
                table: "Sprints");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectsId",
                table: "Sprints",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Sprints_ProjectsId",
                table: "Sprints",
                column: "ProjectsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sprints_Projects_ProjectsId",
                table: "Sprints",
                column: "ProjectsId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sprints_Projects_ProjectsId",
                table: "Sprints");

            migrationBuilder.DropIndex(
                name: "IX_Sprints_ProjectsId",
                table: "Sprints");

            migrationBuilder.DropColumn(
                name: "ProjectsId",
                table: "Sprints");

            migrationBuilder.CreateIndex(
                name: "IX_Sprints_ProjectId",
                table: "Sprints",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sprints_Projects_ProjectId",
                table: "Sprints",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
