using FluentValidation;
using GestaoPedidos.Api.Application.DTOs;
using GestaoPedidos.Api.Application.Interfaces;
using GestaoPedidos.Api.Domain.Entities;
using GestaoPedidos.Api.Domain.Enums;
using GestaoPedidos.Api.Domain.Interfaces;

namespace GestaoPedidos.Api.Application.Services;

public class PedidoService : IPedidoService
{
    private readonly IPedidoRepository _repository;
    private readonly IValidator<CriarPedidoDto> _validator;

    public PedidoService(IPedidoRepository repository, IValidator<CriarPedidoDto> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Guid> CriarAsync(CriarPedidoDto dto)
    {
        var validacao = await _validator.ValidateAsync(dto);
        if (!validacao.IsValid)
            throw new ValidationException(validacao.Errors);

        var itens = dto.Itens.Select(x =>
            new ItemPedido(
                x.ProdutoNome,
                x.Quantidade,
                x.PrecoUnitario))
            .ToList();

        var pedido = new Pedido(dto.ClienteNome, itens);

        await _repository.CriarAsync(pedido);
        await _repository.SalvarAlteracoesAsync();

        return pedido.Id;
    }

    public async Task<PedidoDetalheDto?> ObterPorIdAsync(Guid id)
    {
        return await _repository.ObterPorIdAsync(id);
    }

    public async Task<PagedResponse<PedidoListagemDto>>
        ObterPorStatusAsync(
            StatusPedido? status,
            int page,
            int pageSize)
    {
        return await _repository
            .ObterPorStatusAsync(status, page, pageSize);
    }

    public async Task CancelarAsync(Guid id)
    {
        var pedido = await _repository.ObterEntidadePorIdAsync(id);

        if (pedido is null)
            throw new KeyNotFoundException("Pedido não encontrado.");

        pedido.Cancelar();

        await _repository.SalvarAlteracoesAsync();
    }
}
