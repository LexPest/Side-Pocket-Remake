namespace Model.SPCore.GameplayModelDefinition.ObjectModel
{
  /// <summary>
  /// Интерфейс фиксированно обновляемого объекта
  /// </summary>
  public interface IFixedUpdatable
  {
    /// <summary>
    /// Является ли активным
    /// </summary>
    /// <returns></returns>
    bool IsActive();
    
    /// <summary>
    /// Сигнал фиксированного обновления модели (используется преимущественно для обработки физики и ввода)
    /// </summary>
    /// <param name="parFixedDeltaTime">Время шага фиксированного обновления в секундах</param>
    void FixedUpdate(double parFixedDeltaTime);
  }
}