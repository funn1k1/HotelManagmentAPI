using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelManagment_API.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PricePerNight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    HotelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2023, 9, 10, 20, 3, 39, 28, DateTimeKind.Local).AddTicks(2919));

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2023, 9, 10, 20, 3, 39, 28, DateTimeKind.Local).AddTicks(2933));

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2023, 9, 10, 20, 3, 39, 28, DateTimeKind.Local).AddTicks(2936));

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2023, 9, 10, 20, 3, 39, 28, DateTimeKind.Local).AddTicks(2938));

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2023, 9, 10, 20, 3, 39, 28, DateTimeKind.Local).AddTicks(2940));

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "HotelId", "IsAvailable", "PricePerNight", "RoomNumber" },
                values: new object[,]
                {
                    { 1, 1, true, 150.00m, "101" },
                    { 2, 1, false, 175.00m, "102" },
                    { 3, 1, false, 130.00m, "103" },
                    { 4, 2, false, 150.00m, "Standard double room" },
                    { 5, 2, true, 500.00m, "Deluxe sea view suite" },
                    { 6, 3, true, 100.00m, "202A" },
                    { 7, 4, false, 450.00m, "200" },
                    { 8, 4, true, 500.00m, "201" },
                    { 9, 5, false, 115.00m, "Standard single room" },
                    { 10, 5, true, 200.00m, "Family room with two beds" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_HotelId",
                table: "Rooms",
                column: "HotelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2023, 9, 9, 1, 25, 16, 3, DateTimeKind.Local).AddTicks(2807));

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2023, 9, 9, 1, 25, 16, 3, DateTimeKind.Local).AddTicks(2830));

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2023, 9, 9, 1, 25, 16, 3, DateTimeKind.Local).AddTicks(2833));

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2023, 9, 9, 1, 25, 16, 3, DateTimeKind.Local).AddTicks(2836));

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2023, 9, 9, 1, 25, 16, 3, DateTimeKind.Local).AddTicks(2839));
        }
    }
}
