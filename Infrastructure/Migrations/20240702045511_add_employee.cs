using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class add_employee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "e_department",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_e_department", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "e_employee",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    employee_code = table.Column<string>(type: "text", nullable: false),
                    branch = table.Column<string>(type: "text", nullable: false),
                    full_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    initial_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    current_group = table.Column<string>(type: "text", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false),
                    company_email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    personal_email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    phone = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    birthday = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    permanent_address = table.Column<string>(type: "text", nullable: true),
                    current_address = table.Column<string>(type: "text", nullable: true),
                    id_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    date_issue = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    location_issue = table.Column<string>(type: "text", nullable: true),
                    is_married = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_e_employee", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "e_position",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_e_position", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "e_employee_department",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    employee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    department_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_e_employee_department", x => x.id);
                    table.ForeignKey(
                        name: "FK_e_employee_department_e_department_department_id",
                        column: x => x.department_id,
                        principalSchema: "public",
                        principalTable: "e_department",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_e_employee_department_e_employee_employee_id",
                        column: x => x.employee_id,
                        principalSchema: "public",
                        principalTable: "e_employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "e_timesheet",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    employee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    group = table.Column<string>(type: "text", nullable: false),
                    month_year = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    project_participation_hours = table.Column<float>(type: "real", nullable: true),
                    consumed_hours = table.Column<float>(type: "real", nullable: true),
                    late_early_departures = table.Column<int>(type: "integer", nullable: true),
                    absence_hours = table.Column<float>(type: "real", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_e_timesheet", x => x.id);
                    table.ForeignKey(
                        name: "FK_e_timesheet_e_employee_employee_id",
                        column: x => x.employee_id,
                        principalSchema: "public",
                        principalTable: "e_employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "e_employee_positon",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    employee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    position_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_e_employee_positon", x => x.id);
                    table.ForeignKey(
                        name: "FK_e_employee_positon_e_employee_employee_id",
                        column: x => x.employee_id,
                        principalSchema: "public",
                        principalTable: "e_employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_e_employee_positon_e_position_position_id",
                        column: x => x.position_id,
                        principalSchema: "public",
                        principalTable: "e_position",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_e_employee_department_department_id",
                schema: "public",
                table: "e_employee_department",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "IX_e_employee_department_employee_id",
                schema: "public",
                table: "e_employee_department",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_e_employee_positon_employee_id",
                schema: "public",
                table: "e_employee_positon",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_e_employee_positon_position_id",
                schema: "public",
                table: "e_employee_positon",
                column: "position_id");

            migrationBuilder.CreateIndex(
                name: "IX_e_timesheet_employee_id",
                schema: "public",
                table: "e_timesheet",
                column: "employee_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "e_employee_department",
                schema: "public");

            migrationBuilder.DropTable(
                name: "e_employee_positon",
                schema: "public");

            migrationBuilder.DropTable(
                name: "e_timesheet",
                schema: "public");

            migrationBuilder.DropTable(
                name: "e_department",
                schema: "public");

            migrationBuilder.DropTable(
                name: "e_position",
                schema: "public");

            migrationBuilder.DropTable(
                name: "e_employee",
                schema: "public");
        }
    }
}
