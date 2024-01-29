using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpedienteMedico.Migrations
{
    public partial class droppingSsupended : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isSuspend",
                table: "MedicalHistoryTreatments");

            migrationBuilder.DropColumn(
                name: "isSuspend",
                table: "MedicalHistorySufferings");

            migrationBuilder.DropColumn(
                name: "isSuspend",
                table: "MedicalHistoryMedicines");

            migrationBuilder.AddColumn<bool>(
                name: "IsSuspended",
                table: "Treatments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSuspended",
                table: "Sufferings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSuspended",
                table: "Medicines",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSuspended",
                table: "Treatments");

            migrationBuilder.DropColumn(
                name: "IsSuspended",
                table: "Sufferings");

            migrationBuilder.DropColumn(
                name: "IsSuspended",
                table: "Medicines");

            migrationBuilder.AddColumn<bool>(
                name: "isSuspend",
                table: "MedicalHistoryTreatments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isSuspend",
                table: "MedicalHistorySufferings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isSuspend",
                table: "MedicalHistoryMedicines",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
