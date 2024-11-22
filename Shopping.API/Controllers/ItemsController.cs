using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shopping.DAL;
using Shopping.Interfaces;
using Shopping.Shared;

namespace Shopping.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : AppControllerBase
    {
        private readonly IItemRepository _itemRepo;
        public ItemsController(IItemRepository itemRepo) 
        {
            _itemRepo = itemRepo;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetItems(int pageIndex = 0, int pageSize = 30)
        {
            try
            {
                var items = await _itemRepo.GetAllItemsAsync(pageIndex, pageSize);
                return Ok(items);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(int id)
        {
            try
            {
                var item = await _itemRepo.GetItemAsync(id);
                return item == null ? NotFound(item) : Ok(item);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateItem(AddItemDto item)
        {
            try
            {
                var result = await _itemRepo.AddItemAsync(item);
                return FromOperationResult(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPut("update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateItem(UpdateItemDto item) 
        {
            try
            {
                var result = await _itemRepo.UpdateItemAsync(item);
                return FromOperationResult(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            try
            {
                var result = await _itemRepo.DeleteItemAsync(id);
                return FromOperationResult(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
