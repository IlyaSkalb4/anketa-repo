using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace anketa_webapi_app.Migrations
{
    /// <inheritdoc />
    public partial class MainMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerQuestion_Answers_AnswersId",
                table: "AnswerQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_AnswerQuestion_Questions_QuestionsId",
                table: "AnswerQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionQuestionnaire_Questionnaires_QuestionnairesId",
                table: "QuestionQuestionnaire");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionQuestionnaire_Questions_QuestionsId",
                table: "QuestionQuestionnaire");

            migrationBuilder.DropTable(
                name: "AnswerUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionQuestionnaire",
                table: "QuestionQuestionnaire");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnswerQuestion",
                table: "AnswerQuestion");

            migrationBuilder.RenameTable(
                name: "QuestionQuestionnaire",
                newName: "QuestionnaireQuestions");

            migrationBuilder.RenameTable(
                name: "AnswerQuestion",
                newName: "QuestionAnswers");

            migrationBuilder.RenameColumn(
                name: "HealthStatus",
                table: "Users",
                newName: "UserName");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionQuestionnaire_QuestionsId",
                table: "QuestionnaireQuestions",
                newName: "IX_QuestionnaireQuestions_QuestionsId");

            migrationBuilder.RenameIndex(
                name: "IX_AnswerQuestion_QuestionsId",
                table: "QuestionAnswers",
                newName: "IX_QuestionAnswers_QuestionsId");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "AccessFailedCount",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LockoutEnabled",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LockoutEnd",
                table: "Users",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedUserName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecurityStamp",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionnaireQuestions",
                table: "QuestionnaireQuestions",
                columns: new[] { "QuestionnairesId", "QuestionsId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionAnswers",
                table: "QuestionAnswers",
                columns: new[] { "AnswersId", "QuestionsId" });

            migrationBuilder.CreateTable(
                name: "UserAnswers",
                columns: table => new
                {
                    AnswersId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAnswers", x => new { x.AnswersId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_UserAnswers_Answers_AnswersId",
                        column: x => x.AnswersId,
                        principalTable: "Answers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAnswers_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_UsersId",
                table: "UserAnswers",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAnswers_Answers_AnswersId",
                table: "QuestionAnswers",
                column: "AnswersId",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAnswers_Questions_QuestionsId",
                table: "QuestionAnswers",
                column: "QuestionsId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionnaireQuestions_Questionnaires_QuestionnairesId",
                table: "QuestionnaireQuestions",
                column: "QuestionnairesId",
                principalTable: "Questionnaires",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionnaireQuestions_Questions_QuestionsId",
                table: "QuestionnaireQuestions",
                column: "QuestionsId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionAnswers_Answers_AnswersId",
                table: "QuestionAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionAnswers_Questions_QuestionsId",
                table: "QuestionAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionnaireQuestions_Questionnaires_QuestionnairesId",
                table: "QuestionnaireQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionnaireQuestions_Questions_QuestionsId",
                table: "QuestionnaireQuestions");

            migrationBuilder.DropTable(
                name: "UserAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionnaireQuestions",
                table: "QuestionnaireQuestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionAnswers",
                table: "QuestionAnswers");

            migrationBuilder.DropColumn(
                name: "AccessFailedCount",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LockoutEnabled",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NormalizedUserName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SecurityStamp",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "QuestionnaireQuestions",
                newName: "QuestionQuestionnaire");

            migrationBuilder.RenameTable(
                name: "QuestionAnswers",
                newName: "AnswerQuestion");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Users",
                newName: "HealthStatus");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionnaireQuestions_QuestionsId",
                table: "QuestionQuestionnaire",
                newName: "IX_QuestionQuestionnaire_QuestionsId");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionAnswers_QuestionsId",
                table: "AnswerQuestion",
                newName: "IX_AnswerQuestion_QuestionsId");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionQuestionnaire",
                table: "QuestionQuestionnaire",
                columns: new[] { "QuestionnairesId", "QuestionsId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnswerQuestion",
                table: "AnswerQuestion",
                columns: new[] { "AnswersId", "QuestionsId" });

            migrationBuilder.CreateTable(
                name: "AnswerUser",
                columns: table => new
                {
                    AnswersId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerUser", x => new { x.AnswersId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_AnswerUser_Answers_AnswersId",
                        column: x => x.AnswersId,
                        principalTable: "Answers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnswerUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerUser_UsersId",
                table: "AnswerUser",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerQuestion_Answers_AnswersId",
                table: "AnswerQuestion",
                column: "AnswersId",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerQuestion_Questions_QuestionsId",
                table: "AnswerQuestion",
                column: "QuestionsId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionQuestionnaire_Questionnaires_QuestionnairesId",
                table: "QuestionQuestionnaire",
                column: "QuestionnairesId",
                principalTable: "Questionnaires",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionQuestionnaire_Questions_QuestionsId",
                table: "QuestionQuestionnaire",
                column: "QuestionsId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
