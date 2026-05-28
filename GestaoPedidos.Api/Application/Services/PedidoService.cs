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
    private readonly IUnitOfWork _uow;
    private readonly IValidator<CriarPedidoDto> _validator;
    private readonly ILogger<PedidoService> _logger;

    public PedidoService(
        IPedidoRepository repository,
        IUnitOfWork uow,
        IValidator<CriarPedidoDto> validator,
        ILogger<PedidoService> logger)
    {
        _repository = repository;
        _uow = uow;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Guid> CriarAsync(CriarPedidoDto dto)
    {
        _logger.LogInformation("Iniciando criação de pedido para cliente {Cliente}", dto.ClienteNome);

        var validacao = await _validator.ValidateAsync(dto);
        if (!validacao.IsValid)
        {
            _logger.LogWarning("Validação falhou ao criar pedido para {Cliente}", dto.ClienteNome);
            throw new ValidationException(validacao.Errors);
        }

        var itens = dto.Itens.Select(x =>
            new ItemPedido(x.ProdutoNome, x.Quantidade, x.PrecoUnitario))
            .ToList();

        var pedido = new Pedido(dto.ClienteNome, itens);

        await _repository.CriarAsync(pedido);
        await _uow.CommitAsync();

        _logger.LogInformation(
            "Pedido {PedidoId} criado com sucesso para cliente {Cliente} com {QuantidadeItens} itens",
            pedido.Id,
            dto.ClienteNome,
            itens.Count);

        return pedido.Id;
    }

    public async Task<PedidoDetalheDto?> ObterPorIdAsync(Guid id)
    {
        return await _repository.ObterPorIdAsync(id);
    }

    public async Task<PagedResponse<PedidoListagemDto>> ObterPorStatusAsync(
    StatusPedido? status,
    int page,
    int pageSize)
    {
        _logger.LogInformation(
            "Buscando pedidos. Status: {Status}, Page: {Page}, PageSize: {PageSize}",
            status,
            page,
            pageSize);

        page = page <= 0 ? 1 : page;
        pageSize = pageSize <= 0 ? 10 : pageSize;

        return await _repository.ObterPorStatusAsync(status, page, pageSize);
    }

    public async Task PagarAsync(Guid id)
    {
        _logger.LogInformation("Iniciando pagamento do pedido {PedidoId}", id);

        var pedido = await _repository.ObterEntidadePorIdAsync(id);

        if (pedido is null)
        {
            _logger.LogWarning("Pedido {PedidoId} não encontrado para pagamento", id);
            throw new KeyNotFoundException("Pedido não encontrado.");
        }

        pedido.MarcarComoPago();

        await _uow.CommitAsync();

        _logger.LogInformation("Pedido {PedidoId} marcado como pago", id);
    }

    public async Task CancelarAsync(Guid id)
    {
        _logger.LogInformation("Iniciando cancelamento do pedido {PedidoId}", id);

        var pedido = await _repository.ObterEntidadePorIdAsync(id);

        if (pedido is null)
        {
            _logger.LogWarning("Pedido {PedidoId} não encontrado para cancelamento", id);
            throw new KeyNotFoundException("Pedido não encontrado.");
        }

        pedido.Cancelar();

        await _uow.CommitAsync();

        _logger.LogInformation("Pedido {PedidoId} cancelado com sucesso", id);
    }
}
