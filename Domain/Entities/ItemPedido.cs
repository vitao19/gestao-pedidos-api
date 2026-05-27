namespace GestaoPedidos.Api.Domain.Entities;

public class ItemPedido
{
    public int Id { get; set; }

    public Guid PedidoId { get; set; }

    public string ProdutoNome { get; set; } = string.Empty;

    public int Quantidade { get; set; }

    public decimal PrecoUnitario { get; set; }

    public Pedido Pedido { get; set; } = null!;
}