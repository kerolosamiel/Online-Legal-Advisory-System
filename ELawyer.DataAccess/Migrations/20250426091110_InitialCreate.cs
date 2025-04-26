using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ELawyer.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.ID);
                });

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
                name: "Clients",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoOfLawyers = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClientType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FrontCardImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BackCardImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientRatingID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Specializations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specializations", x => x.ID);
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
                name: "SubSpecializations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpecializationID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubSpecializations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SubSpecializations_Specializations_SpecializationID",
                        column: x => x.SpecializationID,
                        principalTable: "Specializations",
                        principalColumn: "ID",
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
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientID = table.Column<int>(type: "int", nullable: true),
                    LawyerID = table.Column<int>(type: "int", nullable: true),
                    AdminID = table.Column<int>(type: "int", nullable: true),
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
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Admins_AdminID",
                        column: x => x.AdminID,
                        principalTable: "Admins",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Clients_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Clients",
                        principalColumn: "ID");
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
                name: "Consultations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lawyerID = table.Column<int>(type: "int", nullable: true),
                    ClientID = table.Column<int>(type: "int", nullable: true),
                    Attachments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Consultations_Clients_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Clients",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Lawyers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    About = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoOfClients = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FrontCardImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BackCardImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinceseNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExperienceYears = table.Column<int>(type: "int", nullable: true),
                    Linkedin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConsultationFee = table.Column<int>(type: "int", nullable: true),
                    AverageRateing = table.Column<double>(type: "float", nullable: true),
                    ServiceID = table.Column<int>(type: "int", nullable: true),
                    LawyerRatingID = table.Column<int>(type: "int", nullable: true),
                    SpecializationnewID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lawyers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Lawyers_Specializations_SpecializationnewID",
                        column: x => x.SpecializationnewID,
                        principalTable: "Specializations",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "lawyerSpecializations",
                columns: table => new
                {
                    LawyerId = table.Column<int>(type: "int", nullable: false),
                    SpecializationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lawyerSpecializations", x => new { x.LawyerId, x.SpecializationId });
                    table.ForeignKey(
                        name: "FK_lawyerSpecializations_Lawyers_LawyerId",
                        column: x => x.LawyerId,
                        principalTable: "Lawyers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_lawyerSpecializations_Specializations_SpecializationId",
                        column: x => x.SpecializationId,
                        principalTable: "Specializations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Recievedat = table.Column<DateTime>(type: "datetime2", nullable: true),
                    lawyerID = table.Column<int>(type: "int", nullable: true),
                    ClientID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Clients_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Clients",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Payments_Lawyers_lawyerID",
                        column: x => x.lawyerID,
                        principalTable: "Lawyers",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rate = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    lawyerID = table.Column<int>(type: "int", nullable: true),
                    ClientID = table.Column<int>(type: "int", nullable: true),
                    ClientRatingID = table.Column<int>(type: "int", nullable: true),
                    LawyerRatingID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Ratings_Clients_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Clients",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Ratings_Clients_ClientRatingID",
                        column: x => x.ClientRatingID,
                        principalTable: "Clients",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Ratings_Lawyers_LawyerRatingID",
                        column: x => x.LawyerRatingID,
                        principalTable: "Lawyers",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Ratings_Lawyers_lawyerID",
                        column: x => x.lawyerID,
                        principalTable: "Lawyers",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Responses",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lawyerID = table.Column<int>(type: "int", nullable: true),
                    ClientID = table.Column<int>(type: "int", nullable: true),
                    Attachments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConsultationID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Responses", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Responses_Clients_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Clients",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Responses_Lawyers_lawyerID",
                        column: x => x.lawyerID,
                        principalTable: "Lawyers",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<TimeOnly>(type: "time", nullable: false),
                    LawyerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Services_Lawyers_LawyerID",
                        column: x => x.LawyerID,
                        principalTable: "Lawyers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Specializations",
                columns: new[] { "ID", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Deals with crimes and legal punishments, representing individuals or the state in cases involving offenses like theft, assault, or murder.", "Criminal Law" },
                    { 2, "Focuses on business transactions, corporate governance, contracts, mergers, and regulatory compliance to ensure legal and ethical business operations.", "Commercial And Corporate Law" },
                    { 3, "Covers workplace rights, employment disputes, social security, and insurance claims, ensuring fair treatment and compliance with labor regulations", "Labor And Insurance Law" },
                    { 4, " Governs family matters such as marriage, divorce, child custody, and inheritance, ensuring legal protection of personal and familial rights.", "Personal Atatus Law" },
                    { 5, "Regulates property ownership, transactions, leasing, zoning, and land disputes, ensuring legal compliance in real estate dealings.", "Real Estate And Property Law" },
                    { 6, "Deals with regulations, government agencies, public administration, and legal disputes involving governmental decisions and policies.", "Administrative Law And Government Issues" },
                    { 7, "Governs legal relations between nations, international treaties, trade laws, and dispute resolution through arbitration rather than litigation.", "International Law And Arbitration" },
                    { 8, "Covers taxation regulations, compliance, tax planning, and financial advisory services to help individuals and businesses manage their fiscal responsibilities.", "Tax Law and Financial Consultation" },
                    { 9, "Protects creations like inventions, trademarks, patents, and artistic works, ensuring exclusive rights and preventing unauthorized use.", "Intellectual Property And Copyright Law" }
                });

            migrationBuilder.InsertData(
                table: "SubSpecializations",
                columns: new[] { "ID", "Name", "SpecializationID" },
                values: new object[,]
                {
                    { 1, "Defending defendants in criminal cases.", 1 },
                    { 2, "Providing advice on criminal laws and penalties", 1 },
                    { 3, "Drug-related issues, theft, murder, fraud, and cybercrimes", 1 },
                    { 4, "Filing appeals against criminal convictions", 1 },
                    { 5, "Establishing companies and preparing legal contracts", 2 },
                    { 6, "Drafting and reviewing partnership and intellectual property contracts", 2 },
                    { 7, "Bankruptcy and liquidation issues", 2 },
                    { 8, "Commercial disputes between companies or individuals", 2 },
                    { 9, "Dyeing and reviewing employment contracts", 3 },
                    { 10, "Arbitrary dismissal and employee harassment cases", 3 },
                    { 11, "Consultations on social insurance and pension laws", 3 },
                    { 12, "Labor disputes between companies and employees", 3 },
                    { 13, "Wedding, divorce and pollen gifts", 4 },
                    { 14, "Alimony and child custody", 4 },
                    { 15, "Providing inheritance according to Sharia and civil laws", 4 },
                    { 16, "Consultations regarding civil marriage and customary marriage", 4 },
                    { 17, "Drafting and reviewing sales and lease contracts", 5 },
                    { 18, "Disputes between buyers and sellers", 5 },
                    { 19, "Consultations on real estate ownership and legal registration", 5 },
                    { 20, "Disputes related to residential associations and real estate development", 5 },
                    { 21, "Issues related to dealing with government agencies", 6 },
                    { 22, "Appeals informed administrative decisions", 6 },
                    { 23, "onsultations regarding legal licenses and permits", 6 },
                    { 24, "Issues related to treaties and international relations", 7 },
                    { 25, "Legal consultations on international trade and foreign investment", 7 },
                    { 26, "International dispute resolution and commercial arbitration", 7 },
                    { 27, "Consultations on tax obligations for individuals and companies", 8 },
                    { 28, "Tax evasion issues and legal accounting procedures", 8 },
                    { 29, "Drafting and reviewing corporate tax plans", 8 },
                    { 30, "Registration and protection of patents and trademarks", 9 },
                    { 31, "Copyright and Intellectual Property Infringement Issues", 9 },
                    { 32, "Copyright Law Consultations", 9 }
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
                name: "IX_AspNetUsers_AdminID",
                table: "AspNetUsers",
                column: "AdminID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ClientID",
                table: "AspNetUsers",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_LawyerID",
                table: "AspNetUsers",
                column: "LawyerID");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Consultations_ClientID",
                table: "Consultations",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_Consultations_lawyerID",
                table: "Consultations",
                column: "lawyerID");

            migrationBuilder.CreateIndex(
                name: "IX_Lawyers_ServiceID",
                table: "Lawyers",
                column: "ServiceID");

            migrationBuilder.CreateIndex(
                name: "IX_Lawyers_SpecializationnewID",
                table: "Lawyers",
                column: "SpecializationnewID");

            migrationBuilder.CreateIndex(
                name: "IX_lawyerSpecializations_SpecializationId",
                table: "lawyerSpecializations",
                column: "SpecializationId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ClientID",
                table: "Payments",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_lawyerID",
                table: "Payments",
                column: "lawyerID");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_ClientID",
                table: "Ratings",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_ClientRatingID",
                table: "Ratings",
                column: "ClientRatingID");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_lawyerID",
                table: "Ratings",
                column: "lawyerID");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_LawyerRatingID",
                table: "Ratings",
                column: "LawyerRatingID");

            migrationBuilder.CreateIndex(
                name: "IX_Responses_ClientID",
                table: "Responses",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_Responses_lawyerID",
                table: "Responses",
                column: "lawyerID");

            migrationBuilder.CreateIndex(
                name: "IX_Services_LawyerID",
                table: "Services",
                column: "LawyerID");

            migrationBuilder.CreateIndex(
                name: "IX_SubSpecializations_SpecializationID",
                table: "SubSpecializations",
                column: "SpecializationID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Lawyers_LawyerID",
                table: "AspNetUsers",
                column: "LawyerID",
                principalTable: "Lawyers",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultations_Lawyers_lawyerID",
                table: "Consultations",
                column: "lawyerID",
                principalTable: "Lawyers",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Lawyers_Services_ServiceID",
                table: "Lawyers",
                column: "ServiceID",
                principalTable: "Services",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Lawyers_LawyerID",
                table: "Services");

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
                name: "Consultations");

            migrationBuilder.DropTable(
                name: "lawyerSpecializations");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "Responses");

            migrationBuilder.DropTable(
                name: "SubSpecializations");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Lawyers");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Specializations");
        }
    }
}
