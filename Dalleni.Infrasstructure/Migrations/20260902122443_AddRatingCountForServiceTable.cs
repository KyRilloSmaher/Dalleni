using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dalleni.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRatingCountForServiceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfficialEntityInvitation_AspNetUsers_InvitedByUserId",
                table: "OfficialEntityInvitation");

            migrationBuilder.DropForeignKey(
                name: "FK_OfficialEntityInvitation_OfficialEntities_OfficialEntityId",
                table: "OfficialEntityInvitation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfficialEntityInvitation",
                table: "OfficialEntityInvitation");

            migrationBuilder.RenameTable(
                name: "OfficialEntityInvitation",
                newName: "OfficialEntityInvitations");

            migrationBuilder.RenameIndex(
                name: "IX_OfficialEntityInvitation_TokenHash",
                table: "OfficialEntityInvitations",
                newName: "IX_OfficialEntityInvitations_TokenHash");

            migrationBuilder.RenameIndex(
                name: "IX_OfficialEntityInvitation_OfficialEntityId",
                table: "OfficialEntityInvitations",
                newName: "IX_OfficialEntityInvitations_OfficialEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_OfficialEntityInvitation_InvitedByUserId",
                table: "OfficialEntityInvitations",
                newName: "IX_OfficialEntityInvitations_InvitedByUserId");

            migrationBuilder.AddColumn<int>(
                name: "RatingCount",
                table: "Services",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfficialEntityInvitations",
                table: "OfficialEntityInvitations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OfficialEntityInvitations_AspNetUsers_InvitedByUserId",
                table: "OfficialEntityInvitations",
                column: "InvitedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OfficialEntityInvitations_OfficialEntities_OfficialEntityId",
                table: "OfficialEntityInvitations",
                column: "OfficialEntityId",
                principalTable: "OfficialEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfficialEntityInvitations_AspNetUsers_InvitedByUserId",
                table: "OfficialEntityInvitations");

            migrationBuilder.DropForeignKey(
                name: "FK_OfficialEntityInvitations_OfficialEntities_OfficialEntityId",
                table: "OfficialEntityInvitations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfficialEntityInvitations",
                table: "OfficialEntityInvitations");

            migrationBuilder.DropColumn(
                name: "RatingCount",
                table: "Services");

            migrationBuilder.RenameTable(
                name: "OfficialEntityInvitations",
                newName: "OfficialEntityInvitation");

            migrationBuilder.RenameIndex(
                name: "IX_OfficialEntityInvitations_TokenHash",
                table: "OfficialEntityInvitation",
                newName: "IX_OfficialEntityInvitation_TokenHash");

            migrationBuilder.RenameIndex(
                name: "IX_OfficialEntityInvitations_OfficialEntityId",
                table: "OfficialEntityInvitation",
                newName: "IX_OfficialEntityInvitation_OfficialEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_OfficialEntityInvitations_InvitedByUserId",
                table: "OfficialEntityInvitation",
                newName: "IX_OfficialEntityInvitation_InvitedByUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfficialEntityInvitation",
                table: "OfficialEntityInvitation",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OfficialEntityInvitation_AspNetUsers_InvitedByUserId",
                table: "OfficialEntityInvitation",
                column: "InvitedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OfficialEntityInvitation_OfficialEntities_OfficialEntityId",
                table: "OfficialEntityInvitation",
                column: "OfficialEntityId",
                principalTable: "OfficialEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
