using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data_Access_Layer.Migrations
{
    public partial class addedReimbCol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reimbursements",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    ReimbursementType = table.Column<string>(nullable: false),
                    RequestedValue = table.Column<int>(nullable: false),
                    ApprovedValue = table.Column<int>(nullable: false),
                    Currency = table.Column<string>(nullable: false),
                    RequestPhase = table.Column<int>(nullable: false),
                    ReceiptAttached = table.Column<bool>(nullable: false),
                    ReceiptFile = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    ApprovedOrRejectedBy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reimbursements", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reimbursements");
        }
    }
}
