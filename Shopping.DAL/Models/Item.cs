namespace Shopping.DAL;

public partial class Item : BaseEntity
{
    public string ItemName { get; set; }
    public string? Description { get; set; }
    public int UomId { get; set; }
    public int Quantity { get; set; } = 0;
    public decimal Price { get; set; } = decimal.Zero;
    public virtual UnitOfMeasure UnitOfMeasure { get; set; }
    public virtual ICollection<OrderDetail> OrderDetails { get; set; }
}
