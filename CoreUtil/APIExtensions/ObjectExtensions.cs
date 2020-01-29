using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Security;

namespace CoreUtil.APIExtensions
{
  /// <summary>
  /// Расширения для работы с непосредственно объектами, активно использует механизм рефлексии
  /// </summary>
  public static class ObjectExtensions
  {
    /// <summary>
    /// Получить все метаданные о MulticastDelegates объекта
    /// </summary>
    /// <param name="parT">Тип объекта</param>
    /// <returns></returns>
    public static IEnumerable<FieldInfo> GetAllSubscribedMethodsInfo(this Type parT)
    {
      Func<EventInfo, FieldInfo> ei2Fi =
        parEi => parT.GetField(parEi.Name,
          BindingFlags.NonPublic |
          BindingFlags.Instance |
          BindingFlags.GetField);

      return from eventInfo in parT.GetEvents(BindingFlags.Public | BindingFlags.NonPublic)
        select ei2Fi(eventInfo);
    }

    /// <summary>
    /// Удалить все сведения о подписках на MulticastDelegates объекта
    /// </summary>
    /// <param name="parTarget">Целевой объект</param>
    /// <param name="parSubscribedMethodsInfo">Кэшированная информация о MulticastDelegates</param>
    /// <typeparam name="T"></typeparam>
    public static void RemoveSubscribedMethods<T>(T parTarget, IEnumerable<FieldInfo> parSubscribedMethodsInfo)
    {
      foreach (var fieldInfo in parSubscribedMethodsInfo)
      {
        fieldInfo.SetValue(parTarget, null);
      }
    }

    /// <summary>
    /// Получить все поля класса
    /// </summary>
    /// <param name="parType">Целевой тип</param>
    /// <returns></returns>
    public static IEnumerable<FieldInfo> GetAllFields(this Type parType)
    {
      if (parType == null)
      {
        return Enumerable.Empty<FieldInfo>();
      }

      BindingFlags flags = BindingFlags.Public |
                           BindingFlags.NonPublic |
                           BindingFlags.Instance |
                           BindingFlags.DeclaredOnly;

      return parType.GetFields(flags).Where(parF =>
          parF.GetCustomAttribute<InspectionIgnore>() == null)
        .Union(GetAllFields(parType.BaseType));
    }

    /// <summary>
    /// Сбросить все поля класса
    /// </summary>
    /// <param name="parTarget">Целеовой объект</param>
    /// <param name="parFieldsInfo">Кэшированная информация о полях класса</param>
    /// <typeparam name="T"></typeparam>
    public static void RemoveAllFields<T>(T parTarget, IEnumerable<FieldInfo> parFieldsInfo)
    {
      foreach (var fieldInfo in parFieldsInfo)
      {
        fieldInfo.SetValue(parTarget, GetDefault(fieldInfo.FieldType));
      }
    }

    /// <summary>
    /// Получить все свойства класса
    /// </summary>
    /// <param name="parType">Целевой тип</param>
    /// <returns></returns>
    public static IEnumerable<PropertyInfo> GetAllProperties(this Type parType)
    {
      if (parType == null)
      {
        return Enumerable.Empty<PropertyInfo>();
      }

      BindingFlags flags = BindingFlags.Public |
                           BindingFlags.NonPublic |
                           BindingFlags.Instance |
                           BindingFlags.DeclaredOnly;

      return parType.GetProperties(flags).Where(parF => parF.GetCustomAttribute<InspectionIgnore>() == null)
        .Union(GetAllProperties(parType.BaseType));
    }

    /// <summary>
    /// Получить значение по-умолчанию для типа
    /// </summary>
    /// <param name="parType">Целевой тип</param>
    /// <returns></returns>
    public static object GetDefault(Type parType)
    {
      if (parType.IsValueType)
      {
        return Activator.CreateInstance(parType);
      }

      return null;
    }
  }
}