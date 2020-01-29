using Controller.MInput.MDevices;
using Model.SPCore.MGameActions;

namespace Controller.MInput.MGameActionBindsToDevices
{
  /// <summary>
  /// Класс предназначен для привязки значения игровой кнопки в модели к конкретному устройству в контроллере
  /// </summary>
  public class MGameActionButtonBindToDevice
  {
    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parTargetDevice">Идентификатор кнопки целевого устройства</param>
    /// <param name="parDeviceButtonId">Целевое устройство для привязки</param>
    /// <param name="parTargetGameActionButton">Целевая игровая кнопка на стороне модели для привязки</param>
    public MGameActionButtonBindToDevice(MDevice parTargetDevice, string parDeviceButtonId,
      MGameActionButton parTargetGameActionButton)
    {
      TargetDevice = parTargetDevice;
      DeviceButtonId = parDeviceButtonId;
      TargetGameActionButton = parTargetGameActionButton;
    }

    /// <summary>
    /// Идентификатор кнопки целевого устройства
    /// </summary>
    public string DeviceButtonId { get; }

    /// <summary>
    /// Целевое устройство для привязки
    /// </summary>
    public MDevice TargetDevice { get; }

    /// <summary>
    /// Целевая игровая кнопка на стороне модели для привязки
    /// </summary>
    public MGameActionButton TargetGameActionButton { get; }

    /// <summary>
    /// Обновить значение игровой кнопки на стороне модели
    /// </summary>
    public void UpdateGameActionButton()
    {
      TargetGameActionButton.SetPressedStatus(TargetDevice.GetButtonValue(DeviceButtonId));
    }

    /// <summary>
    /// Получить 'сырое' значение игровой оси на стороне контроллера
    /// </summary>
    /// <returns>'Сырое' значение игровой кнопки устройства</returns>
    public bool GetGameActionButton()
    {
      return TargetDevice.GetButtonValue(DeviceButtonId);
    }
  }
}