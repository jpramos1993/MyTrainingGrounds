﻿using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Services.Contracts
{
    public interface IShoppingCartService
    {
        Task<List<CartItemDto>> GetItems(int usedId);

        Task<CartItemDto> AddItem(CartItemToAddDto cartItemToDto);

        Task<CartItemDto> DeleteItem(int id);
    }
}
