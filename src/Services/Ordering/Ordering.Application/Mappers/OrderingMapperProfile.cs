using AutoMapper;
using EventBus.Messages.Events;
using Ordering.Application.Commands;
using Ordering.Application.Responses;
using Ordering.Core.Entities;

namespace Ordering.Application.Mappers;

public class OrderingMapperProfile : Profile
{
    public OrderingMapperProfile()
    {
        CreateMap<Order, OrderResponse>().ReverseMap();
        CreateMap<Order, CheckoutOrderCommand>().ReverseMap();
        CreateMap<Order, UpdateOrderCommand>().ReverseMap();
        CreateMap<BasketCheckoutEvent, CheckoutOrderCommand>().ReverseMap();
    }
}