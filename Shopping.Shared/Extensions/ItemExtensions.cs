using Shopping.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Shared;

public static class ItemExtensions
{
    public static IQueryable<ItemDto> MapToItemDto(this IQueryable<Item> item)
    {
        return item.Select(i => new ItemDto(
            i.Id,
            i.ItemName,
            i.Description,
            i.UnitOfMeasure.UOM,
            i.Quantity,
            i.Price
        ));
    }
}
