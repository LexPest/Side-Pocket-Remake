namespace Model.SPCore.GameplayModelDefinition.SidePocketGame.Data.Setup
{
  /// <summary>
  /// Игровой уровень
  /// </summary>
  public class GameLevel
  {
    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parLevelName">Название игрового уровня</param>
    /// <param name="parActualLevelLevelConfig">Связанная стартовая конфигурация уровня</param>
    public GameLevel(string parLevelName, LevelStartConfig parActualLevelLevelConfig)
    {
      LevelName = parLevelName;
      ActualLevelLevelConfig = parActualLevelLevelConfig;
    }

    /// <summary>
    /// Название игрового уровня
    /// </summary>
    public string LevelName { get; private set; }
    
    /// <summary>
    /// Связанная стартовая конфигурация уровня
    /// </summary>
    public LevelStartConfig ActualLevelLevelConfig { get; private set; }
  }
}