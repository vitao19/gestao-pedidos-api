using FluentValidation;
using GestaoPedidos.Api.Application.DTOs;

namespace GestaoPedidos.Api.Application.Validators;

public class CriarItemPedidoDtoValidator
 : AbstractValidator<CriarItemPedidoDto>
{
    public CriarItemPedidoDtoValidator()
    {
        RuleFor(x => x.ProdutoNome)
            .NotEmpty()
            .WithMessage("O nome do produto é obrigatório.")
            .MaximumLength(150).WithMessage("O nome do produto deve ter no máximo 150 caracteres."); 

        RuleFor(x => x.Quantidade)
            .GreaterThan(0)
            .WithMessage("A quantidade deve ser maior que zero.");

        RuleFor(x => x.PrecoUnitario)
            .GreaterThan(0)
            .WithMessage("O preço unitário deve ser maior que zero.");
    }
}
