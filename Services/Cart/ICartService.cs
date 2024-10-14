namespace zstore.net.Services.Cart;

public interface ICartService
{
  void AddToCart(long productId, int quantity = 1);
  Dictionary<long, int> GetCartItems();
  void RemoveFromCart(long productId);
  void ClearCart();
}