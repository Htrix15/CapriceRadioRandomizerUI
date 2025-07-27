using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Db.Migrations
{
    /// <inheritdoc />
    public partial class AddRemoteSources : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RemoteSources",
                columns: table => new
                {
                    Key = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    PlayLink = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    TrackInfoBaseLink = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RemoteSources", x => x.Key);
                    table.ForeignKey(
                        name: "FK_RemoteSources_Genres_Key",
                        column: x => x.Key,
                        principalTable: "Genres",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RemoteSources");
        }
    }
}
