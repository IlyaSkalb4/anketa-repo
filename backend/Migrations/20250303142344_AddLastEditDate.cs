using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace anketa_webapi_app.Migrations
{
    /// <inheritdoc />
    public partial class AddLastEditDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastEditDate",
                table: "Questionnaires",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastEditDate",
                table: "Questionnaires");
        }
    }
}
