using System;

namespace CoreUtil.APIExtensions
{
  /// <summary>
  /// Вспомогательный атрибут-маркер: данное поле или свойство обрабатывать не нужно
  /// </summary>
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
  public class InspectionIgnore : Attribute
  {
  }
}