using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace anketa_webapi_app.Migrations
{
    /// <inheritdoc />
    public partial class AddTypeInput : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BooleanValue",
                table: "Answers");

            migrationBuilder.AddColumn<int>(
                name: "InputType",
                table: "Answers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InputType",
                table: "Answers");

            migrationBuilder.AddColumn<bool>(
                name: "BooleanValue",
                table: "Answers",
                type: "bit",
                nullable: true);
        }
    }
}
