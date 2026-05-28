namespace GestaoPedidos.Api.Domain.Entities;

public class ItemPedido
{
    public int Id { get; private set; }
    public Guid PedidoId { get; private set; }
    public string ProdutoNome { get; private set; } = null!;
    public int Quantidade { get; private set; }
    public decimal PrecoUnitario { get; private set; }

    protected ItemPedido() { }

    public ItemPedido(string produtoNome, int quantidade, decimal precoUnitario)
    {
        if (quantidade <= 0)
            throw new ArgumentException("A quantidade deve ser maior que zero.");

        ProdutoNome = produtoNome;
        Quantidade = quantidade;
        PrecoUnitario = precoUnitario;
    }
}