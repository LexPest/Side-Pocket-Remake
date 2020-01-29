using System.Collections.Generic;
using System.Linq;
using Controller.MInput.MGameActionBindsToDevices;
using Model.SPCore.MPlayers;

namespace Controller.MInput.MPlayerBinds
{
  /// <summary>
  /// Контроллер игрока
  /// </summary>
  public class MPlayerController
  {
    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parMPlayerRef">Данные об игроке в модели для привязки</param>
    public MPlayerController(MPlayer parMPlayerRef)
    {
      MPlayerRef = parMPlayerRef;
    }

    /// <summary>
    /// Данные об игроке в модели
    /// </summary>
    public MPlayer MPlayerRef { get; protected set; }

    /// <summary>
    /// Динамический массив, содержащий привязки игровых осей к осям устройств в контроллере
    /// </summary>
    public List<MGameActionAxisBindToDevice> MGameActionAxisBindToDevices { get; protected set; } =
      new List<MGameActionAxisBindToDevice>();

    /// <summary>
    /// Динамический массив, содержащий привязки игровых кнопок к кнопкам устройств в контроллере
    /// </summary>
    public List<MGameActionButtonBindToDevice> MGameActionButtonBindToDevices { get; protected set; } =
      new List<MGameActionButtonBindToDevice>();

    /// <summary>
    /// Обновление состояния игровых действий (кнопок и осей) игрока. Необходимо вызывать из главного цикла приложения.
    /// </summary>
    public void UpdateInput()
    {
      List<MGameActionButtonBindToDevice> unprocessedButtons =
        new List<MGameActionButtonBindToDevice>(MGameActionButtonBindToDevices);

      var buttonsBindsByGameActions = unprocessedButtons.GroupBy(parX => parX.TargetGameActionButton);

      foreach (var buttonsBindsByGameAction in buttonsBindsByGameActions)
      {
        bool isPressed = false;
        foreach (var mGameActionButtonBindToDevice in buttonsBindsByGameAction)
        {
          if (mGameActionButtonBindToDevice.GetGameActionButton())
          {
            isPressed = true;
            break;
          }
        }

        buttonsBindsByGameAction.First().TargetGameActionButton.SetPressedStatus(isPressed);
      }

      for (var index = 0; index < MGameActionAxisBindToDevices.Count; index++)
      {
        var mGameActionAxisBindToDevice = MGameActionAxisBindToDevices[index];
        if (mGameActionAxisBindToDevice.TargetDevice != null)
        {
          mGameActionAxisBindToDevice.UpdateGameActionAxis();
        }
        else
        {
          MGameActionAxisBindToDevices.RemoveAt(index);
          index--;
        }
      }
    }
  }
}