using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace zstore.net.Services.Cart;

public class CookieCartService : ICartService
{
  private readonly IHttpContextAccessor _httpContextAccessor;
  private const string cartCookieName = "cart";

  public CookieCartService(IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;
  }

  public void AddToCart(long productId, int quantity)
  {
    var cart = GetCartItems();
    if (cart.ContainsKey(productId))
    {
      cart[productId] += quantity;
    }
    else
    {
      cart[productId] = quantity;
    }

    SetCartItems(cart);
  }

  public void RemoveFromCart(long productId)
  {
    var cart = GetCartItems();
    if (cart.ContainsKey(productId))
    {
      cart.Remove(productId);
      SetCartItems(cart);
    }
  }

  public Dictionary<long, int> GetCartItems()
  {
    var context = _httpContextAccessor.HttpContext;
    if (context == null)
    {
      return new Dictionary<long, int>();
    }

    if (context.Request.Cookies.TryGetValue(cartCookieName, out var cookieValue))
    {
      return JsonConvert.DeserializeObject<Dictionary<long, int>>(cookieValue) ?? new Dictionary<long, int>();
    }

    return new Dictionary<long, int>();
  }

  public void ClearCart()
  {
    var context = _httpContextAccessor.HttpContext;
    if (context == null)
    {
      return;
    }
    context.Response.Cookies.Delete(cartCookieName);
  }

  private void SetCartItems(Dictionary<long, int> cart)
  {
    var context = _httpContextAccessor.HttpContext;
    if (context == null)
    {
      return;
    }
    var cookieValue = JsonConvert.SerializeObject(cart);
    context.Response.Cookies.Append(cartCookieName, cookieValue, new CookieOptions
    {
      HttpOnly = true,
      MaxAge = TimeSpan.FromDays(30)
    });
  }
}
