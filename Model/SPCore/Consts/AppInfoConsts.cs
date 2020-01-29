using System;
using System.IO;
using System.Reflection;

namespace Model.SPCore.Consts
{
  /// <summary>
  /// Константы, связанные с экземпляром приложения
  /// </summary>
  public static class AppInfoConsts
  {
    /// <summary>
    /// Рабочая директория приложения
    /// </summary>
    public static readonly string WorkingPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

    /// <summary>
    /// Директория настроек приложения
    /// </summary>
    public static readonly string SettingsPath = Path.Combine(WorkingPath, "config/");

    /// <summary>
    /// Директория ресурсов игры
    /// </summary>
    public static readonly string ResourcesAssetsDirpath = $"{AppDomain.CurrentDomain.BaseDirectory}/packs/";

    /// <summary>
    /// Директория с данными об игроках
    /// </summary>
    public static readonly string PlayerdataPath = Path.Combine(WorkingPath, "player/");

    /// <summary>
    /// Путь к игровым настройкам
    /// </summary>
    public static readonly string GameplaySettingsPath = Path.Combine(SettingsPath, "gameplay.cfg");

    /// <summary>
    /// Путь к настройкам отображения и графики
    /// </summary>
    public static readonly string GraphicsSettingsPath = Path.Combine(SettingsPath, "graphics.cfg");

    /// <summary>
    /// Путь к файлу рекордов
    /// </summary>
    public static readonly string RecordsDataPath = Path.Combine(PlayerdataPath, "records.dat");

    /// <summary>
    /// Название приложения
    /// </summary>
    public static readonly string AppName = "Side Pocket Remastered";

    /// <summary>
    /// Версия приложения
    /// </summary>
    public static readonly string Version = "1.0.000";

    /// <summary>
    /// Время запуска приложения
    /// </summary>
    public static readonly DateTime StartupTime = DateTime.Now;

    /// <summary>
    /// Стартовая вместимость динамического массива обновляемых объектов
    /// </summary>
    public static readonly int UpdatableObjectsListStartCapacity = 5;

    /// <summary>
    /// Стартовая вместимость динамического массива фиксированно-обновляемых объектов
    /// </summary>
    public static readonly int FixedupdatableObjectsListStartCapacity = 5;

    /// <summary>
    /// Стандартное значение оси
    /// </summary>
    public static readonly double AxisDefaultValue = 0.0;

    /// <summary>
    /// Стандартное имя первого игрока
    /// </summary>
    public static readonly string Player1DefaultName = "Player 1";

    /// <summary>
    /// Стандартное имя второго игрока
    /// </summary>
    public static readonly string Player2DefaultName = "Player 2";

    /// <summary>
    /// Экземпляр генератора случайных чисел
    /// </summary>
    public static readonly Random AppRandom = new Random();
  }
}