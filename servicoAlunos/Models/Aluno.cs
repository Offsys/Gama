using System;
using System.ComponentModel.DataAnnotations;

namespace servicoAlunos.Models
{
    public class Aluno
    {
        // outras propriedades existentes
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }

        // Nova propriedade para a data de nascimento
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Data_Nascimento { get; set; }

        public int Curso_Id { get; set; } // Valor padrão: 0
        public bool Libera_Conteudo { get; set; } // Valor padrão: false

        // Construtor com valores padrão, incluindo a Data_Nascimento
        public Aluno()
        {
            Nome = string.Empty;
            CPF = string.Empty;
            Email = string.Empty;
            Data_Nascimento = DateTime.MinValue; // Valor padrão: data mínima possível
            Curso_Id = 0; // Valor padrão: 0
            Libera_Conteudo = false; // Valor padrão: false
        }
    }
}
