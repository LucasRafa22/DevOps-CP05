namespace Dimdim.Domain.Entities;

public class ItemPedido
{
    private ItemPedido() { }

    public ItemPedido(Guid pedidoId, string descricao, int quantidade, decimal precoUnitario)
    {
        if (pedidoId == Guid.Empty) throw new ArgumentException("Pedido inválido.");
        if (string.IsNullOrWhiteSpace(descricao)) throw new ArgumentException("Descrição é obrigatória.");
        if (quantidade <= 0) throw new ArgumentException("Quantidade deve ser maior que zero.");
        if (precoUnitario < 0) throw new ArgumentException("Preço não pode ser negativo.");

        Id = Guid.NewGuid();
        PedidoId = pedidoId;
        Descricao = descricao.Trim();
        Quantidade = quantidade;
        PrecoUnitario = precoUnitario;
    }

    public Guid Id { get; private set; }
    public Guid PedidoId { get; private set; }
    public string Descricao { get; private set; } = string.Empty;
    public int Quantidade { get; private set; }
    public decimal PrecoUnitario { get; private set; }
}
