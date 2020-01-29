using CoreUtil.APIExtensions;

namespace CoreUtil.Pool
{
  /// <summary>
  /// Базовый тип для всех объектов, поддерживающих механизм пулинга
  /// </summary>
  public abstract class PoolSupportedObject
  {
    /// <summary>
    /// Обязательный конструктор
    /// </summary>
    /// <param name="parSupportData">Информация о работе с пулом</param>
    protected PoolSupportedObject(ObjectPoolSupportData parSupportData)
    {
      ActualLinkedObjectPoolSupportData = parSupportData;
    }

    /// <summary>
    /// Объект, содержащий информацию о работе с механизмом пулинга для данного объекта
    /// </summary>
    [InspectionIgnore]
    public ObjectPoolSupportData ActualLinkedObjectPoolSupportData { get; set; }

    /// <summary>
    /// Сброс объекта и отправка обратно в пул
    /// </summary>
    public void DisableAndSendToPool()
    {
      OnBeforeResetPerform();
      ActualLinkedObjectPoolSupportData?.LinkedPoolManager?.StoreObject(this);
    }

    /// <summary>
    /// В переопределении должны быть размещены действия перед отправкой в пул
    /// </summary>
    public virtual void OnBeforeResetPerform()
    {
    }
  }
}