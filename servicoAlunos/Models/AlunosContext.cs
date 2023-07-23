using Microsoft.EntityFrameworkCore;
using servicoAlunos.Models;

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
        }
    }
}
