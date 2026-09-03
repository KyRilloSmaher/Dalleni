using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dalleni.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOfficialEntityMembershipAndInvitation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfficialEntities_AspNetUsers_UserId",
                table: "OfficialEntities");

            migrationBuilder.DropIndex(
                name: "IX_OfficialEntities_UserId",
                table: "OfficialEntities");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "OfficialEntities");

            migrationBuilder.AddColumn<double>(
                name: "AverageRating",
                table: "Services",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "Services",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Services",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "OfficialEntities",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000);

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "OfficialEntities",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WebsiteUrl",
                table: "OfficialEntities",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OfficialEntityId",
                table: "Answers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Answers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "OfficialEntityInvitation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OfficialEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvitedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    TokenHash = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficialEntityInvitation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfficialEntityInvitation_AspNetUsers_InvitedByUserId",
                        column: x => x.InvitedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OfficialEntityInvitation_OfficialEntities_OfficialEntityId",
                        column: x => x.OfficialEntityId,
                        principalTable: "OfficialEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OfficialEntityMembership",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OfficialEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficialEntityMembership", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfficialEntityMembership_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OfficialEntityMembership_OfficialEntities_OfficialEntityId",
                        column: x => x.OfficialEntityId,
                        principalTable: "OfficialEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ratings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ratings_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Services_CategoryId",
                table: "Services",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficialEntities_Name",
                table: "OfficialEntities",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Answers_OfficialEntityId",
                table: "Answers",
                column: "OfficialEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficialEntityInvitation_InvitedByUserId",
                table: "OfficialEntityInvitation",
                column: "InvitedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficialEntityInvitation_OfficialEntityId",
                table: "OfficialEntityInvitation",
                column: "OfficialEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficialEntityInvitation_TokenHash",
                table: "OfficialEntityInvitation",
                column: "TokenHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OfficialEntityMembership_OfficialEntityId",
                table: "OfficialEntityMembership",
                column: "OfficialEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficialEntityMembership_UserId_OfficialEntityId",
                table: "OfficialEntityMembership",
                columns: new[] { "UserId", "OfficialEntityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_CreatedAt",
                table: "Ratings",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_ServiceId",
                table: "Ratings",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserId",
                table: "Ratings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_OfficialEntities_OfficialEntityId",
                table: "Answers",
                column: "OfficialEntityId",
                principalTable: "OfficialEntities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Categories_CategoryId",
                table: "Services",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_OfficialEntities_OfficialEntityId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Categories_CategoryId",
                table: "Services");

            migrationBuilder.DropTable(
                name: "OfficialEntityInvitation");

            migrationBuilder.DropTable(
                name: "OfficialEntityMembership");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Services_CategoryId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_OfficialEntities_Name",
                table: "OfficialEntities");

            migrationBuilder.DropIndex(
                name: "IX_Answers_OfficialEntityId",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "AverageRating",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "OfficialEntities");

            migrationBuilder.DropColumn(
                name: "WebsiteUrl",
                table: "OfficialEntities");

            migrationBuilder.DropColumn(
                name: "OfficialEntityId",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Answers");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "OfficialEntities",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "OfficialEntities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_OfficialEntities_UserId",
                table: "OfficialEntities",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OfficialEntities_AspNetUsers_UserId",
                table: "OfficialEntities",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
