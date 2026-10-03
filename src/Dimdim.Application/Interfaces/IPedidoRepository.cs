using Dimdim.Domain.Entities;

namespace Dimdim.Application.Interfaces;

public interface IPedidoRepository
{
    Task<List<Pedido>> ListarAsync();
    Task<Pedido?> ObterAsync(Guid id);
    Task AdicionarAsync(Pedido pedido);
    Task AtualizarAsync(Pedido pedido);
    Task RemoverAsync(Pedido pedido);
    Task SalvarAsync();
}
