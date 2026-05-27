using GestaoPedidos.Api.Domain.Entities;
using GestaoPedidos.Api.Domain.Enums;
using GestaoPedidos.Api.Domain.Interfaces;
using GestaoPedidos.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestaoPedidos.Api.Infrastructure.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly AppDbContext _context;

        public PedidoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CriarAsync(Pedido pedido)
        {
            await _context.Pedidos.AddAsync(pedido);
        }

        public async Task<PedidoDetalheDto?> ObterPorIdAsync(Guid id)
        {
            return await _context.Pedidos
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new PedidoDetalheDto
                {
                    Id = x.Id,
                    ClienteNome = x.ClienteNome,
                    DataCriacao = x.DataCriacao,
                    Status = x.Status.ToString(),
                    ValorTotal = x.ValorTotal,

                    Itens = x.Itens.Select(i => new ItemPedidoDto
                    {
                        ProdutoNome = i.ProdutoNome,
                        Quantidade = i.Quantidade,
                        PrecoUnitario = i.PrecoUnitario
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<PagedResponse<PedidoListagemDto>> ObterPorStatusAsync(
            StatusPedido? status,
            int page,
            int pageSize)
        {
            var query = _context.Pedidos
                .AsNoTracking()
                .AsQueryable();

            if (status.HasValue)
                query = query.Where(x => x.Status == status);

            var totalItems = await query.CountAsync();

            var pedidos = await query
                .OrderByDescending(x => x.DataCriacao)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new PedidoListagemDto
                {
                    Id = x.Id,
                    ClienteNome = x.ClienteNome,
                    DataCriacao = x.DataCriacao,
                    Status = x.Status.ToString(),
                    ValorTotal = x.ValorTotal
                })
                .ToListAsync();

            return new PagedResponse<PedidoListagemDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                Data = pedidos
            };
        }

        public async Task SalvarAlteracoesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
