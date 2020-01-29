using System;
using System.Collections.Concurrent;

namespace ViewOpenTK.OpenGL
{
  /// <summary>
  /// Класс, предназначенный для управления очередью системных или служебных команд OpenGL
  /// </summary>
  public static class OpenGlCommandsInternalHandler
  {
    /// <summary>
    /// Очередь команд
    /// </summary>
    private static ConcurrentQueue<Action> _openGlActionsQueue = new ConcurrentQueue<Action>();

    /// <summary>
    /// Добавить команду в очередь
    /// </summary>
    /// <param name="parCommand">Делегат команды</param>
    public static void AddGlCommand(Action parCommand)
    {
      _openGlActionsQueue.Enqueue(parCommand);
    }
    
    /// <summary>
    /// Получить следующую команду из очереди
    /// </summary>
    /// <returns></returns>
    public static Action GetNextGlAction()
    {
      return _openGlActionsQueue.TryDequeue(out Action nextGlCommand) ? nextGlCommand : null;
    }

    /// <summary>
    /// Все ли служебные команды из очереди выполнены?
    /// </summary>
    /// <returns>True, если все команды из очереди были выполнены</returns>
    public static bool AreAllActionsPerformed()
    {
      return _openGlActionsQueue.IsEmpty;
    }
  }
}