namespace Model.SPCore.GameplayModelDefinition.GameComponents.InGame.ResultScreens
{
  /// <summary>
  /// Итоговое выбранное игроком действие после экрана прохождения уровня
  /// </summary>
  public enum EResultsScreenFinalChoice
  {
    /// <summary>
    /// Отсутствует
    /// </summary>
    None,
    /// <summary>
    /// Сохранить результат и закончить игровую сессию
    /// </summary>
    BankAndExit,
    /// <summary>
    /// Продолжить играть рискуя
    /// </summary>
    ContinuePlaying
  }
}