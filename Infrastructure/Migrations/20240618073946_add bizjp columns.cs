using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class addbizjpcolumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "prj_content_jp",
                schema: "public",
                table: "biz_info",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "prj_name_jp",
                schema: "public",
                table: "biz_info",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "role_jp",
                schema: "public",
                table: "biz_info",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "prj_content_jp",
                schema: "public",
                table: "biz_info");

            migrationBuilder.DropColumn(
                name: "prj_name_jp",
                schema: "public",
                table: "biz_info");

            migrationBuilder.DropColumn(
                name: "role_jp",
                schema: "public",
                table: "biz_info");
        }
    }
}
