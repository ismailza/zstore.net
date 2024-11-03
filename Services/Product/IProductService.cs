namespace zstore.net.Services.Product;

public interface IProductService
{

  Task<(List<Models.Product> Products, int TotalProducts, int TotalPages)> GetProductsAsync(
            int page, string? search, long? category, string? sort);
}