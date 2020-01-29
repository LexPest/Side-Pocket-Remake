using System;

namespace Model.SPCore.DS
{
  /// <summary>
  /// Сведения о рекорде игрока
  /// </summary>
  [Serializable]
  public class RecordPlayerInfo
  {
    /// <summary>
    /// Максимум символов в имени
    /// </summary>
    public const int MAX_CHARS_IN_NAME = 8;

    /// <summary>
    /// Имя игрока
    /// </summary>
    private string _playerName;

    /// <summary>
    /// Количество заработанных очков
    /// </summary>
    private long _pointsEarned;

    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parPointsEarned">Заработано очков</param>
    /// <param name="parPlayerName">Имя игрока</param>
    public RecordPlayerInfo(long parPointsEarned, string parPlayerName)
    {
      _pointsEarned = parPointsEarned;
      PlayerName = parPlayerName;
    }

    /// <summary>
    /// Применить ограничения имени
    /// </summary>
    public void NameConstraintsApply()
    {
      if (_playerName != null)
      {
        if (_playerName.Length > MAX_CHARS_IN_NAME)
        {
          _playerName = _playerName.Remove(MAX_CHARS_IN_NAME, _playerName.Length - MAX_CHARS_IN_NAME);
        }
      }
    }

    /// <summary>
    /// Количество заработанных очков
    /// </summary>
    public long PointsEarned
    {
      get { return _pointsEarned; }
      set { _pointsEarned = value; }
    }

    /// <summary>
    /// Имя игрока
    /// </summary>
    public string PlayerName
    {
      get { return _playerName; }
      set
      {
        _playerName = value;
        NameConstraintsApply();
      }
    }
  }
}