using System;
using Model.SPCore.MGameActions;
using Model.SPCore.MPlayers;

namespace Model.SPCore.GameplayModelDefinition.GameComponents.Universal.DS
{
  /// <summary>
  /// Данные для обработчика ввода от игрока
  /// </summary>
  public class KeyListenerData
  {
    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parButtonToCheck">Игровая кнопка-действие, которое нужно проверять</param>
    /// <param name="parPlayerToCheck">Игрок, которого нужно проверять</param>
    /// <param name="parPerformOnSuccess">Действия при нажатии на проверяемую игровую кнопку</param>
    /// <param name="parAutoRemove">Следует ли убрать действие после первого успешного выполнения</param>
    public KeyListenerData(EGameActionButton parButtonToCheck, MPlayer parPlayerToCheck, Action parPerformOnSuccess,
      bool parAutoRemove)
    {
      ButtonToCheck = parButtonToCheck;
      PlayerToCheck = parPlayerToCheck;
      PerformOnSuccess = parPerformOnSuccess;
      AutoRemove = parAutoRemove;
    }

    /// <summary>
    /// Игровая кнопка-действие, которое нужно проверять
    /// </summary>
    public EGameActionButton ButtonToCheck { get; set; }

    /// <summary>
    /// Игрок, которого нужно проверять
    /// </summary>
    public MPlayer PlayerToCheck { get; set; }

    /// <summary>
    /// Действия при нажатии на проверяемую игровую кнопку
    /// </summary>
    public Action PerformOnSuccess { get; set; }

    /// <summary>
    /// Следует ли убрать действие после первого успешного выполнения
    /// </summary>
    public bool AutoRemove { get; set; }
  }
}