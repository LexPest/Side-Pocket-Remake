#region

using System;

#endregion

namespace Model.SPCore.DS
{
  /// <summary>
  /// Настройки, относящиеся только к геймплею
  /// </summary>
  [Serializable]
  public sealed class GameplaySettingsData
  {
    /// <summary>
    /// Фиксированный временной шаг для фиксированного обновления модели
    /// </summary>
    private double _appFixedTimestamp;

    /// <summary>
    /// Выключена ли вступительная заставка
    /// </summary>
    private bool _isIntroDisabled;

    /// <summary>
    /// Включена ли музыка
    /// </summary>
    private bool _isMusicEnabled;

    /// <summary>
    /// Включены ли звуковые эффекты
    /// </summary>
    private bool _isSfxEnabled;

    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parAppFixedTimestamp">Фиксированный временной шаг для фиксированного обновления модели</param>
    /// <param name="parIsIntroDisabled">Выключена ли вступительная заставка</param>
    /// <param name="parIsMusicEnabled">Включена ли музыка</param>
    /// <param name="parIsSfxEnabled">Включены ли звуковые эффекты</param>
    public GameplaySettingsData(double parAppFixedTimestamp, bool parIsIntroDisabled, bool parIsMusicEnabled,
      bool parIsSfxEnabled)
    {
      _appFixedTimestamp = parAppFixedTimestamp;
      _isIntroDisabled = parIsIntroDisabled;
      _isMusicEnabled = parIsMusicEnabled;
      _isSfxEnabled = parIsSfxEnabled;
    }

    /// <summary>
    /// Возвращает экземпляр со стандартными настройками
    /// </summary>
    /// <returns></returns>
    public static GameplaySettingsData GetDefaultSettings()
    {
      return new GameplaySettingsData(0.003, false, true, true);
    }

    /// <summary>
    /// Фиксированный временной шаг для фиксированного обновления модели
    /// </summary>
    public double AppFixedTimestamp
    {
      get { return _appFixedTimestamp; }
      set { _appFixedTimestamp = value; }
    }

    /// <summary>
    /// Выключена ли вступительная заставка
    /// </summary>
    public bool IsIntroDisabled
    {
      get => _isIntroDisabled;
      set => _isIntroDisabled = value;
    }

    /// <summary>
    /// Включены ли звуковые эффекты
    /// </summary>
    public bool IsSfxEnabled
    {
      get { return _isSfxEnabled; }
      set { _isSfxEnabled = value; }
    }

    /// <summary>
    /// Включена ли музыка
    /// </summary>
    public bool IsMusicEnabled
    {
      get { return _isMusicEnabled; }
      set { _isMusicEnabled = value; }
    }
  }
}