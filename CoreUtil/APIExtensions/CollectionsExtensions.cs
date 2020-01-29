using System;
using System.Collections.Generic;

namespace CoreUtil.APIExtensions
{
  /// <summary>
  /// Расширения работы с коллекциями
  /// </summary>
  public static class CollectionsExtensions
  {
    /// <summary>
    /// Добавить коллекцию к коллекции
    /// </summary>
    /// <param name="parSource">Исходная коллекция</param>
    /// <param name="parCollection">Добавляемая коллекция</param>
    /// <typeparam name="T">Тип исходной коллекции</typeparam>
    /// <typeparam name="TS">Тип добавляемой коллекции</typeparam>
    /// <exception cref="ArgumentNullException">Нельзя добавить Null</exception>
    /// <exception cref="ArgumentException">Одинаковые ключи не поддерживаются</exception>
    public static void AddRange<T, TS>(this Dictionary<T, TS> parSource, Dictionary<T, TS> parCollection)
    {
      if (parCollection == null)
      {
        throw new ArgumentNullException("Collection is null");
      }

      foreach (var item in parCollection)
      {
        if (!parSource.ContainsKey(item.Key))
        {
          parSource.Add(item.Key, item.Value);
        }
        else
        {
          throw new ArgumentException("Dublicate key not supported");
        }
      }
    }
  }
}