using GestaoPedidos.Api.Application.DTOs;
using GestaoPedidos.Api.Application.Interfaces;
using GestaoPedidos.Api.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace GestaoPedidos.Api.Presentation.Controllers;

[ApiController]
[Route("pedidos")]
public class PedidosController : ControllerBase
{
    private readonly IPedidoService _pedidoService;

    public PedidosController(IPedidoService pedidoService)
    {
        _pedidoService = pedidoService;
    }

    [HttpPost]
    public async Task<IActionResult> Criar(
        [FromBody] CriarPedidoDto dto)
    {
        var id = await _pedidoService.CriarAsync(dto);

        return CreatedAtAction(
            nameof(ObterPorId),
            new { id },
            new { id });
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PedidoDetalheDto>> ObterPorId(Guid id)
    {
        var pedido = await _pedidoService.ObterPorIdAsync(id);

        if (pedido is null)
            return NotFound();

        return Ok(pedido);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<PedidoListagemDto>>>
        ObterPorStatus(
            [FromQuery] StatusPedido? status,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
    {
        var pedidos = await _pedidoService
            .ObterPorStatusAsync(status, page, pageSize);

        return Ok(pedidos);
    }

    [HttpPatch("{id:guid}/pagar")]
    public async Task<IActionResult> Pagar(Guid id)
    {
        await _pedidoService.PagarAsync(id);

        return NoContent();
    }

    [HttpPatch("{id:guid}/cancelar")]
    public async Task<IActionResult> Cancelar(Guid id)
    {
        await _pedidoService.CancelarAsync(id);

        return NoContent();
    }
}