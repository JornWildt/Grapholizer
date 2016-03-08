using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Grapholizer.Core.Utility
{
  public static class ReflectionHelper
  {
    public static Dictionary<string, object> ConvertObjectPropertiesToDictionary(object src)
    {
      Dictionary<string, object> result = new Dictionary<string, object>();

      if (src == null)
        return result;

      Type t = src.GetType();
      foreach (PropertyInfo fi in t.GetProperties())
      {
        string key = fi.Name;
        object value = ReadValue(fi, src);
        result[key] = value;
      }

      return result;
    }


    private static object ReadValue(PropertyInfo property, object data)
    {
      if (property.CanRead)
        return property.GetValue(data, null);
      throw new ArgumentException(string.Format("The value {0} cannot be read", property.Name));
    }
  }
}
