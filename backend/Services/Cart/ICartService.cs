using CosmeticEnterpriseBack.DTO.Cart;

namespace CosmeticEnterpriseBack.Services.Cart;

public interface ICartService
{
    Task<ShoppingCartResponse> GetCartAsync(long userId, CancellationToken cancellationToken);
    Task<ShoppingCartResponse> AddItemAsync(long userId, AddCartItemRequest request, CancellationToken cancellationToken);
    Task<ShoppingCartResponse> UpdateItemQuantityAsync(long userId, long itemId, UpdateCartItemQuantityRequest request, CancellationToken cancellationToken);
    Task<ShoppingCartResponse> RemoveItemAsync(long userId, long itemId, CancellationToken cancellationToken);
    Task ClearCartAsync(long userId, CancellationToken cancellationToken);
}