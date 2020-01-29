using System;
using System.Collections.Generic;
using Model.SPCore.MGameActions;

namespace Model.SPCore.MPlayers
{
  /// <summary>
  /// Информация об игроке с точки зрения ввода и прикрепляемых отслеживаемых игровых действий
  /// </summary>
  public class MPlayer
  {
    /// <summary>
    /// Прикрепленные для отслеживания игровые "оси"
    /// </summary>
    public Dictionary<EGameActionAxis, MGameActionAxis> GameActionAxises =
      new Dictionary<EGameActionAxis, MGameActionAxis>();

    /// <summary>
    /// Прикрепленные для отслеживания игровые кнопки
    /// </summary>
    public Dictionary<EGameActionButton, MGameActionButton> GameActionButtons =
      new Dictionary<EGameActionButton, MGameActionButton>();

    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parPlayerId">Идентификатор игрока</param>
    /// <param name="parPlayerRole">Назначенная игроку роль</param>
    public MPlayer(string parPlayerId, EPlayerRoles parPlayerRole)
    {
      this.PlayerId = parPlayerId;
      this.PlayerRole = parPlayerRole;
    }

    /// <summary>
    /// Событие, игрок сейчас активен и доступен
    /// </summary>
    public event Action PlayerBecomeActive;

    /// <summary>
    /// Событие, игрок перестал быть активным и сейчас недоступен
    /// </summary>
    public event Action PlayerBecomeUnavailable;

    /// <summary>
    /// Установить флаг активации игрока
    /// </summary>
    /// <param name="parNewValue">Значение флага активации игрока</param>
    public void SetActive(bool parNewValue)
    {
      var previousValue = IsActive;

      IsActive = parNewValue;

      if (parNewValue)
      {
        if (!previousValue)
        {
          PlayerBecomeActive?.Invoke();
        }
      }
      else
      {
        if (previousValue)
        {
          PlayerBecomeUnavailable?.Invoke();
        }
      }
    }

    /// <summary>
    /// Получить состояние определенной игровой оси
    /// </summary>
    /// <param name="parId">Тип игровой оси</param>
    /// <returns></returns>
    public MGameActionAxis GetGameActionAxis(EGameActionAxis parId)
    {
      if (GameActionAxises.ContainsKey(parId))
      {
        return GameActionAxises[parId];
      }
      else
      {
        return new MGameActionAxis(0.0);
      }
    }

    /// <summary>
    /// Получить состояние определенной игровой кнопки
    /// </summary>
    /// <param name="parId">Тип игровой кнопки</param>
    /// <returns></returns>
    public MGameActionButton GetGameActionButton(EGameActionButton parId)
    {
      if (GameActionButtons.ContainsKey(parId))
      {
        return GameActionButtons[parId];
      }
      else
      {
        return new MGameActionButton();
      }
    }

    /// <summary>
    /// Идентификатор игрока
    /// </summary>
    public string PlayerId { get; private set; }

    /// <summary>
    /// Назначенная системная роль игрока
    /// </summary>
    public EPlayerRoles PlayerRole { get; private set; }

    /// <summary>
    /// Флаг, обозначающий является ли сейчас игрок активным и доступным
    /// </summary>
    public bool IsActive { get; private set; }
  }
}