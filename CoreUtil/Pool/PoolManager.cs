using System;
using System.Collections.Generic;
using CoreUtil.APIExtensions;

namespace CoreUtil.Pool
{
  /// <summary>
  /// Менеджер пула объектов
  /// </summary>
  public class PoolManager
  {
    /// <summary>
    /// Стандартный конструктор без параметров
    /// </summary>
    public PoolManager()
    {
    }

    /// <summary>
    /// Содержимое пула
    /// </summary>
    public Dictionary<Type, Stack<PoolSupportedObject>> PoolContents { get; private set; } =
      new Dictionary<Type, Stack<PoolSupportedObject>>();

    /// <summary>
    /// Кэшированные метаданных о типах
    /// </summary>
    public Dictionary<Type, ObjectPoolSupportData> TypesSupportData { get; private set; } =
      new Dictionary<Type, ObjectPoolSupportData>();

    /// <summary>
    /// Получить из пула объект определенного типа
    /// </summary>
    /// <param name="parT">Желаемый преобразованный тип</param>
    /// <typeparam name="T">Реальный тип объекта</typeparam>
    /// <returns></returns>
    public T GetObject<T>(Type parT) where T : PoolSupportedObject
    {
      return (T) GetObject(parT);
    }

    /// <summary>
    /// Получить из пула объект определенного типа
    /// </summary>
    /// <param name="parT">Реальный тип объекта</param>
    /// <returns></returns>
    public object GetObject(Type parT)
    {
      if (PoolContents.ContainsKey(parT))
      {
        Stack<PoolSupportedObject> stackContents = PoolContents[parT];
        if (stackContents.Count > 0)
        {
          ObjectPoolSupportData supportData = TypesSupportData[parT];
          PoolSupportedObject retObject = stackContents.Pop();
          retObject.ActualLinkedObjectPoolSupportData = supportData;
          return retObject;
        }
      }

      object newPoolObjectInstance = ProduceNewObject(parT);
      return newPoolObjectInstance;
    }


    /// <summary>
    /// Создать новый экземпляр класса запрошенного типа
    /// </summary>
    /// <param name="parT">Запрошенный тип</param>
    /// <returns></returns>
    private object ProduceNewObject(Type parT)
    {
      ObjectPoolSupportData supportData = null;
      if (TypesSupportData.ContainsKey(parT))
      {
        supportData = TypesSupportData[parT];
      }
      else
      {
        supportData = new ObjectPoolSupportData(this, parT);
        TypesSupportData.Add(parT, supportData);
      }

      return Activator.CreateInstance(parT, supportData);
    }


    /// <summary>
    /// Сохранить объект в пуле для последующего использования
    /// </summary>
    /// <param name="parObjectToStore">Объект, который будет утилизирован</param>
    /// <typeparam name="T">Тип утилизируемого объекта</typeparam>
    public void StoreObject<T>(T parObjectToStore) where T : PoolSupportedObject
    {
      Type t = parObjectToStore.GetType();

      //нам нужно полностью "сбросить" объект
      ObjectPoolSupportData supportData = TypesSupportData[t];

      ObjectExtensions.RemoveSubscribedMethods(parObjectToStore, supportData.EventsMetaInfo);
      ObjectExtensions.RemoveAllFields(parObjectToStore, supportData.FieldsMetaInfo);

      if (PoolContents.ContainsKey(t))
      {
        PoolContents[t].Push(parObjectToStore);
      }
      else
      {
        Stack<PoolSupportedObject> stackContents = new Stack<PoolSupportedObject>();
        stackContents.Push(parObjectToStore);
        PoolContents.Add(t, stackContents);
      }
    }
  }
}