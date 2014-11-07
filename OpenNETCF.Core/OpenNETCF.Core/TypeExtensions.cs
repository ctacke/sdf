namespace OpenNETCF
{
  using System;

  public static partial class Extensions
  {
    /// <summary>
    /// Determines if a type implements a particular interface
    /// </summary>
    /// <typeparam name="TInterface">The interface type to check for.</typeparam>
    /// <returns>True: if the type implements the interface, otherwise False.</returns>
    public static bool Implements<TInterface>(this Type baseType)
    {
      if (!(typeof(TInterface).IsInterface))
      {
        throw new ArgumentException("TInterface must be an interface type.");
      }

      Type interfaceType = typeof(TInterface);
      foreach (Type t in baseType.GetInterfaces())
      {
        if (t.Equals(interfaceType))
          return true;
      }

      return false;
    }

  }
}
