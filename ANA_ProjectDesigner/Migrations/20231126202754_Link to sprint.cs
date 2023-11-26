using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANAProjectDesigner.Migrations
{
    /// <inheritdoc />
    public partial class Linktosprint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SprintId",
                table: "WorkItemRessource",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"))
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.CreateIndex(
                name: "IX_WorkItemRessource_SprintId",
                table: "WorkItemRessource",
                column: "SprintId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItemRessource_Sprint_SprintId",
                table: "WorkItemRessource",
                column: "SprintId",
                principalTable: "Sprint",
                principalColumn: "SprintId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkItemRessource_Sprint_SprintId",
                table: "WorkItemRessource");

            migrationBuilder.DropIndex(
                name: "IX_WorkItemRessource_SprintId",
                table: "WorkItemRessource");

            migrationBuilder.DropColumn(
                name: "SprintId",
                table: "WorkItemRessource");
        }
    }
}
