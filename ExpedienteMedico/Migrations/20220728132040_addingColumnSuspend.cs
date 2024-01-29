using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpedienteMedico.Migrations
{
    public partial class addingColumnSuspend : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
