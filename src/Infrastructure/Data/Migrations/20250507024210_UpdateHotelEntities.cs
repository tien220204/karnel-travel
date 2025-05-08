using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace KarnelTravel.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHotelEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "HotelStyles");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "HotelStyles");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "HotelAmenities");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "HotelAmenities");

			migrationBuilder.DropColumn(
	            name: "PaymentTypes",
	            table: "Hotels");

			migrationBuilder.AddColumn<int>(
				name: "PaymentTypes",
				table: "Hotels",
				nullable: false,
				defaultValue: 0);

			migrationBuilder.AddColumn<long>(
                name: "PricePerHour",
                table: "HotelRooms",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "AmenityId",
                table: "HotelAmenities",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Amenities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Icon = table.Column<string>(type: "text", nullable: true),
                    AmenityType = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amenities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HotelAmenities_AmenityId",
                table: "HotelAmenities",
                column: "AmenityId");

            migrationBuilder.AddForeignKey(
                name: "FK_HotelAmenities_Amenities_AmenityId",
                table: "HotelAmenities",
                column: "AmenityId",
                principalTable: "Amenities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HotelAmenities_Amenities_AmenityId",
                table: "HotelAmenities");

            migrationBuilder.DropTable(
                name: "Amenities");

            migrationBuilder.DropIndex(
                name: "IX_HotelAmenities_AmenityId",
                table: "HotelAmenities");

            migrationBuilder.DropColumn(
                name: "PricePerHour",
                table: "HotelRooms");

            migrationBuilder.DropColumn(
                name: "AmenityId",
                table: "HotelAmenities");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "HotelStyles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "HotelStyles",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<int[]>(
                name: "PaymentTypes",
                table: "Hotels",
                type: "integer[]",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "HotelAmenities",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "HotelAmenities",
                type: "text",
                nullable: true);
        }
    }
}
