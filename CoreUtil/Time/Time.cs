namespace CoreUtil.Time
{
  /// <summary>
  /// Операции со временем
  /// </summary>
  public static class Time
  {
    /// <summary>
    /// Преобразовать миллисекунды в секунды
    /// </summary>
    /// <param name="parMilliseconds">Время в миллисекундах</param>
    /// <returns>Время в секундах</returns>
    public static double MillisecondsToSeconds(double parMilliseconds)
    {
      return parMilliseconds / 1000;
    }
  }
}