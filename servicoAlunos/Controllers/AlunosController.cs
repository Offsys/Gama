using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using servicoAlunos.Models;


[ApiController]
[Route("api/[controller]")]
public class AlunosController : ControllerBase
{
    private readonly AlunosContext _context;
    private readonly string _secretKey;

    public AlunosController(AlunosContext context, IConfiguration configuration)
    {
        _context = context;
        _secretKey = configuration["JwtSettings:SecretKey"] ?? throw new ArgumentException("JwtSettings:SecretKey não configurado no appsettings.json");
    }

    // Rota: POST api/alunos/insere_aluno
    [HttpPost("insere_aluno")]
    public async Task<IActionResult> InsereAluno([FromBody] Aluno aluno)
    {
        if (aluno == null)
        {
            return BadRequest("Dados inválidos para o aluno.");
        }

        // Verifica se Data_Nascimento é diferente de default(DateTime), então converte para Utc.
        if (aluno.Data_Nascimento_UTC != default(DateTime))
        {
            aluno.Data_Nascimento_UTC = DateTime.SpecifyKind(aluno.Data_Nascimento_UTC, DateTimeKind.Utc);
        }

        _context.Alunos.Add(aluno);
        await _context.SaveChangesAsync();

        return CreatedAtAction("ListaAlunos", new { id = aluno.Id }, aluno);
    }

    // Rota: GET api/alunos/lista_alunos
    [HttpGet("lista_alunos")]
    public async Task<IActionResult> ListaAlunos()
    {
        var alunos = await _context.Alunos.ToListAsync();
        return Ok(alunos);
    }

    // Rota: PUT api/alunos/atualiza_alunos/5
[HttpPut("atualiza_alunos/{id}")]
public async Task<IActionResult> AtualizaAlunos(int id, [FromBody] Aluno alunoAtualizado)
{
    if (alunoAtualizado == null || alunoAtualizado.Id != id)
    {
        return BadRequest("Dados inválidos para o aluno.");
    }

    var alunoExistente = await _context.Alunos.FindAsync(id);
    if (alunoExistente == null)
    {
        return NotFound();
    }

    // Atualize a propriedade Data_Nascimento com o Kind correto (UTC)
    alunoExistente.Nome = alunoAtualizado.Nome;
    alunoExistente.CPF = alunoAtualizado.CPF;
    alunoExistente.Email = alunoAtualizado.Email;
    alunoExistente.Curso_Id = alunoAtualizado.Curso_Id;
    alunoExistente.Libera_Conteudo = alunoAtualizado.Libera_Conteudo;
    alunoExistente.Data_Nascimento_UTC = DateTime.SpecifyKind(alunoAtualizado.Data_Nascimento_UTC, DateTimeKind.Utc);

    _context.Entry(alunoExistente).State = EntityState.Modified;
    await _context.SaveChangesAsync();

    return NoContent();
}







    // Rota: DELETE api/alunos/remove_alunos/5
    [HttpDelete("remove_alunos/{id}")]
    public async Task<IActionResult> RemoveAlunos(int id)
    {
        var aluno = await _context.Alunos.FindAsync(id);
        if (aluno == null)
        {
            return NotFound();
        }

        _context.Alunos.Remove(aluno);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("login_alunos")]
    public IActionResult Login([FromBody] LoginRequest loginRequest)
    {
        if (VerificarCredenciais(loginRequest.Email, loginRequest.CPF))
        {
            // Se as credenciais forem válidas, crie e retorne um token JWT para o usuário.
            var token = GerarTokenJWT(loginRequest.Email);

            return Ok(new { Token = token });
        }

        return Unauthorized("Credenciais inválidas. Verifique seu email e CPF.");
    }

    private bool VerificarCredenciais(string? email, string? cpf)
    {
        // Verifica se email e cpf não são nulos ou vazios
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(cpf))
            {
                return false;
            }

        // Realiza a consulta na tabela de alunos para verificar se existe um aluno com o email e cpf informados
        var aluno = _context.Alunos.FirstOrDefault(a => a.Email == email && a.CPF == cpf);

        // Retorna true se o aluno foi encontrado, indicando que as credenciais são válidas
        return aluno != null;
    }

private string GerarTokenJWT(string? email)
{
    if (string.IsNullOrEmpty(email))
    {
        return string.Empty; // Retorna uma string vazia para indicar que o email é inválido
    }

    var secretKey = _secretKey ?? throw new ArgumentNullException(nameof(_secretKey));
    var key = Encoding.ASCII.GetBytes(secretKey);
    var tokenHandler = new JwtSecurityTokenHandler();
    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Email, email)
        }),
        Expires = DateTime.UtcNow.AddDays(1),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };
    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
}
[HttpGet("gera_boletos/{id}")]
public IActionResult GeraBoletos(int id)
{
    var aluno = _context.Alunos.FirstOrDefault(a => a.Id == id);
    if (aluno == null)
    {
        return NotFound();
    }

    var response = new
    {
        aluno = new
        {
            id = aluno.Id,
            nome = aluno.Nome,
            cpf = aluno.CPF
        },
        curso = new
        {
            id = aluno.Curso_Id, // Usar a propriedade Curso_Id para obter o ID do curso associado ao aluno
            valor = "0.00" // Substitua "aluno.ValorCurso" pelo valor correto do curso associado ao aluno, caso exista essa propriedade na classe Aluno
        }
    };

    return Ok(response);
}
}
