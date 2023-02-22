
namespace ShopOnline.Web.Services.Contracts
{
    using ShopOnline.Models.Dtos;
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetItems();

        Task<ProductDto> GetItem(int id);
    }
}
