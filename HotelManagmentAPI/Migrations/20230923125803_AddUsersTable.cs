using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelManagment_API.Migrations
{
    /// <inheritdoc />
    public partial class AddUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2023, 9, 23, 15, 58, 3, 590, DateTimeKind.Local).AddTicks(5544), "https://images.unsplash.com/photo-1542314831-068cd1dbfeeb?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D" });

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2023, 9, 23, 15, 58, 3, 590, DateTimeKind.Local).AddTicks(5559), "https://www.travelandleisure.com/thmb/pCU_Y9fbQe4CT5Q73J9k2Bqd_bI=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/header-grand-velas-los-cabos-MXALLINC0222-46d3772ad56f4493a83e1bcb49e119f9.jpg" });

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2023, 9, 23, 15, 58, 3, 590, DateTimeKind.Local).AddTicks(5562), "https://i0.wp.com/insideflyer.com/wp-content/uploads/2022/09/Review-M-Social-Times-Square-New-York-City-USA-VS-46-scaled.jpeg" });

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2023, 9, 23, 15, 58, 3, 590, DateTimeKind.Local).AddTicks(5564), "https://hips.hearstapps.com/clv.h-cdn.co/assets/16/33/3200x2198/gallery-1471831519-iablackhawk.jpg" });

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2023, 9, 23, 15, 58, 3, 590, DateTimeKind.Local).AddTicks(5566), "https://www.wyndhamgrandclearwater.com/assets/images/escape-clearwater.png" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2023, 9, 10, 23, 33, 41, 930, DateTimeKind.Local).AddTicks(4827), "https://example.com/hotel1.jpg" });

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2023, 9, 10, 23, 33, 41, 930, DateTimeKind.Local).AddTicks(4842), "https://example.com/hotel2.jpg" });

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2023, 9, 10, 23, 33, 41, 930, DateTimeKind.Local).AddTicks(4844), "https://example.com/hotel3.jpg" });

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2023, 9, 10, 23, 33, 41, 930, DateTimeKind.Local).AddTicks(4846), "https://example.com/hotel4.jpg" });

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "ImageUrl" },
                values: new object[] { new DateTime(2023, 9, 10, 23, 33, 41, 930, DateTimeKind.Local).AddTicks(4848), "https://example.com/hotel5.jpg" });
        }
    }
}
