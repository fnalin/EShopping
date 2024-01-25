namespace Basket.Application.Responses;

public class ShoppingCartResponse
{
    public ShoppingCartResponse(string userName)
    {
        UserName = userName;
    }

    public ShoppingCartResponse(){}

    public string UserName { get; set; }
    public IList<ShoppingCartItemResponse> Items { get; set; }
    public decimal TotalPrice => 
        Items.Sum(x => x.Price) * Items.Sum(x => x.Quantity);
}