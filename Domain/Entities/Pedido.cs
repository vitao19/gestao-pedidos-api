namespace GestaoPedidos.Api.Domain.Entities;

public class Pedido
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string ClienteNome { get; set; } = string.Empty;

    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    public StatusPedido Status { get; set; } = StatusPedido.Novo;

    public decimal ValorTotal { get; set; }

    public List<ItemPedido> Itens { get; set; } = new();
}