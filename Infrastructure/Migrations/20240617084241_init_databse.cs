using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class init_databse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "certificate1_name_jp",
                schema: "public",
                table: "cv_info",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "certificate2_name_jp",
                schema: "public",
                table: "cv_info",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "certificate3_name_jp",
                schema: "public",
                table: "cv_info",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "last_university_name_jp",
                schema: "public",
                table: "cv_info",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "note_jp",
                schema: "public",
                table: "cv_info",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "subject_jp",
                schema: "public",
                table: "cv_info",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "work_process_jp",
                schema: "public",
                table: "cv_info",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "certificate1_name_jp",
                schema: "public",
                table: "cv_info");

            migrationBuilder.DropColumn(
                name: "certificate2_name_jp",
                schema: "public",
                table: "cv_info");

            migrationBuilder.DropColumn(
                name: "certificate3_name_jp",
                schema: "public",
                table: "cv_info");

            migrationBuilder.DropColumn(
                name: "last_university_name_jp",
                schema: "public",
                table: "cv_info");

            migrationBuilder.DropColumn(
                name: "note_jp",
                schema: "public",
                table: "cv_info");

            migrationBuilder.DropColumn(
                name: "subject_jp",
                schema: "public",
                table: "cv_info");

            migrationBuilder.DropColumn(
                name: "work_process_jp",
                schema: "public",
                table: "cv_info");
        }
    }
}
