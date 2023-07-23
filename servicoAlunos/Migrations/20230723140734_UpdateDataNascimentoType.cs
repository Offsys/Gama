using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace servicoAlunos.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDataNascimentoType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        migrationBuilder.AlterColumn<DateTime>(
            name: "Data_Nascimento",
            table: "Alunos",
            type: "date",
            nullable: true,
            oldClrType: typeof(DateTime),
            oldType: "timestamp with time zone",
            oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
