namespace Basket.Core.Entities;

public class ShoppingCart
{
    public string UserName { get; set; }
    public IList<ShoppingCartItem> Items { get; set; }

    public ShoppingCart(string userName, IList<ShoppingCartItem> items)
    {
        UserName = userName;
        Items = items;
    }
}