﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelManagment_API.Migrations
{
    /// <inheritdoc />
    public partial class ChangeImageUrlTypeInHotelsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7cf4ccd0-42f4-4744-8d8f-725f55d76a04");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dffddad0-9ead-45dc-ba51-70c139343976");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Hotels",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a0f00c96-75d8-47bb-99f9-d5a0ac606b56", null, "User", "USER" },
                    { "ea9143d6-8db9-46a0-89e5-efbce964f43f", null, "Admin", "ADMIN" }
                });

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2023, 10, 3, 14, 10, 6, 237, DateTimeKind.Local).AddTicks(3650));

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2023, 10, 3, 14, 10, 6, 237, DateTimeKind.Local).AddTicks(3663));

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2023, 10, 3, 14, 10, 6, 237, DateTimeKind.Local).AddTicks(3665));

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2023, 10, 3, 14, 10, 6, 237, DateTimeKind.Local).AddTicks(3667));

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2023, 10, 3, 14, 10, 6, 237, DateTimeKind.Local).AddTicks(3669));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a0f00c96-75d8-47bb-99f9-d5a0ac606b56");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ea9143d6-8db9-46a0-89e5-efbce964f43f");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Hotels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7cf4ccd0-42f4-4744-8d8f-725f55d76a04", null, "User", "USER" },
                    { "dffddad0-9ead-45dc-ba51-70c139343976", null, "Admin", "ADMIN" }
                });

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2023, 9, 28, 17, 55, 51, 344, DateTimeKind.Local).AddTicks(789));

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2023, 9, 28, 17, 55, 51, 344, DateTimeKind.Local).AddTicks(803));

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2023, 9, 28, 17, 55, 51, 344, DateTimeKind.Local).AddTicks(805));

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2023, 9, 28, 17, 55, 51, 344, DateTimeKind.Local).AddTicks(807));

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2023, 9, 28, 17, 55, 51, 344, DateTimeKind.Local).AddTicks(809));
        }
    }
}
