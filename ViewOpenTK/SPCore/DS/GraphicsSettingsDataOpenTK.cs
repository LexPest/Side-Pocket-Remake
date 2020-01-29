using System;
using OpenTK;
using View.SPCore.DS;

namespace ViewOpenTK.SPCore.DS
{
  /// <summary>
  /// Настройки, относящиеся только к графической составляющей (отображению)
  /// </summary>
  [Serializable]
  public sealed class GraphicsSettingsDataOpenTk : GraphicsSettingsDataBase
  {
    /// <summary>
    /// Включен ли полноэкранный режим
    /// </summary>
    private bool _isFullscreen;
    /// <summary>
    /// Текущее разрешение окна
    /// </summary>
    private ScreenResolutionOpenTk _settingScreenResolution;


    /// <summary>
    /// Конструктор, устанавливающий предоставляемые параметры отображения
    /// </summary>
    /// <param name="parCurrentFixedFramerateLockType">Тип настройки закрепления количества кадров в секунду</param>
    /// <param name="parSettingScreenResolution">Текущее разрешение окна</param>
    /// <param name="parIsFullscreen">Включен ли полноэкранный режим</param>
    public GraphicsSettingsDataOpenTk(EAppFramerateLockType parCurrentFixedFramerateLockType,
      ScreenResolutionOpenTk parSettingScreenResolution, bool parIsFullscreen)
      : base(parCurrentFixedFramerateLockType)
    {
      _settingScreenResolution = parSettingScreenResolution;
      _isFullscreen = parIsFullscreen;
    }

    /// <summary>
    /// Устанавливает стандартные настройки. Считается, что
    /// разрешение экрана будет определено по стандарту согласно
    /// размерам дисплея.
    /// </summary>
    public GraphicsSettingsDataOpenTk() : base()
    {
    }

    /// <summary>
    /// Устанавливает стандартные настройки
    /// </summary>
    public override void SetDefaultSettings()
    {
      base.SetDefaultSettings();
      SettingScreenResolution = GetHighestAvailableScreenResolution();
      IsFullscreen = true;
    }

    /// <summary>
    /// Получить максимальное доступное разрешение экрана
    /// </summary>
    /// <returns></returns>
    private static ScreenResolutionOpenTk GetHighestAvailableScreenResolution()
    {
      return new ScreenResolutionOpenTk(DisplayDevice.Default.Width, DisplayDevice.Default.Height);
    }


    /// <summary>
    /// Текущее разрешение окна
    /// </summary>
    public ScreenResolutionOpenTk SettingScreenResolution
    {
      get { return _settingScreenResolution; }
      set { _settingScreenResolution = value; }
    }

    /// <summary>
    /// Включен ли полноэкранный режим
    /// </summary>
    public bool IsFullscreen
    {
      get { return _isFullscreen; }
      set { _isFullscreen = value; }
    }
  }
}