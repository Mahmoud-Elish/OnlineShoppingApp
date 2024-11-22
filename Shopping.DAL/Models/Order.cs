namespace Shopping.DAL;

public partial class Order : BaseEntity
{
    public DateTime RequestDate { get; set; }
    public DateTime? CloseDate { get; set; }
    public OrderStatus Status { get; set; }
    public string CustomerId { get; set; }
    public string? DiscountPromoCode { get; set; }
    public decimal? DiscountValue { get; set; }
    public decimal TotalPrice { get; set; }
    public string CurrencyCode { get; set; }
    public decimal ExchangeRate { get; set; }
    public decimal ForeignPrice { get; set; }
    public virtual User Customer { get; set; }
    public virtual ICollection<OrderDetail> OrderDetails { get; set; }
}
