using Dimdim.Application.DTOs;
using Dimdim.Application.Interfaces;
using Dimdim.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Dimdim.Api.Controllers;

[ApiController]
[Route("api/itens-pedido")]
public class ItensPedidoController(IItemPedidoRepository repository, IPedidoRepository pedidoRepository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await repository.ListarAsync());

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var item = await repository.ObterAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Post(ItemPedidoCreateDto dto)
    {
        var pedido = await pedidoRepository.ObterAsync(dto.PedidoId);
        if (pedido is null)
            return NotFound(new { mensagem = "Pedido informado não existe." });

        var item = new ItemPedido(dto.PedidoId, dto.Descricao, dto.Quantidade, dto.PrecoUnitario);
        await repository.AdicionarAsync(item);

        pedido.Itens.Add(item);
        pedido.RecalcularTotal();
        await pedidoRepository.AtualizarAsync(pedido);
        await repository.SalvarAsync();

        return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, ItemPedidoUpdateDto dto)
    {
        var item = await repository.ObterAsync(id);
        if (item is null) return NotFound();

        var pedido = await pedidoRepository.ObterAsync(item.PedidoId);
        if (pedido is null) return NotFound();

        var novo = new ItemPedido(item.PedidoId, dto.Descricao, dto.Quantidade, dto.PrecoUnitario);

        await repository.RemoverAsync(item);
        await repository.AdicionarAsync(novo);

        pedido.Itens.Remove(item);
        pedido.Itens.Add(novo);
        pedido.RecalcularTotal();

        await pedidoRepository.AtualizarAsync(pedido);
        await repository.SalvarAsync();

        return Ok(novo);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var item = await repository.ObterAsync(id);
        if (item is null) return NotFound();

        var pedido = await pedidoRepository.ObterAsync(item.PedidoId);
        if (pedido is null) return NotFound();

        await repository.RemoverAsync(item);
        pedido.Itens.Remove(item);
        pedido.RecalcularTotal();
        await pedidoRepository.AtualizarAsync(pedido);
        await repository.SalvarAsync();

        return NoContent();
    }
}
