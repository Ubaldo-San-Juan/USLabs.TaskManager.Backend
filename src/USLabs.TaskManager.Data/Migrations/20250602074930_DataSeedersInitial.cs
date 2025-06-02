using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace USLabs.TaskManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class DataSeedersInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "CreatedAt", "Email", "FirstName", "IsActive", "LastName", "PasswordHash", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("25729196-b379-48e7-8783-448743f3a901"), new DateTime(2025, 6, 2, 7, 49, 29, 698, DateTimeKind.Utc).AddTicks(2400), "Llewellyn3@hotmail.com", "Abraham", true, "Beier", "pnv3rR9ljO", null },
                    { new Guid("55b988ee-9a25-493c-9efe-6ae7a532edd5"), new DateTime(2025, 6, 2, 7, 49, 29, 698, DateTimeKind.Utc).AddTicks(2270), "Derrick50@hotmail.com", "Rodolfo", true, "Schowalter", "kh2f61zoyh", null },
                    { new Guid("5b68c1ab-31bf-4b07-bb2a-2a5110c9177f"), new DateTime(2025, 6, 2, 7, 49, 29, 698, DateTimeKind.Utc).AddTicks(780), "Alf_Jenkins@yahoo.com", "Lauren", true, "Pouros", "m5iJOS1KfJ", null },
                    { new Guid("c3d0dd9f-a983-4ed6-a976-6a7c79affe82"), new DateTime(2025, 6, 2, 7, 49, 29, 698, DateTimeKind.Utc).AddTicks(2130), "Damian88@yahoo.com", "Gussie", true, "MacGyver", "slFEMNMdG8", null }
                });

            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "Id", "Color", "CreatedAt", "Description", "Name", "UserId" },
                values: new object[,]
                {
                    { new Guid("4bc64640-672b-4358-a27e-0ff93cae0def"), "#465433", new DateTime(2025, 6, 2, 7, 49, 29, 702, DateTimeKind.Utc).AddTicks(8910), "Quod alias omnis odit ad qui cupiditate.", "Tools", new Guid("c3d0dd9f-a983-4ed6-a976-6a7c79affe82") },
                    { new Guid("54389d4b-0b69-4ca5-8bfd-53846092d09d"), "#392455", new DateTime(2025, 6, 2, 7, 49, 29, 701, DateTimeKind.Utc).AddTicks(4310), "Et esse voluptates totam harum quae.", "Electronics", new Guid("5b68c1ab-31bf-4b07-bb2a-2a5110c9177f") },
                    { new Guid("cc6a631e-8164-4d9a-8ebb-4e3a8ad7d9f9"), "#56276e", new DateTime(2025, 6, 2, 7, 49, 29, 704, DateTimeKind.Utc).AddTicks(3920), "Voluptas nesciunt sint velit ducimus temporibus non ut.", "Toys", new Guid("55b988ee-9a25-493c-9efe-6ae7a532edd5") },
                    { new Guid("dba4043b-7b29-4e5e-a9cb-7219d7987d73"), "#293d26", new DateTime(2025, 6, 2, 7, 49, 29, 705, DateTimeKind.Utc).AddTicks(9210), "Nemo fuga et error deleniti.", "Sports", new Guid("25729196-b379-48e7-8783-448743f3a901") }
                });

            migrationBuilder.InsertData(
                table: "task_items",
                columns: new[] { "Id", "CategoryId", "CompletedAt", "CreatedAt", "Description", "DueDate", "Priority", "Status", "Title", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { new Guid("0e15410f-ed52-49ef-8c0e-c2816b2fb732"), new Guid("cc6a631e-8164-4d9a-8ebb-4e3a8ad7d9f9"), null, new DateTime(2025, 6, 2, 7, 49, 29, 717, DateTimeKind.Utc).AddTicks(4760), "Aperiam id non aliquam voluptas aut. Non et nulla facere vel. Facere molestiae odio voluptatem ipsa iste vero molestiae recusandae vero. Eos ut neque quae odit occaecati. Exercitationem beatae magnam suscipit et non ab ratione quibusdam excepturi. Est est accusamus autem cumque quod.", new DateTime(2025, 8, 19, 11, 41, 4, 917, DateTimeKind.Local).AddTicks(3530), 2, 1, "Aut asperiores nihil.", null, new Guid("55b988ee-9a25-493c-9efe-6ae7a532edd5") },
                    { new Guid("3934341d-d572-4306-8a84-d5bb8270a4dd"), new Guid("4bc64640-672b-4358-a27e-0ff93cae0def"), null, new DateTime(2025, 6, 2, 7, 49, 29, 714, DateTimeKind.Utc).AddTicks(820), "Laborum inventore porro provident excepturi dicta qui accusantium et pariatur. Commodi qui reiciendis sit quam cum nobis maxime illum. Vel voluptatum totam doloremque laudantium consequatur sapiente suscipit sed eos. Impedit accusantium rerum ut eveniet sunt id. Natus accusantium ea temporibus occaecati dolorem.", new DateTime(2026, 2, 21, 11, 56, 18, 170, DateTimeKind.Local).AddTicks(2132), 1, 3, "Ratione fugit ut.", null, new Guid("c3d0dd9f-a983-4ed6-a976-6a7c79affe82") },
                    { new Guid("65fb15fe-3ee2-4d7d-8563-82344af2221c"), new Guid("dba4043b-7b29-4e5e-a9cb-7219d7987d73"), null, new DateTime(2025, 6, 2, 7, 49, 29, 719, DateTimeKind.Utc).AddTicks(640), "Consectetur laboriosam porro. Itaque debitis qui quia non autem. Sed nesciunt laboriosam. Sunt voluptatem maxime eum consequatur vero iste ipsam ea. Ut doloremque aliquid iure id praesentium natus. Architecto sapiente sit deserunt.", new DateTime(2025, 7, 1, 0, 28, 55, 584, DateTimeKind.Local).AddTicks(3562), 1, 4, "Eaque ut praesentium.", null, new Guid("25729196-b379-48e7-8783-448743f3a901") },
                    { new Guid("a8dd6978-4e78-4a21-aedf-db2bff27a6e5"), new Guid("cc6a631e-8164-4d9a-8ebb-4e3a8ad7d9f9"), null, new DateTime(2025, 6, 2, 7, 49, 29, 715, DateTimeKind.Utc).AddTicks(7770), "Cumque vel laboriosam quia nam sit aspernatur. Quidem rerum qui inventore quia autem iste voluptatem. Consequatur velit blanditiis facilis.", new DateTime(2025, 9, 19, 14, 10, 38, 626, DateTimeKind.Local).AddTicks(659), 1, 4, "Perferendis est natus.", null, new Guid("55b988ee-9a25-493c-9efe-6ae7a532edd5") },
                    { new Guid("abbc5942-f4c7-4944-8481-df1a5eef661e"), new Guid("54389d4b-0b69-4ca5-8bfd-53846092d09d"), null, new DateTime(2025, 6, 2, 7, 49, 29, 710, DateTimeKind.Utc).AddTicks(7480), "Sit doloribus consectetur at nesciunt omnis occaecati non. Id illo quas optio possimus exercitationem voluptas excepturi occaecati. Voluptatem omnis velit. Sint ut quo sint accusantium nobis ex debitis soluta. Est totam sed illum libero sint. Vero quos magnam quam et dolorem.", new DateTime(2026, 3, 31, 19, 43, 39, 675, DateTimeKind.Local).AddTicks(9352), 3, 2, "Excepturi atque doloribus.", null, new Guid("5b68c1ab-31bf-4b07-bb2a-2a5110c9177f") },
                    { new Guid("b965b0a7-6f90-4bef-8066-8c86a8bf3636"), new Guid("dba4043b-7b29-4e5e-a9cb-7219d7987d73"), null, new DateTime(2025, 6, 2, 7, 49, 29, 720, DateTimeKind.Utc).AddTicks(7160), "Possimus voluptas ea quis nulla excepturi cumque doloribus. Ex iure sit voluptas dolor optio recusandae debitis est. Qui alias dolores officiis harum velit iure eum. Mollitia aspernatur voluptatem harum aut dignissimos omnis illum qui. Omnis asperiores at architecto dolorum ut dolorem debitis vero. Vel voluptatem nisi harum dolores consequuntur.", new DateTime(2026, 5, 1, 20, 29, 13, 920, DateTimeKind.Local).AddTicks(1258), 2, 3, "Et corporis et.", null, new Guid("25729196-b379-48e7-8783-448743f3a901") },
                    { new Guid("fbb20d46-e01e-41f9-bd49-9e739e982a95"), new Guid("54389d4b-0b69-4ca5-8bfd-53846092d09d"), null, new DateTime(2025, 6, 2, 7, 49, 29, 709, DateTimeKind.Utc).AddTicks(550), "Aliquam amet maiores omnis quod iste. Nobis aut excepturi esse quod rerum. Vel sed molestiae qui sint et alias minus. Impedit quis sapiente quis molestiae aut et consequatur. Culpa aut nostrum ipsa. Dolor eos eum eius.", new DateTime(2026, 4, 7, 6, 42, 59, 277, DateTimeKind.Local).AddTicks(4485), 2, 1, "Ducimus et qui.", null, new Guid("5b68c1ab-31bf-4b07-bb2a-2a5110c9177f") },
                    { new Guid("fc185c9f-ab03-4099-a545-0ff807f9ca46"), new Guid("4bc64640-672b-4358-a27e-0ff93cae0def"), null, new DateTime(2025, 6, 2, 7, 49, 29, 712, DateTimeKind.Utc).AddTicks(3440), "In ea excepturi. Eos perferendis et. Est ipsum deserunt non sunt reiciendis eligendi omnis. Ipsa ex consequatur. Atque consequatur voluptatem nesciunt in quia minus labore sit.", new DateTime(2025, 12, 10, 9, 55, 10, 943, DateTimeKind.Local).AddTicks(9826), 3, 4, "Nemo dolorem voluptatum.", null, new Guid("c3d0dd9f-a983-4ed6-a976-6a7c79affe82") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "task_items",
                keyColumn: "Id",
                keyValue: new Guid("0e15410f-ed52-49ef-8c0e-c2816b2fb732"));

            migrationBuilder.DeleteData(
                table: "task_items",
                keyColumn: "Id",
                keyValue: new Guid("3934341d-d572-4306-8a84-d5bb8270a4dd"));

            migrationBuilder.DeleteData(
                table: "task_items",
                keyColumn: "Id",
                keyValue: new Guid("65fb15fe-3ee2-4d7d-8563-82344af2221c"));

            migrationBuilder.DeleteData(
                table: "task_items",
                keyColumn: "Id",
                keyValue: new Guid("a8dd6978-4e78-4a21-aedf-db2bff27a6e5"));

            migrationBuilder.DeleteData(
                table: "task_items",
                keyColumn: "Id",
                keyValue: new Guid("abbc5942-f4c7-4944-8481-df1a5eef661e"));

            migrationBuilder.DeleteData(
                table: "task_items",
                keyColumn: "Id",
                keyValue: new Guid("b965b0a7-6f90-4bef-8066-8c86a8bf3636"));

            migrationBuilder.DeleteData(
                table: "task_items",
                keyColumn: "Id",
                keyValue: new Guid("fbb20d46-e01e-41f9-bd49-9e739e982a95"));

            migrationBuilder.DeleteData(
                table: "task_items",
                keyColumn: "Id",
                keyValue: new Guid("fc185c9f-ab03-4099-a545-0ff807f9ca46"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "Id",
                keyValue: new Guid("4bc64640-672b-4358-a27e-0ff93cae0def"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "Id",
                keyValue: new Guid("54389d4b-0b69-4ca5-8bfd-53846092d09d"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "Id",
                keyValue: new Guid("cc6a631e-8164-4d9a-8ebb-4e3a8ad7d9f9"));

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "Id",
                keyValue: new Guid("dba4043b-7b29-4e5e-a9cb-7219d7987d73"));

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("25729196-b379-48e7-8783-448743f3a901"));

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("55b988ee-9a25-493c-9efe-6ae7a532edd5"));

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("5b68c1ab-31bf-4b07-bb2a-2a5110c9177f"));

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("c3d0dd9f-a983-4ed6-a976-6a7c79affe82"));
        }
    }
}
