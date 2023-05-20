using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReactProject.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "https://localhost:7167/images/default.jpg"),
                    CreatedOnDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "TwitterPosts",
                columns: table => new
                {
                    TwitterPostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageURl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Shares = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "getdate()"),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitterPosts", x => x.TwitterPostId);
                    table.ForeignKey(
                        name: "FK_TwitterPosts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserFollows",
                columns: table => new
                {
                    FollowerId = table.Column<int>(type: "int", nullable: false),
                    FollowingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFollows", x => new { x.FollowerId, x.FollowingId });
                    table.ForeignKey(
                        name: "FK_UserFollows_Users_FollowerId",
                        column: x => x.FollowerId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserFollows_Users_FollowingId",
                        column: x => x.FollowingId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TwitterPostId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => new { x.UserId, x.TwitterPostId });
                    table.ForeignKey(
                        name: "FK_Likes_TwitterPosts_TwitterPostId",
                        column: x => x.TwitterPostId,
                        principalTable: "TwitterPosts",
                        principalColumn: "TwitterPostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Likes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedOnDate", "Email", "Password", "UserName" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Test@Test.com", "TestPassword", "MissUser" },
                    { 2, new DateTime(2023, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Test@Test.com", "TestPassword", "MisterUser" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedOnDate", "Email", "ImageUrl", "Password", "UserName" },
                values: new object[] { 3, new DateTime(2023, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Test@Test.com", "https://localhost:7167/images/cat.png", "TestPassword", "TestUser" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedOnDate", "Email", "Password", "UserName" },
                values: new object[] { 4, new DateTime(2023, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Test@Test.com", "TestPassword", "Miss4User" });

            migrationBuilder.InsertData(
                table: "TwitterPosts",
                columns: new[] { "TwitterPostId", "ImageURl", "Shares", "Text", "UserId" },
                values: new object[,]
                {
                    { 1, "https://localhost:7167/images/cat.png", 0, "Hello world!", 1 },
                    { 2, "https://localhost:7167/images/cat.png", 0, "Hello 23 world!", 3 },
                    { 3, "https://localhost:7167/images/cat.png", 0, "Hello 3 world!", 2 }
                });

            migrationBuilder.InsertData(
                table: "UserFollows",
                columns: new[] { "FollowerId", "FollowingId" },
                values: new object[,]
                {
                    { 1, 2 },
                    { 1, 3 },
                    { 4, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Likes_TwitterPostId",
                table: "Likes",
                column: "TwitterPostId");

            migrationBuilder.CreateIndex(
                name: "IX_TwitterPosts_UserId",
                table: "TwitterPosts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFollows_FollowingId",
                table: "UserFollows",
                column: "FollowingId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.DropTable(
                name: "UserFollows");

            migrationBuilder.DropTable(
                name: "TwitterPosts");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
