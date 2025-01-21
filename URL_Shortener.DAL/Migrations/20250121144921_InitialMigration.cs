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
                    { "0a6a73d6-3e34-4a43-90c3-5e78a3d984b2", 0, "73e2a513-f136-4008-904c-f424d2e197ae", "UserAccount", "user2@user.com", false, false, null, "User2", null, null, "AQAAAAIAAYagAAAAENoKMtG3IAQ4/7Tx8tjlO09Xo/dnnTd7tO1qKsHlKQdixdIpT8/sII4/CjFVwv5xJA==", null, false, "88d9db33-45ab-4aa7-b795-88bae1e5e8f1", false, "User2" },
                    { "0ae67c27-0915-4f7a-8385-38850e6f8f3d", 0, "c5cd98f9-f581-42fb-bc05-8c024ba31486", "UserAccount", "admin2@user.com", false, false, null, "Admin2", null, null, "AQAAAAIAAYagAAAAEKLL9oZvNRbWkrcpUDTIq4m/FIjFs98JaahRITT0utJxpwEPFgV17axRRlohDaMnQw==", null, false, "ac384698-28a5-4163-b471-7e7c8c894c69", false, "Admin2" },
                    { "1ec6d434-ef87-4d40-a16b-15195cff094f", 0, "e387cc8e-e35a-41fe-b8a7-6fb116f3b3e1", "UserAccount", "user1@user.com", false, false, null, "User1", null, null, "AQAAAAIAAYagAAAAEKIAVdRRQEvHdn9Jj1DxrN6EmwbFzNZ4oVzZjkCzrKAC0/BBoTw2fven3Ujk3r/AnQ==", null, false, "a859b115-b434-4113-8409-d5c82c292c3f", false, "User1" },
                    { "2c7ea02a-93f3-4cd8-9bbd-6df65fd019cb", 0, "697a3a89-ecca-43d8-a1df-34097f2c9342", "UserAccount", "admin1@user.com", false, false, null, "Admin1", null, null, "AQAAAAIAAYagAAAAEEUZzw/kqkQq5atRoCS9MdypjcyXuf73bsNrrZxiB8M0w/84Mk0ieGHRC9X+togebg==", null, false, "4c84b0e0-781c-4818-95b6-d4d23fdc11cb", false, "Admin1" },
                    { "40b29fac-8c21-4212-bd4c-ef469b5209e1", 0, "5604de06-9340-473a-92d8-508d7c2d3668", "UserAccount", "admin3@user.com", false, false, null, "Admin3", null, null, "AQAAAAIAAYagAAAAELzx2Igstm5XVh+u4mrZKCXPOZL/j6dwxrqDuXdAbswM/nw27cgll4IDhZIsBKG1+g==", null, false, "acb40238-8251-425b-bee4-a767102dead4", false, "Admin3" },
                    { "cae16653-8b16-4b2f-99d8-b62125b0ca65", 0, "89242b54-611f-4e84-9cdc-f01eaf965003", "UserAccount", "user3@user.com", false, false, null, "User3", null, null, "AQAAAAIAAYagAAAAECFJ1oIARwHPwz6vpe1LcmzweW+UcPS03NEIcq2ckCYTuduGw1Avr0atd+ySkDv17w==", null, false, "02abd987-cbbc-4d59-8647-a5c06fd5e272", false, "User3" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "1c1ebabd-6745-4fc2-808d-48df8107736c", "0a6a73d6-3e34-4a43-90c3-5e78a3d984b2" },
                    { "47e17bad-e591-4084-b31c-40c1e4859bd7", "0ae67c27-0915-4f7a-8385-38850e6f8f3d" },
                    { "1c1ebabd-6745-4fc2-808d-48df8107736c", "1ec6d434-ef87-4d40-a16b-15195cff094f" },
                    { "47e17bad-e591-4084-b31c-40c1e4859bd7", "2c7ea02a-93f3-4cd8-9bbd-6df65fd019cb" },
                    { "47e17bad-e591-4084-b31c-40c1e4859bd7", "40b29fac-8c21-4212-bd4c-ef469b5209e1" },
                    { "1c1ebabd-6745-4fc2-808d-48df8107736c", "cae16653-8b16-4b2f-99d8-b62125b0ca65" }
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
