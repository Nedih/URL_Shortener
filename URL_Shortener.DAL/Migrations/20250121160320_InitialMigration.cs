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
                    UrlId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    { "12063cdc-e45b-4f66-bd3f-119a7a4a8c5c", 0, "d30fa888-b59c-4c07-9d40-a87296428505", "UserAccount", "admin1@admin.com", false, false, null, "Admin3", "ADMIN1@ADMIN.COM", "ADMIN1@ADMIN.COM", "AQAAAAIAAYagAAAAENT9FNckgudXJFOnbCb5kcmgKzupS4qRvVzKrqGiuFYRRFN66Y5LX7wOMW8L+0oOHA==", null, false, "e405c43b-4c6d-432e-931b-a8c9aba95133", false, "admin1@admin.com" },
                    { "17643fd0-ade1-4c40-ba60-07758bcead9e", 0, "8baf08d2-fc98-43be-8a92-e13b7c9c300f", "UserAccount", "admin3@admin.com", false, false, null, "Admin3", "ADMIN3@ADMIN.COM", "ADMIN3@ADMIN.COM", "AQAAAAIAAYagAAAAEEVN5b4myDK37yauuHnuA0sndwPiwPHxDnaDwT3XivZTaomgJI+aE/+3aACYyOXayA==", null, false, "1b5ebef1-689b-49d0-881e-8278c64b7398", false, "admin3@admin.com" },
                    { "237aa5f1-a53c-4828-ba17-4820e987bba2", 0, "8d3f99a6-65f6-4ea9-830a-895a380ba468", "UserAccount", "user3@user.com", false, false, null, "User1", "USER3@USER.COM", "USER3@USER.COM", "AQAAAAIAAYagAAAAEMAKgF0b12DMsWhMAg/TDCdFCz+TsmvuiXbXaYc9i/GFH/s9Ut1bmrzZ6cnUY0gQvA==", null, false, "bc51601d-1b79-4319-bbe9-cb8fdeae8fb7", false, "user3@user.com" },
                    { "23bf6bff-f538-4d72-b79a-6efbf0dc9965", 0, "3f3027c0-a6ff-40f1-83a9-ded5533e5d59", "UserAccount", "user1@user.com", false, false, null, "User1", "USER1@USER.COM", "USER1@USER.COM", "AQAAAAIAAYagAAAAEAQcZd8ALkxLlNdz1cwFpYl5/xQBZo9cLPxSuXPUpPk1P/5NFnQV7q3pujjlXFykyQ==", null, false, "49b998be-94ea-452c-bf1e-3d42df643758", false, "user1@user.com" },
                    { "26bf362a-f68a-4f5c-8021-2e1c5557a679", 0, "5568cc69-9041-41e6-89c4-53ef862e5dab", "UserAccount", "admin2@admin.com", false, false, null, "Admin3", "ADMIN2@ADMIN.COM", "ADMIN2@ADMIN.COM", "AQAAAAIAAYagAAAAEIZQvfnd4AKhUWOr/k67RsRq3pW0l1ujaX0WyzCR7Tj5rqJoLrLKtUcE1y7TC3XhWw==", null, false, "731aa9b4-2b53-436c-a165-a688448dab07", false, "admin2@admin.com" },
                    { "5a69fcba-2888-437b-9bd9-777e94f13d94", 0, "79ffcf97-3472-4cfa-91f2-1837a90dd83f", "UserAccount", "user2@user.com", false, false, null, "User1", "USER2@USER.COM", "USER2@USER.COM", "AQAAAAIAAYagAAAAEN3xavumcyiSPoA45UihuZDzistz2Oo0KSn4zAppNBe1Dy5bCkN2mrpiGYUnjd/YSQ==", null, false, "1b34b3ba-246a-48a7-a9c5-4f9f9354b9d3", false, "user2@user.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "1c1ebabd-6745-4fc2-808d-48df8107736c", "12063cdc-e45b-4f66-bd3f-119a7a4a8c5c" },
                    { "1c1ebabd-6745-4fc2-808d-48df8107736c", "17643fd0-ade1-4c40-ba60-07758bcead9e" },
                    { "1c1ebabd-6745-4fc2-808d-48df8107736c", "237aa5f1-a53c-4828-ba17-4820e987bba2" },
                    { "1c1ebabd-6745-4fc2-808d-48df8107736c", "23bf6bff-f538-4d72-b79a-6efbf0dc9965" },
                    { "1c1ebabd-6745-4fc2-808d-48df8107736c", "26bf362a-f68a-4f5c-8021-2e1c5557a679" },
                    { "1c1ebabd-6745-4fc2-808d-48df8107736c", "5a69fcba-2888-437b-9bd9-777e94f13d94" }
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
