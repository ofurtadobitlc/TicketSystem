using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketSystem.Web.Migrations
{
    /// <inheritdoc />
    public partial class TicketAttachmentAdjusted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketAttachments_Tickets_TicketModelId",
                table: "TicketAttachments");

            migrationBuilder.DropIndex(
                name: "IX_TicketAttachments_TicketModelId",
                table: "TicketAttachments");

            migrationBuilder.DropColumn(
                name: "TicketModelId",
                table: "TicketAttachments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TicketModelId",
                table: "TicketAttachments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketAttachments_TicketModelId",
                table: "TicketAttachments",
                column: "TicketModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketAttachments_Tickets_TicketModelId",
                table: "TicketAttachments",
                column: "TicketModelId",
                principalTable: "Tickets",
                principalColumn: "Id");
        }
    }
}
