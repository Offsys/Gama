using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace servicoAlunos.Models
{
    public class Aluno
    {
        // outras propriedades existentes
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }

        [Column("Data_Nascimento")]
        
        public DateTime Data_Nascimento_UTC { get; set; }

        public int Curso_Id { get; set; } // Valor padrão: 0
        public bool Libera_Conteudo { get; set; } // Valor padrão: false

        // Construtor com valores padrão, incluindo a Data_Nascimento_UTC
        public Aluno()
        {
            Nome = string.Empty;
            CPF = string.Empty;
            Email = string.Empty;
            Data_Nascimento_UTC = DateTime.MinValue.ToUniversalTime(); // Valor padrão: data mínima possível em UTC
            Curso_Id = 0; // Valor padrão: 0
            Libera_Conteudo = false; // Valor padrão: false
        }
    }
}
