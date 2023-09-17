using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelManagment_API.Migrations
{
    /// <inheritdoc />
    public partial class SeedHotelsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Hotels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "Id", "Address", "CreatedDate", "Description", "Email", "ImageUrl", "Name", "PhoneNumber", "Rating" },
                values: new object[,]
                {
                    { 1, "123 Main St", new DateTime(2023, 9, 9, 1, 25, 16, 3, DateTimeKind.Local).AddTicks(2807), "A wonderful place to stay.", "hotel1@gmail.com", "https://example.com/hotel1.jpg", "Sample Hotel 1", "123-456-7890", 4.5m },
                    { 2, "456 Elm St", new DateTime(2023, 9, 9, 1, 25, 16, 3, DateTimeKind.Local).AddTicks(2830), "Another great option for travelers.", "hotel2@gmail.com", "https://example.com/hotel2.jpg", "Sample Hotel 2", "987-654-3210", 4.0m },
                    { 3, "789 Oak St", new DateTime(2023, 9, 9, 1, 25, 16, 3, DateTimeKind.Local).AddTicks(2833), "Experience luxury at its finest.", "hotel3@gmail.com", "https://example.com/hotel3.jpg", "Sample Hotel 3", "555-123-4567", 4.8m },
                    { 4, "101 Pine St", new DateTime(2023, 9, 9, 1, 25, 16, 3, DateTimeKind.Local).AddTicks(2836), "Affordable and comfortable accommodations.", "hotel4@gmail.com", "https://example.com/hotel4.jpg", "Sample Hotel 4", "777-999-8888", 3.7m },
                    { 5, "321 Cedar St", new DateTime(2023, 9, 9, 1, 25, 16, 3, DateTimeKind.Local).AddTicks(2839), "A cozy retreat for nature lovers.", "hotel5@gmail.com", "https://example.com/hotel5.jpg", "Sample Hotel 5", "222-555-1111", 4.2m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Hotels");
        }
    }
}
