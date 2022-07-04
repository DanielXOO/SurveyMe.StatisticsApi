using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Statistics.Data.EFCore.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SurveyStatistics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SurveyId = table.Column<Guid>(type: "uuid", nullable: false),
                    AnswersCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyStatistics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BaseQuestionStatistics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionType = table.Column<int>(type: "integer", nullable: false),
                    AnswersCount = table.Column<int>(type: "integer", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    SurveyStatisticsId = table.Column<Guid>(type: "uuid", nullable: false),
                    AverageRate = table.Column<double>(type: "double precision", nullable: true),
                    AverageScale = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseQuestionStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseQuestionStatistics_SurveyStatistics_SurveyStatisticsId",
                        column: x => x.SurveyStatisticsId,
                        principalTable: "SurveyStatistics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonalityInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SurveyStatisticsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalityInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalityInfo_SurveyStatistics_SurveyStatisticsId",
                        column: x => x.SurveyStatisticsId,
                        principalTable: "SurveyStatistics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CheckboxOptionAnswerStatistics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    AnswersCount = table.Column<int>(type: "integer", nullable: false),
                    CheckboxQuestionStatisticsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckboxOptionAnswerStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckboxOptionAnswerStatistics_BaseQuestionStatistics_Check~",
                        column: x => x.CheckboxQuestionStatisticsId,
                        principalTable: "BaseQuestionStatistics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RadioOptionAnswerStatistics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    AnswersCount = table.Column<int>(type: "integer", nullable: false),
                    RadioQuestionStatisticsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RadioOptionAnswerStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RadioOptionAnswerStatistics_BaseQuestionStatistics_RadioQue~",
                        column: x => x.RadioQuestionStatisticsId,
                        principalTable: "BaseQuestionStatistics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaseQuestionStatistics_SurveyStatisticsId",
                table: "BaseQuestionStatistics",
                column: "SurveyStatisticsId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckboxOptionAnswerStatistics_CheckboxQuestionStatisticsId",
                table: "CheckboxOptionAnswerStatistics",
                column: "CheckboxQuestionStatisticsId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalityInfo_SurveyStatisticsId",
                table: "PersonalityInfo",
                column: "SurveyStatisticsId");

            migrationBuilder.CreateIndex(
                name: "IX_RadioOptionAnswerStatistics_RadioQuestionStatisticsId",
                table: "RadioOptionAnswerStatistics",
                column: "RadioQuestionStatisticsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckboxOptionAnswerStatistics");

            migrationBuilder.DropTable(
                name: "PersonalityInfo");

            migrationBuilder.DropTable(
                name: "RadioOptionAnswerStatistics");

            migrationBuilder.DropTable(
                name: "BaseQuestionStatistics");

            migrationBuilder.DropTable(
                name: "SurveyStatistics");
        }
    }
}
