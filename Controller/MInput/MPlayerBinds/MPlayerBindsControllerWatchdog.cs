using System;
using System.Collections.Generic;
using System.Linq;
using Model.SPCore;
using Model.SPCore.DS;
using Model.SPCore.MPlayers;

namespace Controller.MInput.MPlayerBinds
{
  /// <summary>
  /// Наблюдатель за событиями, касающихся игроков модели
  /// </summary>
  public class MPlayerBindsControllerWatchdog
  {
    /// <summary>
    /// Целевая модель приложения
    /// </summary>
    private readonly AppModel _appModel;

    /// <summary>
    /// Стандартный конструктор, имеет проверку на то, что модель еще не запущена
    /// </summary>
    /// <param name="parAppModel">Целевая модель приложения</param>
    /// <exception cref="ApplicationException">Модель не должна быть запущена, а только создана</exception>
    public MPlayerBindsControllerWatchdog(AppModel parAppModel)
    {
      _appModel = parAppModel;
      if (_appModel.CurrentAppState.CurrentBaseAppState != EBaseAppStates.Created)
      {
        throw new ApplicationException("Watchdogs should be launched before app startup!");
      }

      PlayerControllers = new List<MPlayerController>();
      _appModel.PlayersManager.OnPlayerAdded += OnPlayerAddedHandler;
      _appModel.PlayersManager.OnPlayerRemoved += OnPlayerRemovedHandler;
    }

    /// <summary>
    /// Динамический массив контроллеров игроков
    /// </summary>
    public List<MPlayerController> PlayerControllers { get; protected set; }

    /// <summary>
    /// Стандартный деструктор
    /// </summary>
    ~MPlayerBindsControllerWatchdog()
    {
      if (_appModel?.PlayersManager != null)
      {
        _appModel.PlayersManager.OnPlayerAdded -= OnPlayerAddedHandler;
        _appModel.PlayersManager.OnPlayerRemoved -= OnPlayerRemovedHandler;
      }
    }

    /// <summary>
    /// Обработчик события добавления нового игрока
    /// </summary>
    /// <param name="parPlayer">Новый игрок</param>
    private void OnPlayerAddedHandler(MPlayer parPlayer)
    {
      PlayerControllers.Add(new MPlayerController(parPlayer));
    }

    /// <summary>
    /// Обработчик события удаления игрока
    /// </summary>
    /// <param name="parPlayer">Удаленный игрок</param>
    private void OnPlayerRemovedHandler(MPlayer parPlayer)
    {
      MPlayerController foundPlayerController = PlayerControllers.FirstOrDefault(parX => parX.MPlayerRef == parPlayer);
      if (foundPlayerController == null)
      {
        return;
      }

      PlayerControllers.Remove(foundPlayerController);
    }
  }
}