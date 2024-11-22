
namespace Shopping.DAL;

public partial class UnitOfMeasure : BaseEntity
{
    public string UOM { get; set; }
    public string Description { get; set; }
    public virtual ICollection<Item> Items { get; set; }
}
