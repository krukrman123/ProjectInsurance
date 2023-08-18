using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectInsurance.Migrations
{
    public partial class AddInsuranceEventToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsuranceId = table.Column<int>(type: "int", nullable: false),
                    PolicyHolderId = table.Column<int>(type: "int", nullable: false),
                    PolicyHolderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PolicyHolderLastname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InsuranceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InsuranceSubject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InsuranceEventTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InsuranceEvent_InsuranceId",
                        column: x => x.InsuranceId,
                        principalTable: "Insurance",
                        principalColumn: "Id",
                        onUpdate: ReferentialAction.Cascade,
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InsuranceEvent_PolicyHolderId",
                        column: x => x.PolicyHolderId,
                        principalTable: "Insured",
                        principalColumn: "Id",
                        onUpdate: ReferentialAction.NoAction,
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceEvent_InsuranceId",
                table: "Event",
                column: "InsuranceId");

            migrationBuilder.CreateIndex(
               name: "IX_InsuranceEvent_PolicyHolderId",
               table: "Event",
               column: "PolicyHolderId");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Event");
        }
    }
}
