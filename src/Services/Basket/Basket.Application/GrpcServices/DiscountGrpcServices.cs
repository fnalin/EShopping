using Discount.Grpc.Protos;

namespace Basket.Application.GrpcServices;

public class DiscountGrpcServices
{
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;

    public DiscountGrpcServices(DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    {
        _discountProtoServiceClient = discountProtoServiceClient;
    }

    public async Task<CouponModel> GetDiscount(string productName)
    {
        var discountRequest = new GetDiscountRequest { ProductName = productName };
        return await _discountProtoServiceClient.GetDiscountAsync(discountRequest);
    }
}