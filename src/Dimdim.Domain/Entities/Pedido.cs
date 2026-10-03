namespace Dimdim.Domain.Entities;

public class Pedido
{
    private Pedido() { }

    public Pedido(string clienteNome)
    {
        if (string.IsNullOrWhiteSpace(clienteNome))
            throw new ArgumentException("Nome do cliente é obrigatório.");

        Id = Guid.NewGuid();
        ClienteNome = clienteNome.Trim();
        DataCriacao = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public string ClienteNome { get; private set; } = string.Empty;
    public DateTime DataCriacao { get; private set; }
    public decimal ValorTotal { get; private set; }
    public ICollection<ItemPedido> Itens { get; private set; } = new List<ItemPedido>();

    public void AtualizarCliente(string clienteNome)
    {
        if (string.IsNullOrWhiteSpace(clienteNome))
            throw new ArgumentException("Nome do cliente é obrigatório.");

        ClienteNome = clienteNome.Trim();
    }

    public void RecalcularTotal()
    {
        ValorTotal = Itens.Sum(i => i.Quantidade * i.PrecoUnitario);
    }
}
