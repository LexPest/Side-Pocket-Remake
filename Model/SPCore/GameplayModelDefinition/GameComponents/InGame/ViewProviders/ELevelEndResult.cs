namespace Model.SPCore.GameplayModelDefinition.GameComponents.InGame.ViewProviders
{
  /// <summary>
  /// Результат окончания игрового уровня
  /// </summary>
  public enum ELevelEndResult
  {
    /// <summary>
    /// Отсутствует
    /// </summary>
    None,
    /// <summary>
    /// Победа, уровень пройден
    /// </summary>
    Win,
    /// <summary>
    /// Поражение, игрок теряет все
    /// </summary>
    Lose
  }
}