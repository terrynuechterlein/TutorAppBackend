﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorAppBackend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.CreateTable(
            //     name: "AspNetRoles",
            //     columns: table => new
            //     {
            //         Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //         Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            //         NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            //         ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_AspNetRoles", x => x.Id);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "AspNetUsers",
            //     columns: table => new
            //     {
            //         Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //         FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         School = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         Grade = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         Major = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         ProfilePictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         BannerImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         IsTutor = table.Column<bool>(type: "bit", nullable: false),
            //         ClassYear = table.Column<int>(type: "int", nullable: false),
            //         Popularity = table.Column<int>(type: "int", nullable: false),
            //         YoutubeUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         TwitchUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         DiscordUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         LinkedInUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         IsSetupComplete = table.Column<bool>(type: "bit", nullable: false),
            //         UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            //         NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            //         Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            //         NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            //         EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
            //         PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
            //         TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
            //         LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //         LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
            //         AccessFailedCount = table.Column<int>(type: "int", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_AspNetUsers", x => x.Id);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "AspNetRoleClaims",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "int", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //         ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
            //             column: x => x.RoleId,
            //             principalTable: "AspNetRoles",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "AspNetUserClaims",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "int", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //         ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_AspNetUserClaims_AspNetUsers_UserId",
            //             column: x => x.UserId,
            //             principalTable: "AspNetUsers",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "AspNetUserLogins",
            //     columns: table => new
            //     {
            //         LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
            //         ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
            //         ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
            //         table.ForeignKey(
            //             name: "FK_AspNetUserLogins_AspNetUsers_UserId",
            //             column: x => x.UserId,
            //             principalTable: "AspNetUsers",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "AspNetUserRoles",
            //     columns: table => new
            //     {
            //         UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //         RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
            //         table.ForeignKey(
            //             name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
            //             column: x => x.RoleId,
            //             principalTable: "AspNetRoles",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_AspNetUserRoles_AspNetUsers_UserId",
            //             column: x => x.UserId,
            //             principalTable: "AspNetUsers",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "AspNetUserTokens",
            //     columns: table => new
            //     {
            //         UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //         LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
            //         Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
            //         Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
            //         table.ForeignKey(
            //             name: "FK_AspNetUserTokens_AspNetUsers_UserId",
            //             column: x => x.UserId,
            //             principalTable: "AspNetUsers",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Messages",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "int", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //         Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
            //         SenderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //         ReceiverId = table.Column<string>(type: "nvarchar(450)", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Messages", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_Messages_AspNetUsers_ReceiverId",
            //             column: x => x.ReceiverId,
            //             principalTable: "AspNetUsers",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //         table.ForeignKey(
            //             name: "FK_Messages_AspNetUsers_SenderId",
            //             column: x => x.SenderId,
            //             principalTable: "AspNetUsers",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Posts",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "int", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         LikesCount = table.Column<int>(type: "int", nullable: false),
            //         PostedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //         UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Posts", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_Posts_AspNetUsers_UserId",
            //             column: x => x.UserId,
            //             principalTable: "AspNetUsers",
            //             principalColumn: "Id");
            //     });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsOpenToRequests = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // migrationBuilder.CreateTable(
            //     name: "Subject",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "int", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         Department = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Subject", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_Subject_AspNetUsers_UserId",
            //             column: x => x.UserId,
            //             principalTable: "AspNetUsers",
            //             principalColumn: "Id");
            //     });

            // migrationBuilder.CreateTable(
            //     name: "UserUser",
            //     columns: table => new
            //     {
            //         FollowersId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //         FollowingId = table.Column<string>(type: "nvarchar(450)", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_UserUser", x => new { x.FollowersId, x.FollowingId });
            //         table.ForeignKey(
            //             name: "FK_UserUser_AspNetUsers_FollowersId",
            //             column: x => x.FollowersId,
            //             principalTable: "AspNetUsers",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_UserUser_AspNetUsers_FollowingId",
            //             column: x => x.FollowingId,
            //             principalTable: "AspNetUsers",
            //             principalColumn: "Id");
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Comment",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "int", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         CommentedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //         UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
            //         PostId = table.Column<int>(type: "int", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Comment", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_Comment_AspNetUsers_UserId",
            //             column: x => x.UserId,
            //             principalTable: "AspNetUsers",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //         table.ForeignKey(
            //             name: "FK_Comment_Posts_PostId",
            //             column: x => x.PostId,
            //             principalTable: "Posts",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //     });

            migrationBuilder.CreateTable(
                name: "ProjectMembers",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProjectId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMembers", x => new { x.ProjectId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ProjectMembers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectMembers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            // migrationBuilder.CreateIndex(
            //     name: "IX_AspNetRoleClaims_RoleId",
            //     table: "AspNetRoleClaims",
            //     column: "RoleId");

            // migrationBuilder.CreateIndex(
            //     name: "RoleNameIndex",
            //     table: "AspNetRoles",
            //     column: "NormalizedName",
            //     unique: true,
            //     filter: "[NormalizedName] IS NOT NULL");

            // migrationBuilder.CreateIndex(
            //     name: "IX_AspNetUserClaims_UserId",
            //     table: "AspNetUserClaims",
            //     column: "UserId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_AspNetUserLogins_UserId",
            //     table: "AspNetUserLogins",
            //     column: "UserId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_AspNetUserRoles_RoleId",
            //     table: "AspNetUserRoles",
            //     column: "RoleId");

            // migrationBuilder.CreateIndex(
            //     name: "EmailIndex",
            //     table: "AspNetUsers",
            //     column: "NormalizedEmail");

            // migrationBuilder.CreateIndex(
            //     name: "UserNameIndex",
            //     table: "AspNetUsers",
            //     column: "NormalizedUserName",
            //     unique: true,
            //     filter: "[NormalizedUserName] IS NOT NULL");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Comment_PostId",
            //     table: "Comment",
            //     column: "PostId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Comment_UserId",
            //     table: "Comment",
            //     column: "UserId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Messages_ReceiverId",
            //     table: "Messages",
            //     column: "ReceiverId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Messages_SenderId",
            //     table: "Messages",
            //     column: "SenderId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Posts_UserId",
            //     table: "Posts",
            //     column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_UserId",
                table: "ProjectMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CreatorId",
                table: "Projects",
                column: "CreatorId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Subject_UserId",
            //     table: "Subject",
            //     column: "UserId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_UserUser_FollowingId",
            //     table: "UserUser",
            //     column: "FollowingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropTable(
            //     name: "AspNetRoleClaims");

            // migrationBuilder.DropTable(
            //     name: "AspNetUserClaims");

            // migrationBuilder.DropTable(
            //     name: "AspNetUserLogins");

            // migrationBuilder.DropTable(
            //     name: "AspNetUserRoles");

            // migrationBuilder.DropTable(
            //     name: "AspNetUserTokens");

            // migrationBuilder.DropTable(
            //     name: "Comment");

            // migrationBuilder.DropTable(
            //     name: "Messages");

            migrationBuilder.DropTable(
                name: "ProjectMembers");

            // migrationBuilder.DropTable(
            //     name: "Subject");

            // migrationBuilder.DropTable(
            //     name: "UserUser");

            // migrationBuilder.DropTable(
            //     name: "AspNetRoles");

            // migrationBuilder.DropTable(
            //     name: "Posts");

            migrationBuilder.DropTable(
                name: "Projects");

            // migrationBuilder.DropTable(
            //     name: "AspNetUsers");
        }
    }
}
