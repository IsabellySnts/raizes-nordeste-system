using Microsoft.EntityFrameworkCore;
using RaizesDoNordeste.Domain.Aggregates;
using RaizesDoNordeste.Domain.Interfaces.Repositories;

namespace RaizesDoNordeste.Infrastructure.Persistence.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly RaizesDoNordesteDbContext _context;

    public ClienteRepository(RaizesDoNordesteDbContext context)
    {
        _context = context;
    }

    public async Task<Cliente> CriarAsync(Cliente cliente)
    {
        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
        return cliente;
    }

    public async Task<Cliente?> ObterPorIdAsync(long id)
    {
        return await _context.Clientes.FindAsync(id);
    }

    public async Task<Cliente?> ObterPorIdComUsuarioAsync(long id)
    {
        return await _context.Clientes
            .Include(c => c.Usuario)
            .Include(c => c.Fidelidade)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Cliente?> ObterPorCpfAsync(string cpf)
    {
        var todosClientes = await _context.Clientes.ToListAsync();
        return todosClientes.FirstOrDefault(c => c.Cpf == cpf);
    }

    public async Task<IEnumerable<Cliente>> ObterTodosAsync()
    {
        return await _context.Clientes
            .Include(c => c.Fidelidade)
            .ToListAsync();
    }

    public async Task AtualizarAsync(Cliente cliente)
    {
        _context.Clientes.Update(cliente);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> CpfExisteAsync(string cpf, long? ignorarId = null)
    {
        var todosClientes = await _context.Clientes.ToListAsync();
        return todosClientes.Any(c => c.Cpf == cpf && (!ignorarId.HasValue || c.Id != ignorarId.Value));
    }

    public async Task<bool> EmailExisteAsync(string email, long? ignorarId = null)
    {
        return await _context.Clientes
            .AnyAsync(c => c.Email == email && (!ignorarId.HasValue || c.Id != ignorarId.Value));
    }
}
