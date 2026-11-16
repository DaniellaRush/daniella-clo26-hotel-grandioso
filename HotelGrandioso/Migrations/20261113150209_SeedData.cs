using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hotel.Grandioso.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RoomTypes",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Standard room", "Standard" },
                    { 2, "Luxury suite", "Luxury" },
                    { 3, "Bridal suite", "Bridal" }
                });

            migrationBuilder.InsertData(
                table: "Treatments",
                columns: new[] { "Id", "DurationMinutes", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 60, "Entré (per hour)", 150m },
                    { 2, 60, "Massage (60 min)", 950m },
                    { 3, 30, "Pedicure (big toe)", 350m },
                    { 4, 60, "Pedicure (foot)", 650m }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Floor", "PricePerNight", "RoomNumber", "RoomTypeId", "SizeM2" },
                values: new object[,]
                {
                    { 1, 3, 1200m, 301, 1, 120 },
                    { 2, 3, 1200m, 302, 1, 120 },
                    { 3, 3, 1200m, 303, 1, 120 },
                    { 4, 3, 2800m, 304, 2, 180 },
                    { 5, 3, 3500m, 305, 3, 220 },
                    { 6, 2, 1000m, 201, 1, 100 },
                    { 7, 2, 1000m, 202, 1, 100 },
                    { 8, 2, 1000m, 203, 1, 100 },
                    { 9, 2, 1000m, 204, 1, 100 },
                    { 10, 2, 1000m, 205, 1, 100 },
                    { 11, 1, 800m, 101, 1, 80 },
                    { 12, 1, 800m, 102, 1, 80 },
                    { 13, 1, 800m, 103, 1, 80 },
                    { 14, 1, 800m, 104, 1, 80 },
                    { 15, 1, 800m, 105, 1, 80 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Treatments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Treatments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Treatments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Treatments",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
