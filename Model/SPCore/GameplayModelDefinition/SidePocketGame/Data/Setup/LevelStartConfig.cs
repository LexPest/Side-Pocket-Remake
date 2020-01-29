using Model.SPCore.Managers.Sound.Data;

namespace Model.SPCore.GameplayModelDefinition.SidePocketGame.Data.Setup
{
  /// <summary>
  /// Стартовая конфигурация уровня
  /// </summary>
  public class LevelStartConfig
  {
    /// <summary>
    /// Проигрываемая мелодия уровня
    /// </summary>
    public EAppMusicAssets LevelBackgroundMusic { get; }

    /// <summary>
    /// Стартовые конфигурации шаров на уровне
    /// </summary>
    public BallStartConfig[] LevelBalls { get; }

    /// <summary>
    /// Конфигурация стола
    /// </summary>
    public GameField LevelGameField { get; }

    /// <summary>
    /// Данные конфигурации белого шара-игрока
    /// </summary>
    public BallStartConfig PlayerWhiteBall { get; }

    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parLevelGameField">Конфигурация стола</param>
    /// <param name="parLevelBalls">Стартовые конфигурации шаров на уровне</param>
    /// <param name="parPlayerWhiteBall">Данные конфигурации белого шара-игрока</param>
    /// <param name="parLevelBackgroundMusic">Проигрываемая мелодия уровня</param>
    public LevelStartConfig(GameField parLevelGameField, BallStartConfig[] parLevelBalls, BallStartConfig parPlayerWhiteBall,
      EAppMusicAssets parLevelBackgroundMusic)
    {
      LevelGameField = parLevelGameField;
      LevelBalls = parLevelBalls;
      PlayerWhiteBall = parPlayerWhiteBall;
      LevelBackgroundMusic = parLevelBackgroundMusic;
    }
  }
}