using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilyBudget.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedExpenseSharingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_AspNetUsers_FamilyBudgetUserId",
                table: "Expenses");

            migrationBuilder.AlterColumn<Guid>(
                name: "FamilyBudgetUserId",
                table: "Expenses",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateTable(
                name: "UserExpenseSharingOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FamilyBudgetUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SharedWithUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserExpenseSharingOptions", x => x.Id);
                    table.UniqueConstraint("AK_UserExpenseSharingOptions_FamilyBudgetUserId_SharedWithUserId", x => new { x.FamilyBudgetUserId, x.SharedWithUserId });
                    table.ForeignKey(
                        name: "FK_UserExpenseSharingOptions_AspNetUsers_FamilyBudgetUserId",
                        column: x => x.FamilyBudgetUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserExpenseSharingOptions_AspNetUsers_SharedWithUserId",
                        column: x => x.SharedWithUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserExpenseSharingOptions_SharedWithUserId",
                table: "UserExpenseSharingOptions",
                column: "SharedWithUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_AspNetUsers_FamilyBudgetUserId",
                table: "Expenses",
                column: "FamilyBudgetUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_AspNetUsers_FamilyBudgetUserId",
                table: "Expenses");

            migrationBuilder.DropTable(
                name: "UserExpenseSharingOptions");

            migrationBuilder.AlterColumn<Guid>(
                name: "FamilyBudgetUserId",
                table: "Expenses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_AspNetUsers_FamilyBudgetUserId",
                table: "Expenses",
                column: "FamilyBudgetUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
