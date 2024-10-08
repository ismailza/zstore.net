using Newtonsoft.Json;

namespace zstore.net.Services.Cart;

public class SessionCartService : ICartService
{
  private readonly IHttpContextAccessor _httpContextAccessor;
  private const string cartSessionKey = "cart";

  public SessionCartService(IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;
  }

  public void AddToCart(long productId, int quantity = 1)
  {
    if (_httpContextAccessor.HttpContext == null)
    {
      return;
    }
    var session = _httpContextAccessor.HttpContext.Session;
    var cart = session.GetString(cartSessionKey) ?? "{}";

    var cartItems = JsonConvert.DeserializeObject<Dictionary<long, int>>(cart) ?? new Dictionary<long, int>();

    if (cartItems.ContainsKey(productId))
    {
      cartItems[productId] += quantity;
    }
    else
    {
      cartItems[productId] = quantity;
    }

    session.SetString(cartSessionKey, JsonConvert.SerializeObject(cartItems));
  }

  public Dictionary<long, int> GetCartItems()
  {
    if (_httpContextAccessor.HttpContext == null)
    {
      return new Dictionary<long, int>();
    }
    var session = _httpContextAccessor.HttpContext.Session;
    var cart = session.GetString(cartSessionKey) ?? "{}";

    return JsonConvert.DeserializeObject<Dictionary<long, int>>(cart) ?? new Dictionary<long, int>();
  }

  public void RemoveFromCart(long productId)
  {
    if (_httpContextAccessor.HttpContext == null)
    {
      return;
    }
    var session = _httpContextAccessor.HttpContext.Session;
    var cart = session.GetString(cartSessionKey) ?? "{}";

    var cartItems = JsonConvert.DeserializeObject<Dictionary<long, int>>(cart) ?? new Dictionary<long, int>();

    if (cartItems.ContainsKey(productId))
    {
      cartItems.Remove(productId);
    }

    session.SetString(cartSessionKey, JsonConvert.SerializeObject(cartItems));
  }

  public void ClearCart()
  {
    if (_httpContextAccessor.HttpContext == null)
    {
      return;
    }
    var session = _httpContextAccessor.HttpContext.Session;
    session.Remove(cartSessionKey);
  }

}