using Dimdim.Application.Interfaces;
using Dimdim.Domain.Entities;
using Dimdim.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Dimdim.Infrastructure.Repositories;

public class ItemPedidoRepository(AppDbContext db) : IItemPedidoRepository
{
    public Task<List<ItemPedido>> ListarAsync() =>
        db.ItensPedido.AsNoTracking().ToListAsync();

    public Task<ItemPedido?> ObterAsync(Guid id) =>
        db.ItensPedido.FirstOrDefaultAsync(x => x.Id == id);

    public async Task AdicionarAsync(ItemPedido item) => await db.ItensPedido.AddAsync(item);
    public Task AtualizarAsync(ItemPedido item) { db.ItensPedido.Update(item); return Task.CompletedTask; }
    public Task RemoverAsync(ItemPedido item) { db.ItensPedido.Remove(item); return Task.CompletedTask; }
    public Task SalvarAsync() => db.SaveChangesAsync();
}
