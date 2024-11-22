using Microsoft.EntityFrameworkCore;
using Shopping.DAL;
using Shopping.Interfaces;
using Shopping.Shared;

namespace Shopping.BL;

public class ItemRepository : GenericRepository<Item>, IItemRepository
{
    public ItemRepository(AppDbContext context) : base(context)
    {
    }
    public async Task<IReadOnlyList<ItemDto>> GetAllItemsAsync(int pageNumber, int pageSize)
    {
        var items = await GetQueryable(pageNumber, pageSize).MapToItemDto().ToListAsync();
        return items;
    }
    public async Task<IReadOnlyList<ItemDto>> GetAllAvailableItemsAsync(int pageNumber, int pageSize)
    {
        var items = await GetQueryable(pageNumber, pageSize).MapToItemDto().Where(i=>i.Quantity>0).ToListAsync();
        return items;
    }
    public Task<ItemDto> GetItemAsync(int id)
    {
        var item = GetQueryable(id).MapToItemDto().FirstOrDefaultAsync();
        return item;
    }
    public async Task<ResultDto> AddItemAsync(AddItemDto item)
    {
        if (!await UnitOfMeasureExists(item.UomId))
        {
            return false.ToOperationResult();
        }
        var newItem = new Item()
        {
            ItemName = item.ItemName,
            Description = item.Description,
            UomId = item.UomId,
            Quantity = item.Quantity ?? 0,
            Price = item.Price ?? 0,
        };
        var result = await AddAsync(newItem);
        return result.ToOperationResult();
    }
    public async Task<ResultDto> UpdateItemAsync(UpdateItemDto item)
    {
        if (!await UnitOfMeasureExists(item.UomId))
        {
            return false.ToOperationResult();
        }
        var result = await _dbSet.Where(i => i.Id == item.Id)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(i => i.ItemName, item.ItemName)
            .SetProperty(i=>i.Description,item.Description)
            .SetProperty(i=>i.UomId,item.UomId)
            .SetProperty(i=>i.Quantity,item.Quantity ?? 0)
            .SetProperty(i=>i.Price,item.Price ?? 0));

        return result.ToOperationResult();
    }
    public async Task<ResultDto> DeleteItemAsync(int id)
    {
        var result = await DeleteAsync(id);
        return result.ToOperationResult();
    }
    #region Extensions
    private async Task<bool> UnitOfMeasureExists(int id)
    {
        return await _context.UnitOfMeasures.AnyAsync(u => u.Id == id);
    }
    #endregion
}
