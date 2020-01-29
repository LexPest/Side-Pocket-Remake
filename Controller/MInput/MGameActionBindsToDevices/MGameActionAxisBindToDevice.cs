using Controller.MInput.MDevices;
using Model.SPCore.MGameActions;

namespace Controller.MInput.MGameActionBindsToDevices
{
  /// <summary>
  /// Класс предназначен для привязки значения игровой оси в модели к конкретному устройству в контроллере
  /// </summary>
  public class MGameActionAxisBindToDevice
  {
    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parTargetDevice">Идентификатор оси целевого устройства</param>
    /// <param name="parDeviceAxisId">Целевое устройство для привязки</param>
    /// <param name="parTargetGameActionAxis">Целевая игровая ось на стороне модели для привязки</param>
    public MGameActionAxisBindToDevice(MDevice parTargetDevice, string parDeviceAxisId,
      MGameActionAxis parTargetGameActionAxis)
    {
      TargetDevice = parTargetDevice;
      DeviceAxisId = parDeviceAxisId;
      TargetGameActionAxis = parTargetGameActionAxis;
    }

    /// <summary>
    /// Идентификатор оси целевого устройства
    /// </summary>
    public string DeviceAxisId { get; }

    /// <summary>
    /// Целевое устройство для привязки
    /// </summary>
    public MDevice TargetDevice { get; }

    /// <summary>
    /// Целевая игровая ось на стороне модели для привязки
    /// </summary>
    public MGameActionAxis TargetGameActionAxis { get; }

    /// <summary>
    /// Обновить значение игровой оси на стороне модели
    /// </summary>
    public void UpdateGameActionAxis()
    {
      TargetGameActionAxis.SetAxisValue(TargetDevice.GetAxisValue(DeviceAxisId));
    }

    /// <summary>
    /// Получить 'сырое' значение игровой оси на стороне контроллера
    /// </summary>
    /// <returns>'Сырое' значение игровой оси устройства</returns>
    public double GetGameActionAxis()
    {
      return TargetDevice.GetAxisValue(DeviceAxisId);
    }
  }
}