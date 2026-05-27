using GestaoPedidos.Api.Application.DTOs;
using GestaoPedidos.Api.Domain.Entities;
using GestaoPedidos.Api.Domain.Enums;

namespace GestaoPedidos.Api.Domain.Interfaces;

public interface IPedidoRepository
{
    Task CriarAsync(Pedido pedido);

    Task<Pedido?> ObterEntidadePorIdAsync(Guid id);

    Task<PedidoDetalheDto?> ObterPorIdAsync(Guid id);

    Task<PagedResponse<PedidoListagemDto>> ObterPorStatusAsync(
        StatusPedido? status,
        int page,
        int pageSize);

    Task SalvarAlteracoesAsync();
}