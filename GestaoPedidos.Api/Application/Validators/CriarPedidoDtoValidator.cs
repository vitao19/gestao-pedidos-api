using FluentValidation;
using GestaoPedidos.Api.Application.DTOs;

namespace GestaoPedidos.Api.Application.Validators;

public class CriarPedidoDtoValidator : AbstractValidator<CriarPedidoDto>
{
    public CriarPedidoDtoValidator()
    {
        RuleFor(x => x.ClienteNome)
            .NotEmpty()
            .WithMessage("O nome do cliente é obrigatório.");

        RuleFor(x => x.ClienteNome)
            .NotEmpty().WithMessage("O nome do cliente é obrigatório.")
            .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Itens)
            .NotEmpty()
            .WithMessage("O pedido deve possuir ao menos um item.");

        RuleForEach(x => x.Itens)
            .SetValidator(new CriarItemPedidoDtoValidator());
    }
}
