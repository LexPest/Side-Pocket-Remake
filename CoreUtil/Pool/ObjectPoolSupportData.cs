using System;
using System.Collections.Generic;
using System.Reflection;
using CoreUtil.APIExtensions;

namespace CoreUtil.Pool
{
  /// <summary>
  /// Содержит информацию о работающим с механизмом пулинга объектом
  /// </summary>
  public class ObjectPoolSupportData
  {
    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parLinkedPoolManager">Связанный пул-менеджер</param>
    /// <param name="parTargetType">Целевой тип объекта</param>
    public ObjectPoolSupportData(PoolManager parLinkedPoolManager, Type parTargetType)
    {
      LinkedPoolManager = parLinkedPoolManager;

      //получение метаданных об объекте
      EventsMetaInfo = parTargetType.GetAllSubscribedMethodsInfo();
      FieldsMetaInfo = parTargetType.GetAllFields();
      PropsMetaInfo = parTargetType.GetAllProperties();
    }

    /// <summary>
    /// Связанный пул-менеджер
    /// </summary>
    public PoolManager LinkedPoolManager { get; private set; }
    
    /// <summary>
    /// Метаданные о событиях класса
    /// </summary>
    public IEnumerable<FieldInfo> EventsMetaInfo { get; private set; }
    
    /// <summary>
    /// Метаданные о полях класса
    /// </summary>
    public IEnumerable<FieldInfo> FieldsMetaInfo { get; private set; }
    
    /// <summary>
    /// Метаданные о свойствах класса
    /// </summary>
    public IEnumerable<PropertyInfo> PropsMetaInfo { get; private set; }
  }
}