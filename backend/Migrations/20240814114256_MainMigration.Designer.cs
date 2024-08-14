﻿// <auto-generated />
using System;
using AnketaDatabaseLibrary.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace anketa_webapi_app.Migrations
{
    [DbContext(typeof(AnketaDatabaseContext))]
    [Migration("20240814114256_MainMigration")]
    partial class MainMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AnswerApplicationUser", b =>
                {
                    b.Property<int>("AnswersId")
                        .HasColumnType("int");

                    b.Property<string>("UsersId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("AnswersId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("UserAnswers", (string)null);
                });

            modelBuilder.Entity("AnswerQuestion", b =>
                {
                    b.Property<int>("AnswersId")
                        .HasColumnType("int");

                    b.Property<int>("QuestionsId")
                        .HasColumnType("int");

                    b.HasKey("AnswersId", "QuestionsId");

                    b.HasIndex("QuestionsId");

                    b.ToTable("QuestionAnswers", (string)null);
                });

            modelBuilder.Entity("QuestionQuestionnaire", b =>
                {
                    b.Property<int>("QuestionnairesId")
                        .HasColumnType("int");

                    b.Property<int>("QuestionsId")
                        .HasColumnType("int");

                    b.HasKey("QuestionnairesId", "QuestionsId");

                    b.HasIndex("QuestionsId");

                    b.ToTable("QuestionnaireQuestions", (string)null);
                });

            modelBuilder.Entity("backend.Data.Models.Answer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool?>("BooleanValue")
                        .HasColumnType("bit")
                        .HasColumnName("BooleanValue");

                    b.Property<float?>("FloatValue")
                        .HasColumnType("real")
                        .HasColumnName("FloatValue");

                    b.Property<string>("TextValue")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("TextValue");

                    b.HasKey("Id");

                    b.ToTable("Answers", (string)null);
                });

            modelBuilder.Entity("backend.Data.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("Id");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<int?>("Age")
                        .HasColumnType("int")
                        .HasColumnName("Age");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateOfChange")
                        .HasColumnType("datetime2")
                        .HasColumnName("DateOfChange");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Email");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Height")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Height");

                    b.Property<string>("LastLocation")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("LastLocation");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Name");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sex")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Sex");

                    b.Property<string>("Surname")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Surname");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Weight")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Weight");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("backend.Data.Models.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("QuestionText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("QuestionText");

                    b.HasKey("Id");

                    b.ToTable("Questions", (string)null);
                });

            modelBuilder.Entity("backend.Data.Models.Questionnaire", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateOfCreation")
                        .HasColumnType("datetime2")
                        .HasColumnName("DateOfCreation");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Description");

                    b.Property<int>("NumberOfPasses")
                        .HasColumnType("int")
                        .HasColumnName("NumberOfPasses");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Title");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Questionnaires", (string)null);
                });

            modelBuilder.Entity("AnswerApplicationUser", b =>
                {
                    b.HasOne("backend.Data.Models.Answer", null)
                        .WithMany()
                        .HasForeignKey("AnswersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("backend.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AnswerQuestion", b =>
                {
                    b.HasOne("backend.Data.Models.Answer", null)
                        .WithMany()
                        .HasForeignKey("AnswersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("backend.Data.Models.Question", null)
                        .WithMany()
                        .HasForeignKey("QuestionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("QuestionQuestionnaire", b =>
                {
                    b.HasOne("backend.Data.Models.Questionnaire", null)
                        .WithMany()
                        .HasForeignKey("QuestionnairesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("backend.Data.Models.Question", null)
                        .WithMany()
                        .HasForeignKey("QuestionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("backend.Data.Models.Questionnaire", b =>
                {
                    b.HasOne("backend.Data.Models.ApplicationUser", "User")
                        .WithMany("Questionnaires")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("backend.Data.Models.ApplicationUser", b =>
                {
                    b.Navigation("Questionnaires");
                });
#pragma warning restore 612, 618
        }
    }
}
