using FluentAssertions;
using GestaoPedidos.Api.Domain.Entities;
using GestaoPedidos.Api.Tests.Builders;
using Xunit;

namespace GestaoPedidos.Api.Tests;

public class PedidoTests
{
    [Fact]
    public void Pedido_SemItens_DeveLancarExcecao()
    {
        var act = () => PedidoBuilder.Novo()
            .ComCliente("João")
            .Build();

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("*pelo menos um item*");
    }

    [Fact]
    public void Pedido_DeveCalcularValorTotalCorretamente()
    {
        var pedido = PedidoBuilder.Novo()
            .ComCliente("João")
            .ComItem("Mouse", 2, 100)
            .ComItem("Teclado", 1, 200)
            .Build();

        pedido.ValorTotal.Should().Be(400);
    }

    [Fact]
    public void Pedido_Pago_NaoPodeSerCancelado()
    {
        var pedido = PedidoBuilder.Novo()
            .ComCliente("João")
            .ComItem("Mouse", 1, 100)
            .Build();

        pedido.MarcarComoPago();

        var act = () => pedido.Cancelar();

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("*não pode ser cancelado*");
    }
}