using Model.SPCore.MPlayers;

namespace Model.SPCore.GameplayModelDefinition.SidePocketGame.Data
{
  /// <summary>
  /// Игрок (связан непосредственно с игровой сессией)
  /// </summary>
  public class Player
  {
    /// <summary>
    /// Набрано очков
    /// </summary>
    private int _score;
    
    /// <summary>
    /// Связанный обработчик ввода
    /// </summary>
    public MPlayer PlayerInput;

    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parPlayerInput">Связанный обработчик ввода</param>
    /// <param name="parScore">Набрано очков</param>
    /// <param name="parLifeCount">Оставшееся количество жизней</param>
    public Player(MPlayer parPlayerInput, int parScore, int parLifeCount)
    {
      PlayerInput = parPlayerInput;
      Score = parScore;
      LifeCount = parLifeCount;
    }

    /// <summary>
    /// Набрано очков
    /// </summary>
    public int Score
    {
      get => _score;
      set { _score = value >= 0 ? value : 0; }
    }

    /// <summary>
    /// Оставшееся количество жизней
    /// </summary>
    public int LifeCount { get; set; }
  }
}