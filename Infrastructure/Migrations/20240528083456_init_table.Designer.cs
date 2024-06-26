﻿// <auto-generated />
using System;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240528083456_init_table")]
    partial class init_table
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.BizInfo", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("basic_design")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<bool>("coding")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<DateTime>("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("created_by")
                        .HasColumnType("uuid");

                    b.Property<Guid>("cvInfoId")
                        .HasColumnType("uuid");

                    b.Property<bool>("del_flg")
                        .HasColumnType("boolean");

                    b.Property<bool>("detailed_design")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<bool>("functional_design")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("language")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<bool>("operation")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<Guid>("org_id")
                        .HasColumnType("uuid");

                    b.Property<string>("os_db")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<bool>("overview_design")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<int>("period")
                        .HasColumnType("integer");

                    b.Property<string>("prj_content")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("prj_name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<string>("role")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<bool>("system_analysis")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<bool>("unit_test")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("updated_by")
                        .HasColumnType("uuid");

                    b.HasKey("id");

                    b.HasIndex("cvInfoId");

                    b.ToTable("biz_info", "public");
                });

            modelBuilder.Entity("Domain.Entities.CvInfo", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("birthday")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("branch")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("certificate1_name")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<int?>("certificate1_year")
                        .HasColumnType("integer");

                    b.Property<string>("certificate2_name")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<int?>("certificate2_year")
                        .HasColumnType("integer");

                    b.Property<string>("certificate3_name")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<int?>("certificate3_year")
                        .HasColumnType("integer");

                    b.Property<string>("certificate4_name")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<int?>("certificate4_year")
                        .HasColumnType("integer");

                    b.Property<DateTime>("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("created_by")
                        .HasColumnType("uuid");

                    b.Property<bool>("del_flg")
                        .HasColumnType("boolean");

                    b.Property<string>("furigana")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("gender")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(1)
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.Property<int?>("graduation_year")
                        .HasColumnType("integer");

                    b.Property<bool?>("is_actived")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(1)
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<int?>("lang1_hearing")
                        .HasColumnType("integer");

                    b.Property<int?>("lang1_reading")
                        .HasColumnType("integer");

                    b.Property<int?>("lang1_speaking")
                        .HasColumnType("integer");

                    b.Property<int?>("lang1_writing")
                        .HasColumnType("integer");

                    b.Property<int?>("lang2_hearing")
                        .HasColumnType("integer");

                    b.Property<int?>("lang2_reading")
                        .HasColumnType("integer");

                    b.Property<int?>("lang2_speaking")
                        .HasColumnType("integer");

                    b.Property<int?>("lang2_writing")
                        .HasColumnType("integer");

                    b.Property<string>("last_university_name")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("note")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<Guid>("org_id")
                        .HasColumnType("uuid");

                    b.Property<string>("subject")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("updated_by")
                        .HasColumnType("uuid");

                    b.Property<string>("user_code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("work_process")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.HasKey("id");

                    b.ToTable("cv_info", "public");
                });

            modelBuilder.Entity("Domain.Entities.CvTechnicalInfo", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CvInfoId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TechnicalId")
                        .HasColumnType("uuid");

                    b.Property<int>("Value")
                        .HasColumnType("integer");

                    b.Property<DateTime>("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("created_by")
                        .HasColumnType("uuid");

                    b.Property<bool>("del_flg")
                        .HasColumnType("boolean");

                    b.Property<Guid>("org_id")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("updated_by")
                        .HasColumnType("uuid");

                    b.HasKey("id");

                    b.HasIndex("CvInfoId");

                    b.HasIndex("TechnicalId");

                    b.ToTable("cv_technical_info", "public");
                });

            modelBuilder.Entity("Domain.Entities.Function", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("code")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)");

                    b.Property<DateTime>("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("created_by")
                        .HasColumnType("uuid");

                    b.Property<bool>("del_flg")
                        .HasColumnType("boolean");

                    b.Property<string>("description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("icon")
                        .HasColumnType("text");

                    b.Property<bool>("is_active")
                        .HasColumnType("boolean");

                    b.Property<string>("method")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<string>("module")
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<int?>("order")
                        .HasColumnType("integer");

                    b.Property<string>("parent_cd")
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)");

                    b.Property<string>("path")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("updated_by")
                        .HasColumnType("uuid");

                    b.Property<string>("url")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.HasKey("id");

                    b.HasIndex("parent_cd");

                    b.ToTable("a_function", "public");
                });

            modelBuilder.Entity("Domain.Entities.LogAction", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("body")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("created_by")
                        .HasColumnType("uuid");

                    b.Property<bool>("del_flg")
                        .HasColumnType("boolean");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("method")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("updated_by")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("user_id")
                        .HasColumnType("uuid");

                    b.Property<string>("user_name")
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.HasIndex("user_id");

                    b.ToTable("m_log_action", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.LogException", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("created_by")
                        .HasColumnType("uuid");

                    b.Property<bool>("del_flg")
                        .HasColumnType("boolean");

                    b.Property<string>("function")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<string>("message")
                        .HasColumnType("text");

                    b.Property<string>("stack_trace")
                        .HasColumnType("text");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("updated_by")
                        .HasColumnType("uuid");

                    b.HasKey("id");

                    b.ToTable("log_exception", "public");
                });

            modelBuilder.Entity("Domain.Entities.MasterCode", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("created_by")
                        .HasColumnType("uuid");

                    b.Property<bool>("del_flg")
                        .HasColumnType("boolean");

                    b.Property<string>("key")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("type")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("updated_by")
                        .HasColumnType("uuid");

                    b.Property<string>("value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("m_master_code", "public");
                });

            modelBuilder.Entity("Domain.Entities.Permission", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("created_by")
                        .HasColumnType("uuid");

                    b.Property<bool>("del_flg")
                        .HasColumnType("boolean");

                    b.Property<string>("function_cd")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)");

                    b.Property<string>("role_cd")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("updated_by")
                        .HasColumnType("uuid");

                    b.HasKey("id");

                    b.HasIndex("function_cd");

                    b.HasIndex("role_cd");

                    b.ToTable("a_permission", "public");
                });

            modelBuilder.Entity("Domain.Entities.Resource", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("created_by")
                        .HasColumnType("uuid");

                    b.Property<bool>("del_flg")
                        .HasColumnType("boolean");

                    b.Property<string>("key")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("lang")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<string>("module")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("screen")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("updated_by")
                        .HasColumnType("uuid");

                    b.HasKey("id");

                    b.ToTable("m_resource", "public");
                });

            modelBuilder.Entity("Domain.Entities.Role", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("code")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)");

                    b.Property<DateTime>("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("created_by")
                        .HasColumnType("uuid");

                    b.Property<bool>("del_flg")
                        .HasColumnType("boolean");

                    b.Property<string>("description")
                        .HasMaxLength(240)
                        .HasColumnType("character varying(240)");

                    b.Property<string>("is_actived")
                        .IsRequired()
                        .HasMaxLength(1)
                        .HasColumnType("character varying(1)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(160)
                        .HasColumnType("character varying(160)");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("updated_by")
                        .HasColumnType("uuid");

                    b.HasKey("id");

                    b.ToTable("a_role", "public");
                });

            modelBuilder.Entity("Domain.Entities.Seq", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("created_by")
                        .HasColumnType("uuid");

                    b.Property<bool>("del_flg")
                        .HasColumnType("boolean");

                    b.Property<int>("no")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("updated_by")
                        .HasColumnType("uuid");

                    b.HasKey("id");

                    b.ToTable("m_seq", "public");
                });

            modelBuilder.Entity("Domain.Entities.Technical", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool?>("IsActived")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(1)
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<Guid>("TechnicalCategoryId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("created_by")
                        .HasColumnType("uuid");

                    b.Property<bool>("del_flg")
                        .HasColumnType("boolean");

                    b.Property<Guid>("org_id")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("updated_by")
                        .HasColumnType("uuid");

                    b.HasKey("id");

                    b.HasIndex("TechnicalCategoryId");

                    b.ToTable("technical", "public");
                });

            modelBuilder.Entity("Domain.Entities.TechnicalCategory", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool?>("IsActived")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(1)
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime>("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("created_by")
                        .HasColumnType("uuid");

                    b.Property<bool>("del_flg")
                        .HasColumnType("boolean");

                    b.Property<Guid>("org_id")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("updated_by")
                        .HasColumnType("uuid");

                    b.HasKey("id");

                    b.ToTable("technical_category", "public");
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("birthday")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("code")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)");

                    b.Property<DateTime>("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("created_by")
                        .HasColumnType("uuid");

                    b.Property<bool>("del_flg")
                        .HasColumnType("boolean");

                    b.Property<string>("full_name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("gender")
                        .IsRequired()
                        .HasMaxLength(1)
                        .HasColumnType("character varying(1)");

                    b.Property<string>("hashpass")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<bool?>("is_actived")
                        .HasColumnType("boolean");

                    b.Property<string>("mail")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<Guid>("org_id")
                        .HasColumnType("uuid");

                    b.Property<string>("org_info_code")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("phone")
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)");

                    b.Property<string>("salt")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("updated_by")
                        .HasColumnType("uuid");

                    b.Property<string>("user_name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("id");

                    b.ToTable("a_user", "public");
                });

            modelBuilder.Entity("Domain.Entities.UserRole", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("created_by")
                        .HasColumnType("uuid");

                    b.Property<bool>("del_flg")
                        .HasColumnType("boolean");

                    b.Property<string>("role_cd")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("updated_by")
                        .HasColumnType("uuid");

                    b.Property<string>("user_cd")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)");

                    b.HasKey("id");

                    b.HasIndex("role_cd");

                    b.HasIndex("user_cd");

                    b.ToTable("a_user_role", "public");
                });

            modelBuilder.Entity("Domain.Entities.UserToken", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("access_token")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<DateTime>("access_token_expired_date")
                        .HasColumnType("Date");

                    b.Property<DateTime>("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("created_by")
                        .HasColumnType("uuid");

                    b.Property<bool>("del_flg")
                        .HasColumnType("boolean");

                    b.Property<string>("ip")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("refresh_token")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<DateTime>("refresh_token_expired_date")
                        .HasColumnType("Date");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("updated_by")
                        .HasColumnType("uuid");

                    b.Property<string>("user_cd")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)");

                    b.HasKey("id");

                    b.HasIndex("user_cd");

                    b.ToTable("a_user_token", "public");
                });

            modelBuilder.Entity("Domain.Entities.BizInfo", b =>
                {
                    b.HasOne("Domain.Entities.CvInfo", "cvInfo")
                        .WithMany("bizInfos")
                        .HasForeignKey("cvInfoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("cvInfo");
                });

            modelBuilder.Entity("Domain.Entities.CvTechnicalInfo", b =>
                {
                    b.HasOne("Domain.Entities.CvInfo", "CvInfo")
                        .WithMany("cvTechInfos")
                        .HasForeignKey("CvInfoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Technical", "Technical")
                        .WithMany("CvTechicalInfos")
                        .HasForeignKey("TechnicalId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CvInfo");

                    b.Navigation("Technical");
                });

            modelBuilder.Entity("Domain.Entities.Function", b =>
                {
                    b.HasOne("Domain.Entities.Function", "parent")
                        .WithMany("childs")
                        .HasForeignKey("parent_cd")
                        .HasPrincipalKey("code")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("parent");
                });

            modelBuilder.Entity("Domain.Entities.LogAction", b =>
                {
                    b.HasOne("Domain.Entities.User", "user")
                        .WithMany("log_actions")
                        .HasForeignKey("user_id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("user");
                });

            modelBuilder.Entity("Domain.Entities.Permission", b =>
                {
                    b.HasOne("Domain.Entities.Function", "function")
                        .WithMany("permissions")
                        .HasForeignKey("function_cd")
                        .HasPrincipalKey("code")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Role", "role")
                        .WithMany("permissions")
                        .HasForeignKey("role_cd")
                        .HasPrincipalKey("code")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("function");

                    b.Navigation("role");
                });

            modelBuilder.Entity("Domain.Entities.Technical", b =>
                {
                    b.HasOne("Domain.Entities.TechnicalCategory", "TechnicalCategory")
                        .WithMany("Technicals")
                        .HasForeignKey("TechnicalCategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("TechnicalCategory");
                });

            modelBuilder.Entity("Domain.Entities.UserRole", b =>
                {
                    b.HasOne("Domain.Entities.Role", "role")
                        .WithMany("user_role")
                        .HasForeignKey("role_cd")
                        .HasPrincipalKey("code")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Domain.Entities.User", "user")
                        .WithMany("user_roles")
                        .HasForeignKey("user_cd")
                        .HasPrincipalKey("code")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("role");

                    b.Navigation("user");
                });

            modelBuilder.Entity("Domain.Entities.UserToken", b =>
                {
                    b.HasOne("Domain.Entities.User", "user")
                        .WithMany("user_token")
                        .HasForeignKey("user_cd")
                        .HasPrincipalKey("code")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("Domain.Entities.CvInfo", b =>
                {
                    b.Navigation("bizInfos");

                    b.Navigation("cvTechInfos");
                });

            modelBuilder.Entity("Domain.Entities.Function", b =>
                {
                    b.Navigation("childs");

                    b.Navigation("permissions");
                });

            modelBuilder.Entity("Domain.Entities.Role", b =>
                {
                    b.Navigation("permissions");

                    b.Navigation("user_role");
                });

            modelBuilder.Entity("Domain.Entities.Technical", b =>
                {
                    b.Navigation("CvTechicalInfos");
                });

            modelBuilder.Entity("Domain.Entities.TechnicalCategory", b =>
                {
                    b.Navigation("Technicals");
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Navigation("log_actions");

                    b.Navigation("user_roles");

                    b.Navigation("user_token");
                });
#pragma warning restore 612, 618
        }
    }
}
