using Basket.Application.Responses;
using MediatR;

namespace Basket.Application.Queries;

public class GetBasketByUserNameQuery : IRequest<ShoppingCartResponse>
{
    public GetBasketByUserNameQuery(string userName)
    {
        UserName = userName;
    }

    public string UserName { get; set; }
}