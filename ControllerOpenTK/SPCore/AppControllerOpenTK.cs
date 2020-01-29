using System.Linq;
using Controller.MInput.MGameActionBindsToDevices;
using Controller.MInput.MPlayerBinds;
using Controller.SPCore;
using ControllerOpenTK.MInput.MDevices;
using CoreUtil.ResourceManager;
using CoreUtil.ResourceManager.AssetData.AssetDatabaseUpdateStrategies;
using CoreUtil.ResourceManager.AssetData.Builders;
using Model.SPCore;
using Model.SPCore.MGameActions;
using OpenTK.Input;
using ViewOpenTK.AssetData;
using ViewOpenTK.AssetData.AssetDatabaseUpdateStrategies;
using ViewOpenTK.AssetData.Builders;
using ViewOpenTK.SPCore;

namespace ControllerOpenTK.SPCore
{
  /// <summary>
  /// Контроллер приложения OpenTK
  /// </summary>
  public class AppControllerOpenTk : AppControllerBase
  {
    /// <summary>
    /// Менеджер устройств OpenTK
    /// </summary>
    private MDevicesManagerOpenTk _devicesManagerOpenTk;

    /// <summary>
    /// Контроллер-наблюдатель за игроками
    /// </summary>
    private MPlayerController _mainPlayerRef;

    /// <summary>
    ///  Стандартный конструктор. Создает экземпляр приложения со стандартными настройками.
    /// </summary>
    /// <param name="parApp">Модель приложения</param>
    public AppControllerOpenTk(AppModel parApp) : base(parApp)
    {
      CreateView();
    }

    /// <summary>
    ///  Дополнительный конструктор. Создает экземпляр приложения с заданными настройками.
    /// </summary>
    /// <param name="parApp">Модель приложения</param>
    /// <param name="parGraphicsSettingsPath">Путь к файлу с настройками отображения</param>
    public AppControllerOpenTk(AppModel parApp, string parGraphicsSettingsPath)
      : base(parApp)
    {
      CreateView(parGraphicsSettingsPath);
    }

    /// <summary>
    /// Создание отображения-вида
    /// </summary>
    /// <param name="parGraphicsSettingsPath">Путь к файлу с настройками отображения</param>
    private void CreateView(string parGraphicsSettingsPath = null)
    {
      AppView = new AppViewOpenTk(App, parGraphicsSettingsPath);
      _devicesManagerOpenTk = new MDevicesManagerOpenTk((AppViewOpenTk) AppView, App);
    }

    /// <summary>
    /// Обработка ввода
    /// </summary>
    /// <param name="parDeltaTime">Время кадра обработки устройств в миллисекундах</param>
    protected override void InputHandle(double parDeltaTime)
    {
      if (_mainPlayerRef == null)
      {
        if (PlayerBindsControllerWatchdog != null)
        {
          _mainPlayerRef = PlayerBindsControllerWatchdog.PlayerControllers.First(
            parX => parX.MPlayerRef == App.PlayersManager.Player1);

          _devicesManagerOpenTk.OnDeviceAdded += AssignToMainPlayer;
          _devicesManagerOpenTk.OnBeforeDeviceRemoval += DeassignFromAllPlayers;

          AssignAllToMainPlayer();
        }
      }


      if (_mainPlayerRef != null)
      {
        _devicesManagerOpenTk.DevicesCheckPolling();
        _devicesManagerOpenTk.UpdateDevicesStates();
      }
    }

    /// <summary>
    /// Присвоить владение всеми устройствами главному игроку
    /// </summary>
    protected void AssignAllToMainPlayer()
    {
      foreach (var device in _devicesManagerOpenTk.AvailableDevices)
      {
        AssignToMainPlayer(device);
      }
    }

    /// <summary>
    /// Присвоить владение устройством главному игроку
    /// </summary>
    /// <param name="parDevice">Физическое устройство OpenTK</param>
    protected void AssignToMainPlayer(MDeviceOpenTk parDevice)
    {
      DefineAndPerformStandardAssignToPlayer(_mainPlayerRef, parDevice);
    }

    /// <summary>
    /// Убрать владение устройстом со всех игроков
    /// </summary>
    /// <param name="parDevice">Физическое устройство OpenTK</param>
    protected void DeassignFromAllPlayers(MDeviceOpenTk parDevice)
    {
      var players = PlayerBindsControllerWatchdog.PlayerControllers;
      foreach (var player in players)
      {
        var bindsToDeviceAxis = player.MGameActionAxisBindToDevices.FindAll(parX => parX.TargetDevice == parDevice);
        foreach (var bindToDevice in bindsToDeviceAxis)
        {
          player.MGameActionAxisBindToDevices.Remove(bindToDevice);
        }

        var bindsToDeviceButtons =
          player.MGameActionButtonBindToDevices.FindAll(parX => parX.TargetDevice == parDevice);
        foreach (var bindToDevice in bindsToDeviceButtons)
        {
          player.MGameActionButtonBindToDevices.Remove(bindToDevice);
        }
      }
    }

    /// <summary>
    /// Определить и назначить стандартное управление для игрока
    /// </summary>
    /// <param name="parMPlayerController">Контроллер игрока</param>
    /// <param name="parAvailableDevice">Доступное физическое устройство OpenTK</param>
    protected void DefineAndPerformStandardAssignToPlayer(MPlayerController parMPlayerController,
      MDeviceOpenTk parAvailableDevice)
    {
      void ConditionalAxisBindToDevice(EGameActionAxis parGameActionAxis, MDeviceOpenTk parDeviceOpenTk,
        string parAxisId)
      {
        if (parMPlayerController.MPlayerRef.GameActionAxises.ContainsKey(parGameActionAxis))
        {
          parMPlayerController.MGameActionAxisBindToDevices.Add(
            new MGameActionAxisBindToDevice(parDeviceOpenTk, parAxisId,
              parMPlayerController.MPlayerRef.GameActionAxises[parGameActionAxis]));
        }
      }

      void ConditionalButtonBindToDevice(EGameActionButton parGameActionButton, MDeviceOpenTk parDeviceOpenTk,
        string parButtonId)
      {
        if (parMPlayerController.MPlayerRef.GameActionButtons.ContainsKey(parGameActionButton))
        {
          parMPlayerController.MGameActionButtonBindToDevices.Add(
            new MGameActionButtonBindToDevice(parDeviceOpenTk, parButtonId,
              parMPlayerController.MPlayerRef.GameActionButtons[parGameActionButton]));
        }
      }

      if (parAvailableDevice.IsDeviceKeyboardAndMouse())
      {
        ConditionalButtonBindToDevice(EGameActionButton.Button_A, parAvailableDevice,
          ((int) Key.Space).ToString());
        ConditionalButtonBindToDevice(EGameActionButton.Button_B, parAvailableDevice,
          ((int) Key.Escape).ToString());
        ConditionalButtonBindToDevice(EGameActionButton.Button_X, parAvailableDevice,
          ((int) Key.E).ToString());
        ConditionalButtonBindToDevice(EGameActionButton.Button_Y, parAvailableDevice,
          ((int) Key.Q).ToString());

        ConditionalButtonBindToDevice(EGameActionButton.Dpad_Menu_Up, parAvailableDevice,
          ((int) Key.W).ToString());
        ConditionalButtonBindToDevice(EGameActionButton.Dpad_Menu_Down, parAvailableDevice,
          ((int) Key.S).ToString());
        ConditionalButtonBindToDevice(EGameActionButton.Dpad_Menu_Left, parAvailableDevice,
          ((int) Key.A).ToString());
        ConditionalButtonBindToDevice(EGameActionButton.Dpad_Menu_Right, parAvailableDevice,
          ((int) Key.D).ToString());

        ConditionalButtonBindToDevice(EGameActionButton.Dpad_Menu_Up, parAvailableDevice,
          ((int) Key.Up).ToString());
        ConditionalButtonBindToDevice(EGameActionButton.Dpad_Menu_Down, parAvailableDevice,
          ((int) Key.Down).ToString());
        ConditionalButtonBindToDevice(EGameActionButton.Dpad_Menu_Left, parAvailableDevice,
          ((int) Key.Left).ToString());
        ConditionalButtonBindToDevice(EGameActionButton.Dpad_Menu_Right, parAvailableDevice,
          ((int) Key.Right).ToString());

        ConditionalButtonBindToDevice(EGameActionButton.Button_Start, parAvailableDevice,
          ((int) Key.Enter).ToString());

        ConditionalButtonBindToDevice(EGameActionButton.Button_Bumper_Shift, parAvailableDevice,
          ((int) Key.ShiftLeft).ToString());
      }
      else
      {
        ConditionalAxisBindToDevice(EGameActionAxis.LeftCursorX, parAvailableDevice,
          MDeviceOpenTk.THUMBSTICK_LEFT_X_ID);
        ConditionalAxisBindToDevice(EGameActionAxis.LeftCursorY, parAvailableDevice,
          MDeviceOpenTk.THUMBSTICK_LEFT_Y_ID);

        ConditionalButtonBindToDevice(EGameActionButton.Button_A, parAvailableDevice,
          MDeviceOpenTk.GAMEPAD_BUTTON_BOTTOM_1_ID);
        ConditionalButtonBindToDevice(EGameActionButton.Button_B, parAvailableDevice,
          MDeviceOpenTk.GAMEPAD_BUTTON_BOTTOM_2_ID);
        ConditionalButtonBindToDevice(EGameActionButton.Button_X, parAvailableDevice,
          MDeviceOpenTk.GAMEPAD_BUTTON_TOP_1_ID);
        ConditionalButtonBindToDevice(EGameActionButton.Button_Y, parAvailableDevice,
          MDeviceOpenTk.GAMEPAD_BUTTON_TOP_2_ID);

        ConditionalButtonBindToDevice(EGameActionButton.Dpad_Menu_Up, parAvailableDevice,
          MDeviceOpenTk.GAMEPAD_BUTTON_DPAD_UP_ID);
        ConditionalButtonBindToDevice(EGameActionButton.Dpad_Menu_Down, parAvailableDevice,
          MDeviceOpenTk.GAMEPAD_BUTTON_DPAD_DOWN_ID);
        ConditionalButtonBindToDevice(EGameActionButton.Dpad_Menu_Left, parAvailableDevice,
          MDeviceOpenTk.GAMEPAD_BUTTON_DPAD_LEFT_ID);
        ConditionalButtonBindToDevice(EGameActionButton.Dpad_Menu_Right, parAvailableDevice,
          MDeviceOpenTk.GAMEPAD_BUTTON_DPAD_RIGHT_ID);

        ConditionalButtonBindToDevice(EGameActionButton.Button_Start, parAvailableDevice,
          MDeviceOpenTk.GAMEPAD_BUTTON_CENTER_1_ID);
      }
    }

    /// <summary>
    /// Получает строитель для обработки игровых ассетов и ресурсов
    /// </summary>
    /// <returns>Строитель для обработки игровых ассетов и ресурсов</returns>
    protected override AssetDataAbstractBuilder GetConcreteAssetDataBuilder()
    {
      return new AssetDataOpenTkBuilder();
    }

    /// <summary>
    /// Получает стратегию для построения базы данных игровых ассетов и ресурсов
    /// </summary>
    /// <returns>Стратегия для построения базы данных игровых ассетов и ресурсов</returns>
    protected override IAssetDatabaseUpdateStrategy GetConcreteAssetDatabaseUpdateStrategy()
    {
      return new AssetDatabaseUpdateDiffByExtensionOpenTkStrategy(ResourceManager.StandardExtensionToAssetType,
        AssetDataRelatedConsts.AssetFileExtensionToOpenTkAssetType);
    }

    /// <summary>
    /// Вызов отрисовки
    /// </summary>
    protected override void Render()
    {
      AppView.Render();
    }
  }
}