using System;

namespace ViewOpenTK.OpenTKInput
{
  /// <summary>
  /// Содержит информацию о запросе состояния устройства ввода
  /// </summary>
  public class StateRequest
  {
    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parRequestedDeviceType">Тип устройства</param>
    /// <param name="parOnStateArrived">Действие с полученным состоянием для выполнения</param>
    /// <param name="parDeviceNumId">Числовой идентификатор устройства</param>
    public StateRequest(DeviceType parRequestedDeviceType, Action<object> parOnStateArrived, int parDeviceNumId)
    {
      RequestedDeviceType = parRequestedDeviceType;
      OnStateArrived = parOnStateArrived;
      DeviceNumId = parDeviceNumId;
    }

    /// <summary>
    /// Тип устройства
    /// </summary>
    public DeviceType RequestedDeviceType { get; private set; }

    /// <summary>
    /// Действие с полученным состоянием для выполнения
    /// </summary>
    public Action<object> OnStateArrived { get; private set; }

    /// <summary>
    /// Числовой идентификатор устройства
    /// </summary>
    public int DeviceNumId { get; private set; }
  }
}