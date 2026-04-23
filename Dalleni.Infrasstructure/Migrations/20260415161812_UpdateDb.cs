using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dalleni.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "Tags",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Tags",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UsageCount",
                table: "Tags",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuestionsCount",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AcceptedAnswersCount",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AnswersCount",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuestionsCount",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalDownVotesReceived",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalUpVotesReceived",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "UsageCount",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "QuestionsCount",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "AcceptedAnswersCount",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AnswersCount",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "QuestionsCount",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TotalDownVotesReceived",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TotalUpVotesReceived",
                table: "AspNetUsers");
        }
    }
}
