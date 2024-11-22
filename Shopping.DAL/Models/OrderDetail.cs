
namespace Shopping.DAL;

public partial class OrderDetail : BaseEntity
{
    public int OrderId { get; set; }
    public int ItemId { get; set; }
    public decimal ItemPrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public virtual Order Order { get; set; }
    public virtual Item Item { get; set; }
}
