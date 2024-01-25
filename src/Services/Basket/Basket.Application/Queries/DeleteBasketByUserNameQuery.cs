using MediatR;

namespace Basket.Application.Queries;

public class DeleteBasketByUserNameQuery : IRequest
{
    public DeleteBasketByUserNameQuery(string userName)
    {
        UserName = userName;
    }

    public string UserName { get; set; }
}