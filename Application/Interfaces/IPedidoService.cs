using GestaoPedidos.Api.Application.DTOs;
using GestaoPedidos.Api.Domain.Enums;

namespace GestaoPedidos.Api.Application.Interfaces;

public interface IPedidoService
{
    Task<Guid> CriarAsync(CriarPedidoDto dto);

    Task<PedidoDetalheDto?> ObterPorIdAsync(Guid id);

    Task<PagedResponse<PedidoListagemDto>> ObterPorStatusAsync(
        StatusPedido? status,
        int page,
        int pageSize);

    Task CancelarAsync(Guid id);
}
