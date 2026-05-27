using GestaoPedidos.Api.Domain.Enums;

namespace GestaoPedidos.Api.Domain.Entities;

public class Pedido
{
    public Guid Id { get; private set; }
    public string ClienteNome { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public StatusPedido Status { get; private set; }
    public decimal ValorTotal { get; private set; }

    private readonly List<ItemPedido> _itens = new();
    public IReadOnlyCollection<ItemPedido> Itens => _itens.AsReadOnly();
        
    protected Pedido() { }

    public Pedido(string clienteNome, List<ItemPedido> itens)
    {
        if (itens == null || !itens.Any())
            throw new ArgumentException("O pedido deve conter pelo menos um item.");

        Id = Guid.NewGuid();
        ClienteNome = clienteNome;
        DataCriacao = DateTime.UtcNow;
        Status = StatusPedido.Novo;

        foreach (var item in itens)
        {
            _itens.Add(item);
        }

        CalcularValorTotal();
    }

    private void CalcularValorTotal()
    {
        ValorTotal = _itens.Sum(item => item.Quantidade * item.PrecoUnitario);
    }

    public void MarcarComoPago()
    {
        Status = StatusPedido.Pago;
    }

    public void Cancelar()
    {
        if (Status == StatusPedido.Pago)
            throw new InvalidOperationException("Um pedido pago não pode ser cancelado.");

        Status = StatusPedido.Cancelado;
    }
}