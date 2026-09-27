using Microsoft.EntityFrameworkCore;
using RaizesDoNordeste.Domain.Entities;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Infrastructure.Persistence.Repositories;

public class FuncionarioRepository : IFuncionarioRepository
{
    private readonly RaizesDoNordesteDbContext _context;

    public FuncionarioRepository(RaizesDoNordesteDbContext context)
    {
        _context = context;
    }

    public async Task<Funcionario> CriarAsync(Funcionario funcionario)
    {
        _context.Funcionario.Add(funcionario);
        await _context.SaveChangesAsync();
        return funcionario;
    }

    public async Task<Funcionario?> ObterPorIdAsync(long id)
    {
        return await _context.Funcionario.FindAsync(id);
    }

    public async Task<Funcionario?> ObterPorIdComDetalhesAsync(long id)
    {
        return await _context.Funcionario
            .Include(f => f.Unidade)
            .Include(f => f.Usuario)
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<IEnumerable<Funcionario>> ObterTodosAsync()
    {
        return await _context.Funcionario
            .Include(f => f.Unidade)
            .ToListAsync();
    }

    public async Task<IEnumerable<Funcionario>> ObterPorUnidadeAsync(long unidadeId)
    {
        return await _context.Funcionario
            .Include(f => f.Unidade)
            .Where(f => f.IdUnidade == unidadeId)
            .ToListAsync();
    }

    public async Task AtualizarAsync(Funcionario funcionario)
    {
        _context.Funcionario.Update(funcionario);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> CpfExisteAsync(string cpf, long? ignorarId = null)
    {
        var todos = await _context.Funcionario.ToListAsync();
        return todos.Any(f => f.Cpf == cpf && (!ignorarId.HasValue || f.Id != ignorarId.Value));
    }

    public async Task<bool> EmailExisteAsync(string email, long? ignorarId = null)
    {
        return await _context.Funcionario
            .AnyAsync(f => f.Email == email && (!ignorarId.HasValue || f.Id != ignorarId.Value));
    }
}