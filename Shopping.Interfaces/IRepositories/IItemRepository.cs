using Shopping.DAL;
using Shopping.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Interfaces;

public interface IItemRepository : IGenericRepository<Item>
{
    Task<IReadOnlyList<ItemDto>> GetAllItemsAsync(int pageNumber, int pageSize);
    Task<IReadOnlyList<ItemDto>> GetAllAvailableItemsAsync(int pageNumber, int pageSize);
    Task<ItemDto> GetItemAsync(int id);
    Task<ResultDto> AddItemAsync(AddItemDto item);
    Task<ResultDto> UpdateItemAsync(UpdateItemDto item);
    Task<ResultDto> DeleteItemAsync(int id);
}
