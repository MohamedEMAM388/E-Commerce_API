using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixOrderItemsRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "ItemsOrder",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemsOrder_OrderId",
                table: "ItemsOrder",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemsOrder_Order_OrderId",
                table: "ItemsOrder",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemsOrder_Order_OrderId",
                table: "ItemsOrder");

            migrationBuilder.DropIndex(
                name: "IX_ItemsOrder_OrderId",
                table: "ItemsOrder");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "ItemsOrder");
        }
    }
}
