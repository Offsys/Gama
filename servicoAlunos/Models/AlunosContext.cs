using Microsoft.EntityFrameworkCore;
using servicoAlunos.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;


namespace servicoAlunos.Models
{
    public class AlunosContext : DbContext
    {
        public AlunosContext(DbContextOptions<AlunosContext> options)
            : base(options)
        {
        }

        public DbSet<Aluno> Alunos { get; set; }

        // Aqui você pode adicionar outras tabelas do banco de dados, se necessário

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Defina configurações adicionais do modelo aqui, se necessário

            // Configurar propriedades DateTime para armazenar em UTC automaticamente
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(new ValueConverter<DateTime, DateTime>(
                            v => v.ToUniversalTime(),
                            v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                        ));
                    }
                }
            }
        }
    }
}
