namespace GestaoPedidos.Api.Application.DTOs;

public record CriarItemPedidoDto(string ProdutoNome, int Quantidade, decimal PrecoUnitario);
