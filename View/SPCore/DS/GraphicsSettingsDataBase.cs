using System;

namespace View.SPCore.DS
{
  /// <summary>
  /// Базовый класс настроек отображения игры
  /// </summary>
  [Serializable]
  public abstract class GraphicsSettingsDataBase
  {
    /// <summary>
    /// Тип настройки закрепления максимального количества кадров в секунду
    /// </summary>
    private EAppFramerateLockType _currentFixedFramerateLockType;

    /// <summary>
    /// Тип настройки закрепления максимального количества кадров в секунду
    /// </summary>
    public EAppFramerateLockType CurrentFixedFramerateLockType
    {
      get { return _currentFixedFramerateLockType; }
      set { _currentFixedFramerateLockType = value; }
    }

    /// <summary>
    /// Временной шаг для текущей настройки закрепления максимального количества кадров в секунду
    /// </summary>
    public double FramerateTimestamp
    {
      get
      {
        switch (CurrentFixedFramerateLockType)
        {
          case EAppFramerateLockType.Unlocked:
          {
            return 0;
          }
          case EAppFramerateLockType.Locked24:
          {
            return 1.0 / 24;
          }
          case EAppFramerateLockType.Locked30:
          {
            return 1.0 / 30;
          }
          case EAppFramerateLockType.Locked50:
          {
            return 1.0 / 50;
          }
          case EAppFramerateLockType.Locked60:
          {
            return 1.0 / 60;
          }
          case EAppFramerateLockType.Locked120:
          {
            return 1.0 / 120;
          }
          case EAppFramerateLockType.Locked240:
          {
            return 1.0 / 240;
          }
          default: return 0;
        }
      }
    }

    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parCurrentFixedFramerateLockType">Выбранный тип настройки закрепления максимального количества кадров в секунду</param>
    protected GraphicsSettingsDataBase(EAppFramerateLockType parCurrentFixedFramerateLockType)
    {
      _currentFixedFramerateLockType = parCurrentFixedFramerateLockType;
    }

    /// <summary>
    /// Конструктор для установки значений по умолчанию
    /// </summary>
    public GraphicsSettingsDataBase()
    {
      SetDefaultSettings();
    }

    /// <summary>
    /// Установка значений настроек по умолчанию
    /// </summary>
    public virtual void SetDefaultSettings()
    {
      CurrentFixedFramerateLockType = EAppFramerateLockType.Locked60;

    }

  }
}