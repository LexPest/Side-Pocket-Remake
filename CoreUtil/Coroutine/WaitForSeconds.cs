namespace CoreUtil.Coroutine
{
  /// <summary>
  /// Инструкция Yield: подождать определенное количество секунд
  /// </summary>
  public class WaitForSeconds : YieldInstruction
  {
    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parTimeInSeconds">Время в секундах</param>
    public WaitForSeconds(double parTimeInSeconds)
    {
      TimeInSeconds = parTimeInSeconds;
    }

    /// <summary>
    /// Время в секундах
    /// </summary>
    public double TimeInSeconds { get; private set; }
  }
}