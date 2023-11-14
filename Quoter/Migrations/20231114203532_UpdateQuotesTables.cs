using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quoter.Migrations
{
    /// <inheritdoc />
    public partial class UpdateQuotesTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_QuoteRecords",
                table: "QuoteRecords");

            migrationBuilder.RenameTable(
                name: "QuoteRecords",
                newName: "QuoteUserRecords");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuoteUserRecords",
                table: "QuoteUserRecords",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Quotes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    KeyWord = table.Column<string>(type: "TEXT", nullable: false),
                    GlobalName = table.Column<string>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: false),
                    ChannelName = table.Column<string>(type: "TEXT", nullable: false),
                    ChannelId = table.Column<string>(type: "TEXT", nullable: false),
                    GuildId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quotes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Quotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuoteUserRecords",
                table: "QuoteUserRecords");

            migrationBuilder.RenameTable(
                name: "QuoteUserRecords",
                newName: "QuoteRecords");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuoteRecords",
                table: "QuoteRecords",
                column: "Id");
        }
    }
}
