using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportsFacility.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCourtsDirectFacilityLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Courts_CourtId",
                table: "Bookings");

            migrationBuilder.DropTable(
                name: "Courts");

            migrationBuilder.RenameColumn(
                name: "CourtId",
                table: "Bookings",
                newName: "FacilityId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_CourtId",
                table: "Bookings",
                newName: "IX_Bookings_FacilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Facilities_FacilityId",
                table: "Bookings",
                column: "FacilityId",
                principalTable: "Facilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Facilities_FacilityId",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "FacilityId",
                table: "Bookings",
                newName: "CourtId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_FacilityId",
                table: "Bookings",
                newName: "IX_Bookings_CourtId");

            migrationBuilder.CreateTable(
                name: "Courts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FacilityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courts_Facilities_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courts_FacilityId",
                table: "Courts",
                column: "FacilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Courts_CourtId",
                table: "Bookings",
                column: "CourtId",
                principalTable: "Courts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
