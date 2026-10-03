using Dimdim.Application.Interfaces;
using Dimdim.Domain.Entities;
using Dimdim.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Dimdim.Infrastructure.Repositories;

public class PedidoRepository(AppDbContext db) : IPedidoRepository
{
    public Task<List<Pedido>> ListarAsync() =>
        db.Pedidos.Include(x => x.Itens).AsNoTracking().ToListAsync();

    public Task<Pedido?> ObterAsync(Guid id) =>
        db.Pedidos.Include(x => x.Itens).FirstOrDefaultAsync(x => x.Id == id);

    public async Task AdicionarAsync(Pedido pedido) => await db.Pedidos.AddAsync(pedido);
    public Task AtualizarAsync(Pedido pedido) { db.Pedidos.Update(pedido); return Task.CompletedTask; }
    public Task RemoverAsync(Pedido pedido) { db.Pedidos.Remove(pedido); return Task.CompletedTask; }
    public Task SalvarAsync() => db.SaveChangesAsync();
}
