using BCrypt.Net;

namespace zstore.net.Utils;

public static class Utils
{

  public static String GetHash(String password)
  {
    return BCrypt.Net.BCrypt.HashPassword(password);
  }
  
  public static Boolean VerifyHash(String password, String hash)
  {
    return BCrypt.Net.BCrypt.Verify(password, hash);
  }

  public static String GetRandomString(int length)
  {
    const String chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    return new String(Enumerable.Repeat(chars, length)
      .Select(s => s[new Random().Next(s.Length)]).ToArray());
  }
}