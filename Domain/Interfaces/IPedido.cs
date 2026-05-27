using GestaoPedidos.Api.Domain.Entities;
using GestaoPedidos.Api.Domain.Enums;

namespace GestaoPedidos.Api.Domain.Interfaces;

public interface IPedidoRepository
{
    Task<PedidoDetalheDto?> ObterPorIdAsync(Guid id);

    Task<PagedResponse<PedidoListagemDto>> ObterPorStatusAsync(
            StatusPedido? status,
            int page,
            int pageSize);

    Task CriarAsync(Pedido pedido);

    Task SalvarAlteracoesAsync();
}