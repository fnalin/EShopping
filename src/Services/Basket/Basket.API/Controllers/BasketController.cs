using System.Net;
using Basket.Application.Commands;
using Basket.Application.GrpcServices;
using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Entities;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers;

public class BasketController : ApiController
{
    private readonly IMediator _mediator;
    private readonly DiscountGrpcServices _discountGrpcServices;
    private readonly IPublishEndpoint _publishEndpoint;

    public BasketController(IMediator mediator, DiscountGrpcServices discountGrpcServices, 
        IPublishEndpoint publishEndpoint)
    {
        _mediator = mediator;
        _discountGrpcServices = discountGrpcServices;
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    [Route("[action]/{userName}", Name = "GetBasketByUserName")]
    [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCartResponse>> GetBasketByUserName(string userName)
    {
        var basket = await _mediator.Send(new GetBasketByUserNameQuery(userName));
        return Ok(basket);
    }
    
    [HttpPost]
    [Route("CreateBasket")]
    [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCartResponse>> UpdateBasket([FromBody]CreateShoppingCartCommand command)
    {
        foreach (var item in command.Items)
        {
            var coupon = await _discountGrpcServices.GetDiscount(item.ProductName);
            if (coupon?.Amount > 0)
            {
                item.Price -= coupon.Amount;
            }
        }
        var basket = await _mediator.Send(command);
        return Ok(basket);
    }
    
    [HttpDelete]
    [Route("[action]/{userName}", Name = "DeleteBasketByUserName")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCartResponse>> DeleteBasketByUserName(string userName)
    {
        var basket = await _mediator.Send(new DeleteBasketByUserNameQuery(userName));
        return Ok(basket);
    }
    
    [HttpPost]
    [Route("Checkout")]
    [ProducesResponseType((int)HttpStatusCode.Accepted)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ShoppingCartResponse>> Checkout([FromBody]BasketCheckout basketCheckout)
    {
        //Get existing basket with userName
        var basket = await _mediator.Send(new GetBasketByUserNameQuery(basketCheckout.UserName));
        if (basket is null)
        {
            return BadRequest();
        }

        var eventMsg = BasketMapper.Mapper.Map<BasketCheckoutEvent>(basketCheckout);
        eventMsg.TotalPrice = basket.TotalPrice;
        await _publishEndpoint.Publish(eventMsg);
        
        //remove the basket
        await _mediator.Send(new DeleteBasketByUserNameQuery(basketCheckout.UserName));
        return Accepted();
    }
}