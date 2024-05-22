using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class init_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "a_function",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    module = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    code = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    url = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    path = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    method = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    parent_cd = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    order = table.Column<int>(type: "integer", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    icon = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_function", x => x.id);
                    table.UniqueConstraint("AK_a_function_code", x => x.code);
                    table.ForeignKey(
                        name: "FK_a_function_a_function_parent_cd",
                        column: x => x.parent_cd,
                        principalSchema: "public",
                        principalTable: "a_function",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "a_role",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    name = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    description = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: true),
                    is_actived = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_role", x => x.id);
                    table.UniqueConstraint("AK_a_role_code", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "a_user",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    full_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    user_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    gender = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    birthday = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    org_info_code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    hashpass = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    salt = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    mail = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    phone = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    is_actived = table.Column<bool>(type: "boolean", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false),
                    org_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_user", x => x.id);
                    table.UniqueConstraint("AK_a_user_code", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "cv_info",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    furigana = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    is_actived = table.Column<bool>(type: "boolean", maxLength: 1, nullable: true, defaultValue: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    gender = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false, defaultValue: "0"),
                    birthday = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    last_university_name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    subject = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    graduation_year = table.Column<int>(type: "integer", nullable: true),
                    lang1_hearing = table.Column<int>(type: "integer", nullable: true),
                    lang1_speaking = table.Column<int>(type: "integer", nullable: true),
                    lang1_reading = table.Column<int>(type: "integer", nullable: true),
                    lang1_writing = table.Column<int>(type: "integer", nullable: true),
                    lang2_hearing = table.Column<int>(type: "integer", nullable: true),
                    lang2_speaking = table.Column<int>(type: "integer", nullable: true),
                    lang2_reading = table.Column<int>(type: "integer", nullable: true),
                    lang2_writing = table.Column<int>(type: "integer", nullable: true),
                    certificate1_name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    certificate1_year = table.Column<int>(type: "integer", nullable: true),
                    certificate2_name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    certificate2_year = table.Column<int>(type: "integer", nullable: true),
                    certificate3_name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    certificate3_year = table.Column<int>(type: "integer", nullable: true),
                    certificate4_name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    certificate4_year = table.Column<int>(type: "integer", nullable: true),
                    work_process = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    note = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false),
                    org_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cv_info", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "log_exception",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    function = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    message = table.Column<string>(type: "text", nullable: true),
                    stack_trace = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_log_exception", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "m_master_code",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    key = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    value = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_master_code", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "m_resource",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    lang = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    module = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    screen = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    key = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    text = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_resource", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "m_seq",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    no = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_seq", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "technical_category",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsActived = table.Column<bool>(type: "boolean", maxLength: 1, nullable: true, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false),
                    org_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_technical_category", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "a_permission",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_cd = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    function_cd = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_permission", x => x.id);
                    table.ForeignKey(
                        name: "FK_a_permission_a_function_function_cd",
                        column: x => x.function_cd,
                        principalSchema: "public",
                        principalTable: "a_function",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_a_permission_a_role_role_cd",
                        column: x => x.role_cd,
                        principalSchema: "public",
                        principalTable: "a_role",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "a_user_role",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_cd = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    role_cd = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_user_role", x => x.id);
                    table.ForeignKey(
                        name: "FK_a_user_role_a_role_role_cd",
                        column: x => x.role_cd,
                        principalSchema: "public",
                        principalTable: "a_role",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_a_user_role_a_user_user_cd",
                        column: x => x.user_cd,
                        principalSchema: "public",
                        principalTable: "a_user",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "a_user_token",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_cd = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    access_token = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    refresh_token = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    access_token_expired_date = table.Column<DateTime>(type: "Date", nullable: false),
                    refresh_token_expired_date = table.Column<DateTime>(type: "Date", nullable: false),
                    ip = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_a_user_token", x => x.id);
                    table.ForeignKey(
                        name: "FK_a_user_token_a_user_user_cd",
                        column: x => x.user_cd,
                        principalSchema: "public",
                        principalTable: "a_user",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "m_log_action",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    user_name = table.Column<string>(type: "text", nullable: true),
                    method = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    body = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_log_action", x => x.id);
                    table.ForeignKey(
                        name: "FK_m_log_action_a_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "public",
                        principalTable: "a_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "biz_info",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    prj_name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    prj_content = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    period = table.Column<int>(type: "integer", nullable: false),
                    system_analysis = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    overview_design = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    basic_design = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    functional_design = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    detailed_design = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    coding = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    unit_test = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    operation = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    os_db = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    language = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    cvInfoId = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false),
                    org_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_biz_info", x => x.id);
                    table.ForeignKey(
                        name: "FK_biz_info_cv_info_cvInfoId",
                        column: x => x.cvInfoId,
                        principalSchema: "public",
                        principalTable: "cv_info",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "technical",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsActived = table.Column<bool>(type: "boolean", maxLength: 1, nullable: true, defaultValue: false),
                    TechnicalCategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false),
                    org_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_technical", x => x.id);
                    table.ForeignKey(
                        name: "FK_technical_technical_category_TechnicalCategoryId",
                        column: x => x.TechnicalCategoryId,
                        principalSchema: "public",
                        principalTable: "technical_category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cv_technical_info",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    CvInfoId = table.Column<Guid>(type: "uuid", nullable: false),
                    TechnicalId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    del_flg = table.Column<bool>(type: "boolean", nullable: false),
                    org_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cv_technical_info", x => x.id);
                    table.ForeignKey(
                        name: "FK_cv_technical_info_cv_info_CvInfoId",
                        column: x => x.CvInfoId,
                        principalSchema: "public",
                        principalTable: "cv_info",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_cv_technical_info_technical_TechnicalId",
                        column: x => x.TechnicalId,
                        principalSchema: "public",
                        principalTable: "technical",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_a_function_parent_cd",
                schema: "public",
                table: "a_function",
                column: "parent_cd");

            migrationBuilder.CreateIndex(
                name: "IX_a_permission_function_cd",
                schema: "public",
                table: "a_permission",
                column: "function_cd");

            migrationBuilder.CreateIndex(
                name: "IX_a_permission_role_cd",
                schema: "public",
                table: "a_permission",
                column: "role_cd");

            migrationBuilder.CreateIndex(
                name: "IX_a_user_role_role_cd",
                schema: "public",
                table: "a_user_role",
                column: "role_cd");

            migrationBuilder.CreateIndex(
                name: "IX_a_user_role_user_cd",
                schema: "public",
                table: "a_user_role",
                column: "user_cd");

            migrationBuilder.CreateIndex(
                name: "IX_a_user_token_user_cd",
                schema: "public",
                table: "a_user_token",
                column: "user_cd");

            migrationBuilder.CreateIndex(
                name: "IX_biz_info_cvInfoId",
                schema: "public",
                table: "biz_info",
                column: "cvInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_cv_technical_info_CvInfoId",
                schema: "public",
                table: "cv_technical_info",
                column: "CvInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_cv_technical_info_TechnicalId",
                schema: "public",
                table: "cv_technical_info",
                column: "TechnicalId");

            migrationBuilder.CreateIndex(
                name: "IX_m_log_action_user_id",
                table: "m_log_action",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_technical_TechnicalCategoryId",
                schema: "public",
                table: "technical",
                column: "TechnicalCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "a_permission",
                schema: "public");

            migrationBuilder.DropTable(
                name: "a_user_role",
                schema: "public");

            migrationBuilder.DropTable(
                name: "a_user_token",
                schema: "public");

            migrationBuilder.DropTable(
                name: "biz_info",
                schema: "public");

            migrationBuilder.DropTable(
                name: "cv_technical_info",
                schema: "public");

            migrationBuilder.DropTable(
                name: "log_exception",
                schema: "public");

            migrationBuilder.DropTable(
                name: "m_log_action");

            migrationBuilder.DropTable(
                name: "m_master_code",
                schema: "public");

            migrationBuilder.DropTable(
                name: "m_resource",
                schema: "public");

            migrationBuilder.DropTable(
                name: "m_seq",
                schema: "public");

            migrationBuilder.DropTable(
                name: "a_function",
                schema: "public");

            migrationBuilder.DropTable(
                name: "a_role",
                schema: "public");

            migrationBuilder.DropTable(
                name: "cv_info",
                schema: "public");

            migrationBuilder.DropTable(
                name: "technical",
                schema: "public");

            migrationBuilder.DropTable(
                name: "a_user",
                schema: "public");

            migrationBuilder.DropTable(
                name: "technical_category",
                schema: "public");
        }
    }
}
