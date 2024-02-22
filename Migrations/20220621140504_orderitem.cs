using Microsoft.EntityFrameworkCore.Migrations;

namespace Pharmacy.Migrations
{
    public partial class orderitem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
            name: "FK_OrderItem_Order",
            table: "OrderItems");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_Order",
                table: "OrderItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
           name: "FK_OrderItem_Order",
                table: "OrderItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Restrict);


            migrationBuilder.DropForeignKey(
             name: "FK_OrderItem_Order",
            table: "OrderItems"
               );
        }
    }
}
