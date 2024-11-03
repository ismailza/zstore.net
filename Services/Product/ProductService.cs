using Microsoft.EntityFrameworkCore;
using zstore.net.Data;

namespace zstore.net.Services.Product;

public class ProductService : IProductService
{
  private readonly ZStoreDbContext _context;

  public ProductService(ZStoreDbContext context)
  {
    _context = context;
  }

  private const int PageSize = 10;

  public async Task<(List<Models.Product> Products, int TotalProducts, int TotalPages)> GetProductsAsync(
            int page, string? search, long? category, string? sort)
  {
    // Start with a queryable collection of products
    var query = _context.Products.AsQueryable();

    // Apply search filter if provided
    if (!string.IsNullOrEmpty(search))
    {
      query = query.Where(p => p.Name.ToLower().Contains(search.ToLower()));
    }

    // Apply category filter if provided
    if (category.HasValue)
    {
      query = query.Where(p => p.CategoryId == category.Value);
    }

    // Apply sorting based on the provided sort order
    query = sort switch
    {
      "name_asc" => query.OrderBy(p => p.Name),
      "name_desc" => query.OrderByDescending(p => p.Name),
      "price_asc" => query.OrderBy(p => p.Price),
      "price_desc" => query.OrderByDescending(p => p.Price),
      _ => query.OrderByDescending(p => p.UpdatedAt), // Default sort
    };

    // Get the total number of products for pagination
    var totalProducts = await query.CountAsync();

    // Get the products for the current page
    var products = await query
        .Skip((page - 1) * PageSize)
        .Take(PageSize)
        .Include(p => p.Category)
        .ToListAsync();

    // Calculate total pages for pagination
    var totalPages = (int)System.Math.Ceiling(totalProducts / (double)ProductService.PageSize);

    // Return the products and total count
    return (products, totalProducts, totalPages);
  }

}