namespace GestaoPedidos.Api.Application.DTOs;

public record PedidoDetalheDto(
    Guid Id,
    string ClienteNome,
    DateTime DataCriacao,
    string Status,
    decimal ValorTotal,
    List<ItemPedidoDto> Itens
);
