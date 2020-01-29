using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CoreUtil.Coroutine
{
  /// <summary>
  /// Менеджер короутин
  /// </summary>
  public class CoroutineManager
  {
    /// <summary>
    /// Запущенные короутины
    /// </summary>
    private List<IEnumerator> _runningCoroutines = new List<IEnumerator>();

    /// <summary>
    /// Короутины, которые должны быть запущены спустя некоторое время
    /// </summary>
    private List<RunAfterTimeInfo> _shouldRunAfterTimes = new List<RunAfterTimeInfo>();

    /// <summary>
    /// Короутины, которые должны быть запущены в конце кадра
    /// </summary>
    private List<IEnumerator> _shouldRunAtEndOfFrame = new List<IEnumerator>();

    /// <summary>
    /// Сверхточный счетчик времени
    /// </summary>
    private Stopwatch _stopwatch = new Stopwatch();

    /// <summary>
    /// Обработчик события конца кадра, должен быть вызван из модели в конце каждого кадра
    /// </summary>
    public void ProcessOnTheEndOfFrame()
    {
      List<IEnumerator> copyRunningCoroutines = new List<IEnumerator>(_runningCoroutines);
      _runningCoroutines.Clear();

      List<IEnumerator> copyShouldRunAtEndOfFrame = new List<IEnumerator>(_shouldRunAtEndOfFrame);
      _shouldRunAtEndOfFrame.Clear();

      foreach (var coroutine in copyRunningCoroutines)
      {
        PerformNextCoroutineStep(coroutine);
      }

      List<RunAfterTimeInfo> copyShouldRunAfterTime = new List<RunAfterTimeInfo>(_shouldRunAfterTimes);

      double stopwatchCurrentTime = _stopwatch.ElapsedMilliseconds / 1000f;

      int removeElementsCount = 0;

      foreach (var coroutineAfterTime in copyShouldRunAfterTime)
      {
        if (coroutineAfterTime.Time <= stopwatchCurrentTime)
        {
          PerformNextCoroutineStep(coroutineAfterTime.Coroutine);
          removeElementsCount++;
          continue;
        }

        break;
      }

      while (removeElementsCount > 0)
      {
        _shouldRunAfterTimes.RemoveAt(0);
        removeElementsCount--;
      }

      foreach (var coroutine in copyShouldRunAtEndOfFrame)
      {
        PerformNextCoroutineStep(coroutine);
      }
    }

    /// <summary>
    /// Запускает исполнение короутины
    /// </summary>
    /// <param name="parCoroutine">Короутина</param>
    public void StartCoroutine(IEnumerator parCoroutine)
    {
      ProcessCoroutineDecision(parCoroutine);
    }

    /// <summary>
    /// Производит стандартное продолжение работы короутины
    /// </summary>
    /// <param name="parCoroutine">Короутина</param>
    /// <returns>False если короутина полностью закончила свою работу</returns>
    private void PerformNextCoroutineStep(IEnumerator parCoroutine)
    {
      try
      {
        if (!parCoroutine.MoveNext())
        {
          return;
        }
      }
      catch (Exception ex)
      {
        return;
      }

      ProcessCoroutineDecision(parCoroutine);
    }

    /// <summary>
    /// Решение о продолжении работы короутины
    /// </summary>
    /// <param name="parCoroutine">Короутина</param>
    private void ProcessCoroutineDecision(IEnumerator parCoroutine)
    {
      if (!(parCoroutine.Current is YieldInstruction))
      {
        //Короутина вернула null или неподдерживаемый объект
        _runningCoroutines.Add(parCoroutine);
      }

      if (parCoroutine.Current is WaitForSeconds)
      {
        WaitForSeconds waitForSeconds = (WaitForSeconds) (parCoroutine.Current);
        _shouldRunAfterTimes.Add(new RunAfterTimeInfo(
          _stopwatch.ElapsedMilliseconds / 1000f + waitForSeconds.TimeInSeconds,
          parCoroutine));
      }
      else if (parCoroutine.Current is WaitForEndOfFrame)
      {
        _shouldRunAtEndOfFrame.Add(parCoroutine);
      }

      //здесь можно добавлять обработчики новых Yield-инструкций
    }


    /// <summary>
    /// Останавливает исполнение короутины
    /// </summary>
    /// <param name="parCoroutine">Короутина</param>
    public void StopCoroutine(IEnumerator parCoroutine)
    {
      if (_runningCoroutines.Contains(parCoroutine))
      {
        _runningCoroutines.Remove(parCoroutine);
        return;
      }

      if (_shouldRunAtEndOfFrame.Contains(parCoroutine))
      {
        _shouldRunAtEndOfFrame.Remove(parCoroutine);
        return;
      }

      RunAfterTimeInfo found = _shouldRunAfterTimes.FirstOrDefault(parX => parX.Coroutine == parCoroutine);
      if (found != null)
      {
        _shouldRunAfterTimes.Remove(found);
      }
    }

    #region Singleton

    private static CoroutineManager _instance;

    public static CoroutineManager Instance
    {
      get
      {
        if (_instance == null)
        {
          _instance = new CoroutineManager();
        }

        return _instance;
      }
    }

    private CoroutineManager()
    {
      _stopwatch.Start();
    }

    #endregion
  }
}