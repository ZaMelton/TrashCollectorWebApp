using Microsoft.EntityFrameworkCore.Migrations;

namespace TrashCollectorV2.Data.Migrations
{
    public partial class AddedMoreRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ac1fcff3-ad28-41c3-9347-bebc00adb406");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "58266f0a-a020-49e0-889c-01a99493ace8", "df4c8733-7743-4dbb-8e81-a7b8fc42274b", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5ba2c9c7-61e9-4cd9-bf33-6ad593a285fa", "7bc10ad0-3367-47d5-84dc-2ffbb8f51086", "Customer", "CUSTOMER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "04b095fe-43d4-425a-a9dd-8cf16e03fc30", "77f0eb9f-2513-41e2-bde2-523af715596d", "Employee", "EMPLOYEE" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "04b095fe-43d4-425a-a9dd-8cf16e03fc30");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "58266f0a-a020-49e0-889c-01a99493ace8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5ba2c9c7-61e9-4cd9-bf33-6ad593a285fa");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ac1fcff3-ad28-41c3-9347-bebc00adb406", "e9a4d7d3-3b59-4591-8885-a5b92475c8ac", "Admin", "ADMIN" });
        }
    }
}
