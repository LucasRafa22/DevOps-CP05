namespace Dimdim.Application.DTOs;

public record PedidoCreateDto(string ClienteNome);
public record PedidoUpdateDto(string ClienteNome);
public record ItemPedidoCreateDto(Guid PedidoId, string Descricao, int Quantidade, decimal PrecoUnitario);
public record ItemPedidoUpdateDto(string Descricao, int Quantidade, decimal PrecoUnitario);
