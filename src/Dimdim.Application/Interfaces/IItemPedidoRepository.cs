using Dimdim.Domain.Entities;

namespace Dimdim.Application.Interfaces;

public interface IItemPedidoRepository
{
    Task<List<ItemPedido>> ListarAsync();
    Task<ItemPedido?> ObterAsync(Guid id);
    Task AdicionarAsync(ItemPedido item);
    Task AtualizarAsync(ItemPedido item);
    Task RemoverAsync(ItemPedido item);
    Task SalvarAsync();
}
