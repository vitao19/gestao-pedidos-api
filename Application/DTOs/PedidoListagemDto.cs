namespace GestaoPedidos.Api.Application.DTOs;

public record PedidoListagemDto(
    Guid Id,
    string ClienteNome,
    DateTime DataCriacao,
    string Status,
    decimal ValorTotal
);

