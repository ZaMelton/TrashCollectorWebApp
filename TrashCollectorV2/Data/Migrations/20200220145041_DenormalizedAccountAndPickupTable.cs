using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TrashCollectorV2.Data.Migrations
{
    public partial class DenormalizedAccountAndPickupTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Pickups_PickupId",
                table: "Accounts");

            migrationBuilder.DropTable(
                name: "Pickups");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_PickupId",
                table: "Accounts");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2ee12451-ca7f-4cc1-8b33-d1261e2d3a31");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "97637aa9-7b71-421e-a93b-fee76ac09de3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e1e6b55a-f1e0-49bf-995c-40043d7a60cf");

            migrationBuilder.DropColumn(
                name: "PickupId",
                table: "Accounts");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndSuspend",
                table: "Accounts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "NextPickupDate",
                table: "Accounts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "OneTimePickup",
                table: "Accounts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "PickedUp",
                table: "Accounts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PickupDay",
                table: "Accounts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartSuspend",
                table: "Accounts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c2dcf583-ce05-404e-a5ef-ae3dab01c6a3", "87ff1078-52a9-4564-b25a-23d7b8c8c0b0", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e75cc4d2-0df3-4cc5-94ce-028792fe248a", "d72d40a1-2f91-40b7-bcfd-e02be35af161", "Customer", "CUSTOMER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b28db82f-422d-4c7e-979e-74ecce9170ff", "eef13776-34c3-46b1-b05a-cea0ae926ee7", "Employee", "EMPLOYEE" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b28db82f-422d-4c7e-979e-74ecce9170ff");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c2dcf583-ce05-404e-a5ef-ae3dab01c6a3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e75cc4d2-0df3-4cc5-94ce-028792fe248a");

            migrationBuilder.DropColumn(
                name: "EndSuspend",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "NextPickupDate",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "OneTimePickup",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "PickedUp",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "PickupDay",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "StartSuspend",
                table: "Accounts");

            migrationBuilder.AddColumn<int>(
                name: "PickupId",
                table: "Accounts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Pickups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EndSuspend = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OneTimePickup = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PickupDay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartSuspend = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pickups", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e1e6b55a-f1e0-49bf-995c-40043d7a60cf", "a6ab408b-cf2d-44db-8a00-21decefe44bf", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2ee12451-ca7f-4cc1-8b33-d1261e2d3a31", "40fd33a6-18d0-4c48-8c7b-b31893d96cc3", "Customer", "CUSTOMER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "97637aa9-7b71-421e-a93b-fee76ac09de3", "edd3f2f9-c268-457c-9205-74bb1b73fe16", "Employee", "EMPLOYEE" });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_PickupId",
                table: "Accounts",
                column: "PickupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Pickups_PickupId",
                table: "Accounts",
                column: "PickupId",
                principalTable: "Pickups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
