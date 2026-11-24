using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace movie_tracker.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMediaWatchlistAndReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Movies_MovieId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_WatchlistItems_Movies_MovieId",
                table: "WatchlistItems");

            migrationBuilder.DropTable(
                name: "ActorMovie");

            migrationBuilder.DropTable(
                name: "Actors");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_WatchlistItems_MovieId",
                table: "WatchlistItems");

            migrationBuilder.DropIndex(
                name: "IX_WatchlistItems_UserId_MovieId",
                table: "WatchlistItems");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_MovieId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_UserId_MovieId",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "WatchlistItems",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "Reviews",
                newName: "Type");

            migrationBuilder.AddColumn<string>(
                name: "PosterPath",
                table: "WatchlistItems",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "WatchlistItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TmdbId",
                table: "WatchlistItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TmdbId",
                table: "Reviews",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Media",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TmdbId = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    OstLinks = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WatchlistItems_UserId_TmdbId_Type",
                table: "WatchlistItems",
                columns: new[] { "UserId", "TmdbId", "Type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId_TmdbId_Type",
                table: "Reviews",
                columns: new[] { "UserId", "TmdbId", "Type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Media_TmdbId_Type",
                table: "Media",
                columns: new[] { "TmdbId", "Type" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Media");

            migrationBuilder.DropIndex(
                name: "IX_WatchlistItems_UserId_TmdbId_Type",
                table: "WatchlistItems");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_UserId_TmdbId_Type",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "PosterPath",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "TmdbId",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "TmdbId",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "WatchlistItems",
                newName: "MovieId");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Reviews",
                newName: "MovieId");

            migrationBuilder.CreateTable(
                name: "Actors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Biography = table.Column<string>(type: "TEXT", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Country = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    DurationMinutes = table.Column<int>(type: "INTEGER", nullable: true),
                    Genre = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    PosterUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Rating = table.Column<double>(type: "REAL", nullable: false),
                    ReleaseYear = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActorMovie",
                columns: table => new
                {
                    ActorsId = table.Column<int>(type: "INTEGER", nullable: false),
                    MoviesId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActorMovie", x => new { x.ActorsId, x.MoviesId });
                    table.ForeignKey(
                        name: "FK_ActorMovie_Actors_ActorsId",
                        column: x => x.ActorsId,
                        principalTable: "Actors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActorMovie_Movies_MoviesId",
                        column: x => x.MoviesId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WatchlistItems_MovieId",
                table: "WatchlistItems",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_WatchlistItems_UserId_MovieId",
                table: "WatchlistItems",
                columns: new[] { "UserId", "MovieId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_MovieId",
                table: "Reviews",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId_MovieId",
                table: "Reviews",
                columns: new[] { "UserId", "MovieId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActorMovie_MoviesId",
                table: "ActorMovie",
                column: "MoviesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Movies_MovieId",
                table: "Reviews",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WatchlistItems_Movies_MovieId",
                table: "WatchlistItems",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
