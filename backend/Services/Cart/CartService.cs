using CosmeticEnterpriseBack.Data;
using CosmeticEnterpriseBack.DTO.Cart;
using CosmeticEnterpriseBack.Entities;
using CosmeticEnterpriseBack.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CosmeticEnterpriseBack.Services.Cart;
public class CartService(AppDbContext dbContext, IObjectStorageService objectStorageService): ICartService
{
    public async Task<ShoppingCartResponse> GetCartAsync(long userId, CancellationToken cancellationToken)
    {
        var cart = await dbContext.ShoppingCarts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdUser == userId, cancellationToken);

        if (cart is null)
        {
            return new ShoppingCartResponse
            {
                Id = 0,
                IdUser = userId,
                Items = [],
                TotalItemsQuantity = 0,
                TotalAmount = 0m
            };
        }

        return await BuildCartResponseAsync(cart.Id, cancellationToken);
    }
    public async Task<ShoppingCartResponse> AddItemAsync(long userId, AddCartItemRequest request, 
        CancellationToken cancellationToken)
    {
        var cart = await GetOrCreateCartAsync(userId, cancellationToken);

        var product = await dbContext.FinishedProducts
            .Include(x => x.Images)
            .FirstOrDefaultAsync(x => x.Id == request.IdFinishedProduct, cancellationToken);

        if (product is null)
            throw new KeyNotFoundException("Finished product not found.");

        var existingItem = await dbContext.ShoppingCartItems.FirstOrDefaultAsync(x => 
                x.IdShoppingCart == cart.Id && x.IdFinishedProduct == request.IdFinishedProduct,cancellationToken);

        if (existingItem is null)
        {
            existingItem = new ShoppingCartItem
            {
                IdShoppingCart = cart.Id,
                IdFinishedProduct = product.Id,
                Quantity = request.Quantity,
                UnitPrice = product.Price,
                CreatedAtUtc = DateTime.UtcNow
            };

            dbContext.ShoppingCartItems.Add(existingItem);
        }
        else
        {
            var newQuantity = existingItem.Quantity + request.Quantity;
            if (newQuantity > 999)
                throw new ArgumentException("Quantity cannot be greater than 999.");
            existingItem.Quantity = newQuantity;
        }
        cart.UpdatedAtUtc = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
        return await BuildCartResponseAsync(cart.Id, cancellationToken);
    }
    public async Task<ShoppingCartResponse> UpdateItemQuantityAsync(long userId, long itemId, 
        UpdateCartItemQuantityRequest request, CancellationToken cancellationToken)
    {
        var cart = await GetOrCreateCartAsync(userId, cancellationToken);

        var item = await dbContext.ShoppingCartItems .FirstOrDefaultAsync(x => 
                x.Id == itemId && x.IdShoppingCart == cart.Id, cancellationToken);

        if (item is null)
            throw new KeyNotFoundException("Cart item not found.");

        item.Quantity = request.Quantity;
        cart.UpdatedAtUtc = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
        return await BuildCartResponseAsync(cart.Id, cancellationToken);
    }
    public async Task<ShoppingCartResponse> RemoveItemAsync(long userId, long itemId, 
        CancellationToken cancellationToken)
    {
        var cart = await GetOrCreateCartAsync(userId, cancellationToken);

        var item = await dbContext.ShoppingCartItems
            .FirstOrDefaultAsync(
                x => x.Id == itemId && x.IdShoppingCart == cart.Id,
                cancellationToken);

        if (item is null)
            throw new KeyNotFoundException("Cart item not found.");

        dbContext.ShoppingCartItems.Remove(item);
        cart.UpdatedAtUtc = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return await BuildCartResponseAsync(cart.Id, cancellationToken);
    }
    public async Task ClearCartAsync(long userId, CancellationToken cancellationToken)
    {
        var cart = await dbContext.ShoppingCarts.FirstOrDefaultAsync(x => x.IdUser == userId, 
            cancellationToken);

        if (cart is null)
            return;

        var items = await dbContext.ShoppingCartItems
            .Where(x => x.IdShoppingCart == cart.Id)
            .ToListAsync(cancellationToken);

        if (items.Count == 0)
            return;

        dbContext.ShoppingCartItems.RemoveRange(items);
        cart.UpdatedAtUtc = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
    private async Task<ShoppingCart> GetOrCreateCartAsync(long userId, CancellationToken cancellationToken)
    {
        var cart = await dbContext.ShoppingCarts
            .FirstOrDefaultAsync(x => x.IdUser == userId, cancellationToken);

        if (cart is not null)
            return cart;

        cart = new ShoppingCart
        {
            IdUser = userId,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };

        dbContext.ShoppingCarts.Add(cart);
        await dbContext.SaveChangesAsync(cancellationToken);

        return cart;
    }
    private async Task<ShoppingCartResponse> BuildCartResponseAsync(long cartId, CancellationToken cancellationToken)
    {
        var cart = await dbContext.ShoppingCarts
            .AsNoTracking()
            .Include(x => x.Items)
                .ThenInclude(x => x.FinishedProduct)
                    .ThenInclude(x => x.Images)
            .FirstAsync(x => x.Id == cartId, cancellationToken);

        var items = cart.Items
            .OrderBy(x => x.Id)
            .Select(x =>
            {
                var mainImage = x.FinishedProduct.Images
                    .OrderByDescending(i => i.IsMain)
                    .ThenBy(i => i.SortOrder)
                    .FirstOrDefault();

                return new ShoppingCartItemResponse
                {
                    Id = x.Id,
                    IdFinishedProduct = x.IdFinishedProduct,
                    ProductName = x.FinishedProduct.Name,
                    MainImageUrl = mainImage is null
                        ? null
                        : objectStorageService.GetFileUrl(mainImage.ObjectKey),
                    UnitPrice = x.UnitPrice,
                    Quantity = x.Quantity,
                    LineTotal = x.UnitPrice * x.Quantity
                };
            })
            .ToList();

        return new ShoppingCartResponse
        {
            Id = cart.Id,
            IdUser = cart.IdUser,
            Items = items,
            TotalItemsQuantity = items.Sum(x => x.Quantity),
            TotalAmount = items.Sum(x => x.LineTotal)
        };
    }
}