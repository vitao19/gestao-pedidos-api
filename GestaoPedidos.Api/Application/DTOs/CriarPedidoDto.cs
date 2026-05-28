namespace GestaoPedidos.Api.Application.DTOs;

public record CriarPedidoDto(string ClienteNome, List<CriarItemPedidoDto> Itens);
