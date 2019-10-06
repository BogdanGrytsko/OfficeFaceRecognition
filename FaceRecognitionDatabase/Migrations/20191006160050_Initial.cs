using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FaceRecognitionDatabase.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImageLabels",
                columns: table => new
                {
                    ImageLabelId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Label = table.Column<string>(nullable: true),
                    Image = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageLabels", x => x.ImageLabelId);
                });

            migrationBuilder.CreateTable(
                name: "LogCommands",
                columns: table => new
                {
                    LogCommandId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartCommandDate = table.Column<DateTime>(nullable: false),
                    FinishCommandDate = table.Column<DateTime>(nullable: false),
                    CommandResult = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogCommands", x => x.LogCommandId);
                });

            migrationBuilder.CreateTable(
                name: "LogRecognitions",
                columns: table => new
                {
                    LogRecognitionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecognitionConfidance = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogRecognitions", x => x.LogRecognitionId);
                });

            migrationBuilder.CreateTable(
                name: "UserLabels",
                columns: table => new
                {
                    UserLabelId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Label = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLabels", x => x.UserLabelId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageLabels");

            migrationBuilder.DropTable(
                name: "LogCommands");

            migrationBuilder.DropTable(
                name: "LogRecognitions");

            migrationBuilder.DropTable(
                name: "UserLabels");
        }
    }
}
