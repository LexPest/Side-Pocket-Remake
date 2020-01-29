namespace Model.SPCore.GameplayModelDefinition.ObjectModel
{
  /// <summary>
  /// Интерфейс обновляемого объекта
  /// </summary>
  public interface IUpdatable
  {
    /// <summary>
    /// Является ли активным
    /// </summary>
    /// <returns></returns>
    bool IsActive();
    
    /// <summary>
    /// Сигнал обновления модели
    /// </summary>
    /// <param name="parDeltaTime">Время кадра в секундах</param>
    void Update(double parDeltaTime);
  }
}