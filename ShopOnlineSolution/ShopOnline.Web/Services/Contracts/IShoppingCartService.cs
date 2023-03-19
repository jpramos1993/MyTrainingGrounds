using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Services.Contracts
{
    public interface IShoppingCartService
    {
        Task<IEnumerable<CartItemDto>> GetItems(int usedId);

        Task<CartItemDto> AddItem(CartItemToAddDto cartItemToDto);
    }
}
