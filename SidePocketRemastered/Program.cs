#region

using System;
using ControllerOpenTK.SPCore;
using Model.SPCore;
using Model.SPCore.Consts;

#endregion

namespace SidePocketRemastered
{
  /// <summary>
  /// Главный класс программы с точкой входа
  /// </summary>
  internal class Program
  {
    /// <summary>
    /// Главная точка входа программы
    /// </summary>
    /// <param name="args">Аргументы запуска программы (в текущей версии не используется)</param>
    private static void Main(string[] args)
    {
      Console.WriteLine("Starting engine...");
      Console.WriteLine($"Working dir: {AppInfoConsts.WorkingPath}\nSettings path: {AppInfoConsts.SettingsPath}");

      AppModel appModel = new AppModel();
      AppControllerOpenTk appControllerOpenTk = new AppControllerOpenTk(appModel);

      appControllerOpenTk.Run();
    }
  }
}