using Basket.Application.Responses;
using Basket.Core.Entities;
using MediatR;

namespace Basket.Application.Commands;

public class CreateShoppingCartCommand : IRequest<ShoppingCartResponse>
{
    public string UserName { get; set; }
    public IList<ShoppingCartItem> Items { get; set; }

    public CreateShoppingCartCommand(string userName, IList<ShoppingCartItem> items)
    {
        UserName = userName;
        Items = items;
    }
}