using System;

namespace Controller.MInput.MDevices
{
  /// <summary>
  /// Базовый класс для представления устройств ввода (в том числе потенциальных виртуальных)
  /// </summary>
  public abstract class MDevice : IDisposable
  {
    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parDeviceNumId">Числовой идентификатор устройства</param>
    /// <param name="parDeviceStrId">Дополнительный строковый идентификатор устройства</param>
    /// <param name="parDeviceDescriptor">Строка-описание устройства, обычно содержит имя или идентификационную информацию</param>
    protected MDevice(long parDeviceNumId, string parDeviceStrId, string parDeviceDescriptor)
    {
      DeviceNumId = parDeviceNumId;
      DeviceStrId = parDeviceStrId;
      DeviceDescriptor = parDeviceDescriptor;
    }

    /// <summary>
    /// Числовой идентификатор устройства
    /// </summary>
    public long DeviceNumId { get; private set; }

    /// <summary>
    /// Дополнительный строковый идентификатор устройства
    /// </summary>
    public string DeviceStrId { get; private set; }

    /// <summary>
    /// Строка-описание устройства, обычно содержит имя или идентификационную информацию
    /// </summary>
    public string DeviceDescriptor { get; private set; }

    /// <summary>
    /// Стандартный деструктор
    /// </summary>
    public void Dispose()
    {
      OnDisconnected?.Invoke();
    }

    /// <summary>
    /// Событие для API, будет вызвано при отключении/удалении устройства
    /// </summary>
    public event Action OnDisconnected;

    /// <summary>
    /// Получение значения оси
    /// </summary>
    /// <param name="parId">Идентификатор оси</param>
    /// <returns>Значение оси</returns>
    public abstract double GetAxisValue(string parId);

    /// <summary>
    /// Получение состояния кнопки, нажата в данный момент или нет
    /// </summary>
    /// <param name="parId">Идентификатор кнопки</param>
    /// <returns>Значение кнопки</returns>
    public abstract bool GetButtonValue(string parId);
  }
}