using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELawyer.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ModifyAttrs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Admins_AdminID",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Clients_ClientID",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Lawyers_LawyerID",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Consultations_Clients_ClientID",
                table: "Consultations");

            migrationBuilder.DropForeignKey(
                name: "FK_Consultations_Lawyers_lawyerID",
                table: "Consultations");

            migrationBuilder.DropForeignKey(
                name: "FK_Lawyers_Services_ServiceID",
                table: "Lawyers");

            migrationBuilder.DropForeignKey(
                name: "FK_Lawyers_Specializations_SpecializationnewID",
                table: "Lawyers");

            migrationBuilder.DropForeignKey(
                name: "FK_lawyerSpecializations_Lawyers_LawyerId",
                table: "lawyerSpecializations");

            migrationBuilder.DropForeignKey(
                name: "FK_lawyerSpecializations_Specializations_SpecializationId",
                table: "lawyerSpecializations");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Clients_ClientID",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Lawyers_lawyerID",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Clients_ClientID",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Clients_ClientRatingID",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Lawyers_LawyerRatingID",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Lawyers_lawyerID",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Responses_Clients_ClientID",
                table: "Responses");

            migrationBuilder.DropForeignKey(
                name: "FK_Responses_Lawyers_lawyerID",
                table: "Responses");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Lawyers_LawyerID",
                table: "Services");

            migrationBuilder.DropPrimaryKey(
                name: "PK_lawyerSpecializations",
                table: "lawyerSpecializations");

            migrationBuilder.DropIndex(
                name: "IX_Lawyers_ServiceID",
                table: "Lawyers");

            migrationBuilder.DropIndex(
                name: "IX_Lawyers_SpecializationnewID",
                table: "Lawyers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AdminID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ClientID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_LawyerID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Lawyers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Lawyers");

            migrationBuilder.DropColumn(
                name: "LastLogin",
                table: "Lawyers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Lawyers");

            migrationBuilder.DropColumn(
                name: "SpecializationnewID",
                table: "Lawyers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "LastLogin",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "LastLogin",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Admins");

            migrationBuilder.RenameTable(
                name: "lawyerSpecializations",
                newName: "LawyerSpecializations");

            migrationBuilder.RenameColumn(
                name: "LawyerID",
                table: "Services",
                newName: "LawyerId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Services",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Services_LawyerID",
                table: "Services",
                newName: "IX_Services_LawyerId");

            migrationBuilder.RenameColumn(
                name: "lawyerID",
                table: "Responses",
                newName: "LawyerId");

            migrationBuilder.RenameColumn(
                name: "ConsultationID",
                table: "Responses",
                newName: "ConsultationId");

            migrationBuilder.RenameColumn(
                name: "ClientID",
                table: "Responses",
                newName: "ClientId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Responses",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Responses_lawyerID",
                table: "Responses",
                newName: "IX_Responses_LawyerId");

            migrationBuilder.RenameIndex(
                name: "IX_Responses_ClientID",
                table: "Responses",
                newName: "IX_Responses_ClientId");

            migrationBuilder.RenameColumn(
                name: "lawyerID",
                table: "Ratings",
                newName: "LawyerId");

            migrationBuilder.RenameColumn(
                name: "LawyerRatingID",
                table: "Ratings",
                newName: "LawyerRatingId");

            migrationBuilder.RenameColumn(
                name: "ClientRatingID",
                table: "Ratings",
                newName: "ClientRatingId");

            migrationBuilder.RenameColumn(
                name: "ClientID",
                table: "Ratings",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_LawyerRatingID",
                table: "Ratings",
                newName: "IX_Ratings_LawyerRatingId");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_lawyerID",
                table: "Ratings",
                newName: "IX_Ratings_LawyerId");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_ClientRatingID",
                table: "Ratings",
                newName: "IX_Ratings_ClientRatingId");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_ClientID",
                table: "Ratings",
                newName: "IX_Ratings_ClientId");

            migrationBuilder.RenameColumn(
                name: "lawyerID",
                table: "Payments",
                newName: "LawyerId");

            migrationBuilder.RenameColumn(
                name: "ClientID",
                table: "Payments",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_lawyerID",
                table: "Payments",
                newName: "IX_Payments_LawyerId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_ClientID",
                table: "Payments",
                newName: "IX_Payments_ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_lawyerSpecializations_SpecializationId",
                table: "LawyerSpecializations",
                newName: "IX_LawyerSpecializations_SpecializationId");

            migrationBuilder.RenameColumn(
                name: "ServiceID",
                table: "Lawyers",
                newName: "ServiceId");

            migrationBuilder.RenameColumn(
                name: "Linkedin",
                table: "Lawyers",
                newName: "LinkedIn");

            migrationBuilder.RenameColumn(
                name: "LawyerRatingID",
                table: "Lawyers",
                newName: "LawyerRatingId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Lawyers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "lawyerID",
                table: "Consultations",
                newName: "LawyerId");

            migrationBuilder.RenameColumn(
                name: "ClientID",
                table: "Consultations",
                newName: "ClientId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Consultations",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Consultations_lawyerID",
                table: "Consultations",
                newName: "IX_Consultations_LawyerId");

            migrationBuilder.RenameIndex(
                name: "IX_Consultations_ClientID",
                table: "Consultations",
                newName: "IX_Consultations_ClientId");

            migrationBuilder.RenameColumn(
                name: "ClientRatingID",
                table: "Clients",
                newName: "ClientRatingId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Clients",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "LawyerID",
                table: "AspNetUsers",
                newName: "LawyerId");

            migrationBuilder.RenameColumn(
                name: "ClientID",
                table: "AspNetUsers",
                newName: "ClientId");

            migrationBuilder.RenameColumn(
                name: "AdminID",
                table: "AspNetUsers",
                newName: "AdminId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Admins",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Specializations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Specializations",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Services",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Services",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceType",
                table: "Services",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Services",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "Services",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Responses",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Responses",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Rate",
                table: "Ratings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "Ratings",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LawyerId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserStatus",
                table: "Lawyers",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LinkedIn",
                table: "Lawyers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LinceseNumber",
                table: "Lawyers",
                type: "nvarchar(35)",
                maxLength: 35,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Lawyers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Lawyers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "About",
                table: "Lawyers",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Consultations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Consultations",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserStatus",
                table: "Clients",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Clients",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FrontCardImage",
                table: "Clients",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClientType",
                table: "Clients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BackCardImage",
                table: "Clients",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Clients",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLogin",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Admins",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Admins",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LawyerSpecializations",
                table: "LawyerSpecializations",
                columns: new[] { "LawyerId", "SpecializationId" });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    ServiceOrderId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    PaymentId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScheduledAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceOrders_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceOrders_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AdminId",
                table: "AspNetUsers",
                column: "AdminId",
                unique: true,
                filter: "[AdminId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ClientId",
                table: "AspNetUsers",
                column: "ClientId",
                unique: true,
                filter: "[ClientId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_LawyerId",
                table: "AspNetUsers",
                column: "LawyerId",
                unique: true,
                filter: "[LawyerId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_ClientId",
                table: "Invoices",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOrders_PaymentId",
                table: "ServiceOrders",
                column: "PaymentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOrders_ServiceId",
                table: "ServiceOrders",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Admins_AdminId",
                table: "AspNetUsers",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Clients_ClientId",
                table: "AspNetUsers",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Lawyers_LawyerId",
                table: "AspNetUsers",
                column: "LawyerId",
                principalTable: "Lawyers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultations_Clients_ClientId",
                table: "Consultations",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultations_Lawyers_LawyerId",
                table: "Consultations",
                column: "LawyerId",
                principalTable: "Lawyers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LawyerSpecializations_Lawyers_LawyerId",
                table: "LawyerSpecializations",
                column: "LawyerId",
                principalTable: "Lawyers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LawyerSpecializations_Specializations_SpecializationId",
                table: "LawyerSpecializations",
                column: "SpecializationId",
                principalTable: "Specializations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Clients_ClientId",
                table: "Payments",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Lawyers_LawyerId",
                table: "Payments",
                column: "LawyerId",
                principalTable: "Lawyers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Clients_ClientId",
                table: "Ratings",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Clients_ClientRatingId",
                table: "Ratings",
                column: "ClientRatingId",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Lawyers_LawyerId",
                table: "Ratings",
                column: "LawyerId",
                principalTable: "Lawyers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Lawyers_LawyerRatingId",
                table: "Ratings",
                column: "LawyerRatingId",
                principalTable: "Lawyers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Responses_Clients_ClientId",
                table: "Responses",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Responses_Lawyers_LawyerId",
                table: "Responses",
                column: "LawyerId",
                principalTable: "Lawyers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Lawyers_LawyerId",
                table: "Services",
                column: "LawyerId",
                principalTable: "Lawyers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Admins_AdminId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Clients_ClientId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Lawyers_LawyerId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Consultations_Clients_ClientId",
                table: "Consultations");

            migrationBuilder.DropForeignKey(
                name: "FK_Consultations_Lawyers_LawyerId",
                table: "Consultations");

            migrationBuilder.DropForeignKey(
                name: "FK_LawyerSpecializations_Lawyers_LawyerId",
                table: "LawyerSpecializations");

            migrationBuilder.DropForeignKey(
                name: "FK_LawyerSpecializations_Specializations_SpecializationId",
                table: "LawyerSpecializations");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Clients_ClientId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Lawyers_LawyerId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Clients_ClientId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Clients_ClientRatingId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Lawyers_LawyerId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Lawyers_LawyerRatingId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Responses_Clients_ClientId",
                table: "Responses");

            migrationBuilder.DropForeignKey(
                name: "FK_Responses_Lawyers_LawyerId",
                table: "Responses");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Lawyers_LawyerId",
                table: "Services");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "ServiceOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LawyerSpecializations",
                table: "LawyerSpecializations");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AdminId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ClientId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_LawyerId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastLogin",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "LawyerSpecializations",
                newName: "lawyerSpecializations");

            migrationBuilder.RenameColumn(
                name: "LawyerId",
                table: "Services",
                newName: "LawyerID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Services",
                newName: "ID");

            migrationBuilder.RenameIndex(
                name: "IX_Services_LawyerId",
                table: "Services",
                newName: "IX_Services_LawyerID");

            migrationBuilder.RenameColumn(
                name: "LawyerId",
                table: "Responses",
                newName: "lawyerID");

            migrationBuilder.RenameColumn(
                name: "ConsultationId",
                table: "Responses",
                newName: "ConsultationID");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Responses",
                newName: "ClientID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Responses",
                newName: "ID");

            migrationBuilder.RenameIndex(
                name: "IX_Responses_LawyerId",
                table: "Responses",
                newName: "IX_Responses_lawyerID");

            migrationBuilder.RenameIndex(
                name: "IX_Responses_ClientId",
                table: "Responses",
                newName: "IX_Responses_ClientID");

            migrationBuilder.RenameColumn(
                name: "LawyerRatingId",
                table: "Ratings",
                newName: "LawyerRatingID");

            migrationBuilder.RenameColumn(
                name: "LawyerId",
                table: "Ratings",
                newName: "lawyerID");

            migrationBuilder.RenameColumn(
                name: "ClientRatingId",
                table: "Ratings",
                newName: "ClientRatingID");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Ratings",
                newName: "ClientID");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_LawyerRatingId",
                table: "Ratings",
                newName: "IX_Ratings_LawyerRatingID");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_LawyerId",
                table: "Ratings",
                newName: "IX_Ratings_lawyerID");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_ClientRatingId",
                table: "Ratings",
                newName: "IX_Ratings_ClientRatingID");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_ClientId",
                table: "Ratings",
                newName: "IX_Ratings_ClientID");

            migrationBuilder.RenameColumn(
                name: "LawyerId",
                table: "Payments",
                newName: "lawyerID");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Payments",
                newName: "ClientID");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_LawyerId",
                table: "Payments",
                newName: "IX_Payments_lawyerID");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_ClientId",
                table: "Payments",
                newName: "IX_Payments_ClientID");

            migrationBuilder.RenameIndex(
                name: "IX_LawyerSpecializations_SpecializationId",
                table: "lawyerSpecializations",
                newName: "IX_lawyerSpecializations_SpecializationId");

            migrationBuilder.RenameColumn(
                name: "ServiceId",
                table: "Lawyers",
                newName: "ServiceID");

            migrationBuilder.RenameColumn(
                name: "LinkedIn",
                table: "Lawyers",
                newName: "Linkedin");

            migrationBuilder.RenameColumn(
                name: "LawyerRatingId",
                table: "Lawyers",
                newName: "LawyerRatingID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Lawyers",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "LawyerId",
                table: "Consultations",
                newName: "lawyerID");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Consultations",
                newName: "ClientID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Consultations",
                newName: "ID");

            migrationBuilder.RenameIndex(
                name: "IX_Consultations_LawyerId",
                table: "Consultations",
                newName: "IX_Consultations_lawyerID");

            migrationBuilder.RenameIndex(
                name: "IX_Consultations_ClientId",
                table: "Consultations",
                newName: "IX_Consultations_ClientID");

            migrationBuilder.RenameColumn(
                name: "ClientRatingId",
                table: "Clients",
                newName: "ClientRatingID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Clients",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "LawyerId",
                table: "AspNetUsers",
                newName: "LawyerID");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "AspNetUsers",
                newName: "ClientID");

            migrationBuilder.RenameColumn(
                name: "AdminId",
                table: "AspNetUsers",
                newName: "AdminID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Admins",
                newName: "ID");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Specializations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Specializations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(400)",
                oldMaxLength: 400);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Services",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Services",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ServiceType",
                table: "Services",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Services",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Responses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Responses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5);

            migrationBuilder.AlterColumn<int>(
                name: "Rate",
                table: "Ratings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "Ratings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(400)",
                oldMaxLength: 400,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "lawyerID",
                table: "Payments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ClientID",
                table: "Payments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UserStatus",
                table: "Lawyers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Linkedin",
                table: "Lawyers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LinceseNumber",
                table: "Lawyers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(35)",
                oldMaxLength: 35);

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Lawyers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Lawyers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "About",
                table: "Lawyers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(400)",
                oldMaxLength: 400,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Lawyers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Lawyers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLogin",
                table: "Lawyers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Lawyers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpecializationnewID",
                table: "Lawyers",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Consultations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Consultations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(400)",
                oldMaxLength: 400,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserStatus",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FrontCardImage",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClientType",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "BackCardImage",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Clients",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLogin",
                table: "Clients",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Admins",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLogin",
                table: "Admins",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_lawyerSpecializations",
                table: "lawyerSpecializations",
                columns: new[] { "LawyerId", "SpecializationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Lawyers_ServiceID",
                table: "Lawyers",
                column: "ServiceID");

            migrationBuilder.CreateIndex(
                name: "IX_Lawyers_SpecializationnewID",
                table: "Lawyers",
                column: "SpecializationnewID");

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

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Admins_AdminID",
                table: "AspNetUsers",
                column: "AdminID",
                principalTable: "Admins",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Clients_ClientID",
                table: "AspNetUsers",
                column: "ClientID",
                principalTable: "Clients",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Lawyers_LawyerID",
                table: "AspNetUsers",
                column: "LawyerID",
                principalTable: "Lawyers",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultations_Clients_ClientID",
                table: "Consultations",
                column: "ClientID",
                principalTable: "Clients",
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

            migrationBuilder.AddForeignKey(
                name: "FK_Lawyers_Specializations_SpecializationnewID",
                table: "Lawyers",
                column: "SpecializationnewID",
                principalTable: "Specializations",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_lawyerSpecializations_Lawyers_LawyerId",
                table: "lawyerSpecializations",
                column: "LawyerId",
                principalTable: "Lawyers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_lawyerSpecializations_Specializations_SpecializationId",
                table: "lawyerSpecializations",
                column: "SpecializationId",
                principalTable: "Specializations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Clients_ClientID",
                table: "Payments",
                column: "ClientID",
                principalTable: "Clients",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Lawyers_lawyerID",
                table: "Payments",
                column: "lawyerID",
                principalTable: "Lawyers",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Clients_ClientID",
                table: "Ratings",
                column: "ClientID",
                principalTable: "Clients",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Clients_ClientRatingID",
                table: "Ratings",
                column: "ClientRatingID",
                principalTable: "Clients",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Lawyers_LawyerRatingID",
                table: "Ratings",
                column: "LawyerRatingID",
                principalTable: "Lawyers",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Lawyers_lawyerID",
                table: "Ratings",
                column: "lawyerID",
                principalTable: "Lawyers",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Responses_Clients_ClientID",
                table: "Responses",
                column: "ClientID",
                principalTable: "Clients",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Responses_Lawyers_lawyerID",
                table: "Responses",
                column: "lawyerID",
                principalTable: "Lawyers",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Lawyers_LawyerID",
                table: "Services",
                column: "LawyerID",
                principalTable: "Lawyers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
