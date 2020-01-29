using System.Collections;

namespace CoreUtil.Coroutine
{
  /// <summary>
  /// Информация о запуске по истечению времени
  /// </summary>
  internal class RunAfterTimeInfo
  {
    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parTime">Время</param>
    /// <param name="parCoroutine">Короутина</param>
    public RunAfterTimeInfo(double parTime, IEnumerator parCoroutine)
    {
      Time = parTime;
      Coroutine = parCoroutine;
    }

    /// <summary>
    /// Время
    /// </summary>
    public double Time { get; private set; }

    /// <summary>
    /// Короутина
    /// </summary>
    public IEnumerator Coroutine { get; private set; }
  }
}