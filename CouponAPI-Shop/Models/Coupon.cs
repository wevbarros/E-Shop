using System.ComponentModel.DataAnnotations;

public class Coupon 
{
    [Key]
    public string? CouponCode { get; set; }
    public string? Rarity { get; set; }

    public double Discount { get; set; }

}