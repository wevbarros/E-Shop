using System.ComponentModel.DataAnnotations;

public class ShoppingCart
{
    [Key]
    public string UserName { get; set; } 

    public  bool CouponApplied { get; set; } = false;
    public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();


    public ShoppingCart()
    {
    }
    public ShoppingCart(string userName)
    {
        UserName = userName;
    }
    public decimal TotalPrice
    {
        get
        {
            decimal TotalPrice = 0;
            foreach (var item in Items)
            {
                TotalPrice += item.Price * item.Quantity;
            }
            return TotalPrice;
        }
    }
}

