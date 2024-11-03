using Microsoft.AspNetCore.Mvc;
using zstore.net.Services.Product;

namespace zstore.net.Controllers.APIs;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
  private readonly ILogger<ProductsController> _logger;
  private readonly IProductService _productService;

  public ProductsController(ILogger<ProductsController> logger, IProductService productService)
  {
    _logger = logger;
    _productService = productService;
  }

  [HttpGet]
  public async Task<IActionResult> GetProducts(
            [FromQuery] int? page = 1,
            [FromQuery] string? search = null,
            [FromQuery] long? category = null,
            [FromQuery] string? sort = null)
  {
    _logger.LogInformation("GetProducts API called");
    
    int currentPage = page ?? 1;

    // Get products and total product count using the service
    var (products, totalProducts, totalPages) = await _productService.GetProductsAsync(
        currentPage, search, category, sort);

    // Return the response as JSON
    var response = new
    {
      TotalProducts = totalProducts,
      TotalPages = totalPages,
      CurrentPage = currentPage,
      Products = products
    };

    return Ok(response);
  }

}