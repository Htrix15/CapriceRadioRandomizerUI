using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Db.Migrations
{
    /// <inheritdoc />
    public partial class InitDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Key = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    ItIsParent = table.Column<bool>(type: "INTEGER", nullable: false),
                    ParentGenreKey = table.Column<string>(type: "TEXT", nullable: true),
                    IsAvailable = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsDisabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSkip = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsLastChoice = table.Column<bool>(type: "INTEGER", nullable: false),
                    TrackCount = table.Column<int>(type: "INTEGER", nullable: false),
                    RatingCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Key);
                    table.ForeignKey(
                        name: "FK_Genres_Genres_ParentGenreKey",
                        column: x => x.ParentGenreKey,
                        principalTable: "Genres",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Genres_ParentGenreKey",
                table: "Genres",
                column: "ParentGenreKey");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RemoteSources");

            migrationBuilder.DropTable(
                name: "Genres");
        }
    }
}
