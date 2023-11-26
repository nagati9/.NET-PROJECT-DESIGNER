using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANAProjectDesigner.Migrations
{
    /// <inheritdoc />
    public partial class AddnewtableforWorkItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ressource",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    SprintId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ressource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ressource_Sprint_SprintId",
                        column: x => x.SprintId,
                        principalTable: "Sprint",
                        principalColumn: "SprintId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SprintId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaskName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaskType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkItem_Sprint_SprintId",
                        column: x => x.SprintId,
                        principalTable: "Sprint",
                        principalColumn: "SprintId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkItemRessource",
                columns: table => new
                {
                    WorkItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RessourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OriginalEstimate = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkItemRessource", x => new { x.WorkItemId, x.RessourceId });
                    table.ForeignKey(
                        name: "FK_WorkItemRessource_Ressource_RessourceId",
                        column: x => x.RessourceId,
                        principalTable: "Ressource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_WorkItemRessource_WorkItem_WorkItemId",
                        column: x => x.WorkItemId,
                        principalTable: "WorkItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ressource_SprintId",
                table: "Ressource",
                column: "SprintId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkItem_SprintId",
                table: "WorkItem",
                column: "SprintId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkItemRessource_RessourceId",
                table: "WorkItemRessource",
                column: "RessourceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkItemRessource");

            migrationBuilder.DropTable(
                name: "Ressource");

            migrationBuilder.DropTable(
                name: "WorkItem");
        }
    }
}
