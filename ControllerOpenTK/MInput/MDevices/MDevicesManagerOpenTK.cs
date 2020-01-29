using System;
using System.Collections.Generic;
using Model.SPCore;
using OpenTK.Input;
using ViewOpenTK.SPCore;

namespace ControllerOpenTK.MInput.MDevices
{
  /// <summary>
  /// Главной задачей менеджера является опрос-поллинг устройств для добавления и активации новых,
  /// а также удаления и деактивации отключенных или находящихся в некорректном состоянии
  /// </summary>
  public class MDevicesManagerOpenTk
  {
    /// <summary>
    /// Максимальное число геймпадов (ограничение OpenTK)
    /// </summary>
    private const byte MAX_GAMEPADS_NUMBER = 4;

    /// <summary>
    /// Специальный внутренний идентификатор для устройства клавиатуры
    /// </summary>
    public const short KEYBOARD_DEVICE_SPECIAL_ID = -1;

    /// <summary>
    /// Специальный внутренний дескриптор для устройства клавиатуры
    /// </summary>
    public const string KEYBOARD_DEVICE_DESCRIPTOR = "Keyboard and mouse";

    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parAppView">Компонент вида OpenTK</param>
    /// <param name="parModel">Компонент модели приложения</param>
    public MDevicesManagerOpenTk(AppViewOpenTk parAppView, AppModel parModel)
    {
      OpenTkAppView = parAppView;
      ActualAppModel = parModel;
      AvailableDevices = new List<MDeviceOpenTk>();

      //устройство клавиатуры присутствует всегда

      DeviceAdd(new MDeviceOpenTk(KEYBOARD_DEVICE_SPECIAL_ID, KEYBOARD_DEVICE_DESCRIPTOR, KEYBOARD_DEVICE_DESCRIPTOR,
        parAppView));
      DevicesCheckPolling();

      OpenTkAppView.KeyboardLetterKeyPressed += NewLetterKeyPressed;
    }

    /// <summary>
    /// Доступные устройства
    /// </summary>
    public List<MDeviceOpenTk> AvailableDevices { get; protected set; }

    /// <summary>
    /// Компонент вида OpenTK
    /// </summary>
    private AppViewOpenTk OpenTkAppView { get; set; }

    /// <summary>
    /// Компонент модели приложения
    /// </summary>
    private AppModel ActualAppModel { get; set; }

    /// <summary>
    /// Событие перед удалением деактивированного устройства
    /// </summary>
    public event Action<MDeviceOpenTk> OnBeforeDeviceRemoval;

    /// <summary>
    /// Событие при добавлении активированного устройства
    /// </summary>
    public event Action<MDeviceOpenTk> OnDeviceAdded;

    /// <summary>
    /// Запрос обновления состояния устройств
    /// </summary>
    public void UpdateDevicesStates()
    {
      foreach (var availableDevice in AvailableDevices)
      {
        availableDevice.RequestDeviceState();
      }
    }

    /// <summary>
    /// Операция добавления устройства
    /// </summary>
    /// <param name="parDevice">Физическое устройство OpenTK</param>
    private void DeviceAdd(MDeviceOpenTk parDevice)
    {
      AvailableDevices.Add(parDevice);
      OnDeviceAdded?.Invoke(parDevice);
    }

    /// <summary>
    /// Операция удаления устройства
    /// </summary>
    /// <param name="parDevice">Физическое устройство OpenTK</param>
    private void DeviceRemove(MDeviceOpenTk parDevice)
    {
      OnBeforeDeviceRemoval?.Invoke(parDevice);
      AvailableDevices.Remove(parDevice);
    }

    /// <summary>
    /// Операция удаления устройства
    /// </summary>
    /// <param name="parIndex">Индекс физического устройства OpenTK</param>
    private void DeviceRemove(int parIndex)
    {
      OnBeforeDeviceRemoval?.Invoke(AvailableDevices[parIndex]);
      AvailableDevices.RemoveAt(parIndex);
    }

    /// <summary>
    /// Обработчик нажатия нового символа текстового ввода на клавиатуре
    /// </summary>
    /// <param name="parLetterKey">Символьная последовательность</param>
    private void NewLetterKeyPressed(string parLetterKey)
    {
      ActualAppModel.PlayersManager.UpdateLastPressedKeyboardKey(parLetterKey);
    }

    /// <summary>
    /// Осуществляет активный поллинг-опрос,
    /// должен вызываться каждый кадр
    /// </summary>
    public void DevicesCheckPolling()
    {
      //проверка, не отключены ли текущие устройства
      for (var i = 0; i < AvailableDevices.Count; i++)
      {
        if (AvailableDevices[i].DeviceNumId != KEYBOARD_DEVICE_SPECIAL_ID)
        {
          if (!AvailableDevices[i].IsJoystickValidAndAvailable())
          {
            AvailableDevices[i].Dispose();
            DeviceRemove(i);
            i--;
          }
        }
      }

      //проверка, не подключены ли новые устройства
      for (var j = 0; j < MAX_GAMEPADS_NUMBER; j++)
      {
        //проверим, что устройство с таким идентификатором нам не известно
        if (AvailableDevices.Find(parX => parX.DeviceNumId == j) == null)
        {
          var state = GamePad.GetState(j);
          if (state.IsConnected)
          {
            //да, это явно новое устройство и нам нужно его зарегистрировать
            DeviceAdd(new MDeviceOpenTk(j, GamePad.GetName(j), GamePad.GetName(j), OpenTkAppView));
          }
        }
      }
    }
  }
}