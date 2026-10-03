using Dimdim.Application.DTOs;
using Dimdim.Application.Interfaces;
using Dimdim.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Dimdim.Api.Controllers;

[ApiController]
[Route("api/pedidos")]
public class PedidosController(IPedidoRepository repository, IItemPedidoRepository itemRepository, ILogger<PedidosController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await repository.ListarAsync());

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var pedido = await repository.ObterAsync(id);
        return pedido is null ? NotFound(new { mensagem = "Pedido não encontrado." }) : Ok(pedido);
    }

    [HttpPost]
    public async Task<IActionResult> Post(PedidoCreateDto dto)
    {
        var pedido = new Pedido(dto.ClienteNome);
        await repository.AdicionarAsync(pedido);
        await repository.SalvarAsync();

        logger.LogInformation("Pedido criado. Id={Id} TraceId={TraceId}", pedido.Id, HttpContext.TraceIdentifier);
        return CreatedAtAction(nameof(GetById), new { id = pedido.Id }, pedido);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, PedidoUpdateDto dto)
    {
        var pedido = await repository.ObterAsync(id);
        if (pedido is null) return NotFound();

        pedido.AtualizarCliente(dto.ClienteNome);
        pedido.RecalcularTotal();
        await repository.AtualizarAsync(pedido);
        await repository.SalvarAsync();

        return Ok(pedido);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var pedido = await repository.ObterAsync(id);
        if (pedido is null) return NotFound();

        await repository.RemoverAsync(pedido);
        await repository.SalvarAsync();
        return NoContent();
    }

    [HttpGet("{pedidoId:guid}/itens")]
    public async Task<IActionResult> GetItens(Guid pedidoId)
    {
        var pedido = await repository.ObterAsync(pedidoId);
        return pedido is null ? NotFound() : Ok(pedido.Itens);
    }

    [HttpPost("{pedidoId:guid}/itens")]
    public async Task<IActionResult> PostItem(Guid pedidoId, ItemPedidoCreateDto dto)
    {
        var pedido = await repository.ObterAsync(pedidoId);
        if (pedido is null) return NotFound(new { mensagem = "Pedido não encontrado." });

        var item = new ItemPedido(pedidoId, dto.Descricao, dto.Quantidade, dto.PrecoUnitario);
        pedido.Itens.Add(item);
        pedido.RecalcularTotal();

        await repository.AtualizarAsync(pedido);
        await repository.SalvarAsync();

        return CreatedAtAction(nameof(GetItens), new { pedidoId }, item);
    }
}
