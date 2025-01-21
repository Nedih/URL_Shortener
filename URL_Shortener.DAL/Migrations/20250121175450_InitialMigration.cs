using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace URL_Shortener.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Urls",
                columns: table => new
                {
                    UrlId = table.Column<long>(type: "bigint", nullable: false),
                    UrlText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortenUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UrlCreationDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UrlDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserAccountId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Urls", x => x.UrlId);
                    table.ForeignKey(
                        name: "FK_Urls_AspNetUsers_UserAccountId",
                        column: x => x.UserAccountId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1c1ebabd-6745-4fc2-808d-48df8107736c", "2", "User", "User" },
                    { "47e17bad-e591-4084-b31c-40c1e4859bd7", "1", "Admin", "Admin" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "09094db5-18cb-458b-a692-593a49be22ac", 0, "3807a0de-15b1-4a56-877d-882a7b38c48b", "UserAccount", "admin1@admin.com", false, false, null, "Admin1", "ADMIN1@ADMIN.COM", "ADMIN1@ADMIN.COM", "AQAAAAIAAYagAAAAEA5dfAzAOBCmSM0PnEkF6Ea4xMmr70tQ8z0khnUDdwpJHmhRkv9ZExJsPGa8Xc7RWA==", null, false, "3fa422ec-6981-448c-8d68-a432a5b2f46f", false, "admin1@admin.com" },
                    { "393560c1-08a4-432e-aea3-efaf5015b512", 0, "de308398-36be-4af5-9a5d-5a62b074871f", "UserAccount", "user1@user.com", false, false, null, "User1", "USER1@USER.COM", "USER1@USER.COM", "AQAAAAIAAYagAAAAEAwYIJ/vqG1LOif3jAUh5pCO6n/kZewBJ1H23vTIpbn9UgXyByK+06FqK1Qxz6Zcdw==", null, false, "b1c632f4-1a77-4de9-bbe2-4a0a308bfb4b", false, "user1@user.com" },
                    { "6a752942-d533-4e69-a832-a6fc7c975518", 0, "4af03a00-e5b9-45c8-b645-f92b825da0fd", "UserAccount", "user2@user.com", false, false, null, "User2", "USER2@USER.COM", "USER2@USER.COM", "AQAAAAIAAYagAAAAEPwAq9fKJ+PWrLVAeNzKxdi3Nf3gZIEzYmwuj39hfQY9G6jBpyTAFvZdqxo74nV+vw==", null, false, "bce45702-7c7a-4575-ac82-50a0ba526b3e", false, "user2@user.com" },
                    { "81f99f48-c0bc-457d-ba51-f8a0c7114145", 0, "af7c7e7c-f0a9-4810-96c8-68b0e99ea886", "UserAccount", "user3@user.com", false, false, null, "User3", "USER3@USER.COM", "USER3@USER.COM", "AQAAAAIAAYagAAAAECxu+Dhn4/HrbRegeidb307cYLqtMPIpH0ADNSWi4RUQua8Y4Qex8Fze1useIgQzeQ==", null, false, "74b886dc-d199-4acb-945b-a5b703de01ad", false, "user3@user.com" },
                    { "98c4f332-e1fe-42ae-8ec3-37d2ea1a407b", 0, "cde941c9-a1f6-4c32-8901-124ffa560b9d", "UserAccount", "admin2@admin.com", false, false, null, "Admin2", "ADMIN2@ADMIN.COM", "ADMIN2@ADMIN.COM", "AQAAAAIAAYagAAAAEILTweExGcKkHYRqhtOah/lGWUITcgKzGhXcqKh+ayKjyzJCunn1HECpvIYyo6Ex3Q==", null, false, "73ec830a-81d3-492a-8283-35256b8d0cab", false, "admin2@admin.com" },
                    { "be1fff33-2672-4ae4-9a4e-72143b5a9afa", 0, "4abc7c19-f5b6-47f3-a9d8-cc96d1c3fc3e", "UserAccount", "admin3@admin.com", false, false, null, "Admin3", "ADMIN3@ADMIN.COM", "ADMIN3@ADMIN.COM", "AQAAAAIAAYagAAAAENmQ63X07hx7tuJV4mMxFhMGN5qknmHGTFOUoAgcYYKFN9lPkgx0KNppsnbDtT6TzA==", null, false, "6b047098-16a6-48b8-8bff-34160ebe6d37", false, "admin3@admin.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "1c1ebabd-6745-4fc2-808d-48df8107736c", "09094db5-18cb-458b-a692-593a49be22ac" },
                    { "1c1ebabd-6745-4fc2-808d-48df8107736c", "393560c1-08a4-432e-aea3-efaf5015b512" },
                    { "1c1ebabd-6745-4fc2-808d-48df8107736c", "6a752942-d533-4e69-a832-a6fc7c975518" },
                    { "1c1ebabd-6745-4fc2-808d-48df8107736c", "81f99f48-c0bc-457d-ba51-f8a0c7114145" },
                    { "1c1ebabd-6745-4fc2-808d-48df8107736c", "98c4f332-e1fe-42ae-8ec3-37d2ea1a407b" },
                    { "1c1ebabd-6745-4fc2-808d-48df8107736c", "be1fff33-2672-4ae4-9a4e-72143b5a9afa" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Urls_UserAccountId",
                table: "Urls",
                column: "UserAccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Urls");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
