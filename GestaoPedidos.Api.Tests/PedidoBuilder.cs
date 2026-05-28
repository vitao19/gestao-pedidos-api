using GestaoPedidos.Api.Domain.Entities;

namespace GestaoPedidos.Api.Tests.Builders;

public class PedidoBuilder
{
    private string _cliente = "Cliente Padrão";
    private List<ItemPedido> _itens = new();

    public static PedidoBuilder Novo() => new PedidoBuilder();

    public PedidoBuilder ComCliente(string cliente)
    {
        _cliente = cliente;
        return this;
    }

    public PedidoBuilder ComItem(string produto, int quantidade, decimal valor)
    {
        _itens.Add(new ItemPedido(produto, quantidade, valor));
        return this;
    }

    public PedidoBuilder ComItens(List<ItemPedido> itens)
    {
        _itens = itens;
        return this;
    }

    public Pedido Build()
    {
        return new Pedido(_cliente, _itens);
    }
}