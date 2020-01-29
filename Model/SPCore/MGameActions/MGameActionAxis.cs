using System;
using CoreUtil.GlobDefinitions;

namespace Model.SPCore.MGameActions
{
  /// <summary>
  /// Игровое действие типа "ось"
  /// </summary>
  public class MGameActionAxis
  {
    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parDefaultValue">Значение оси по умолчанию</param>
    public MGameActionAxis(double parDefaultValue)
    {
      Value = parDefaultValue;
      Delta = 0.0;
    }

    /// <summary>
    /// Событие изменения значения оси
    /// </summary>
    public event Action<double, double> AxisValueChanged;

    /// <summary>
    /// Установка нового значения оси. Предназначен для вызоыва из контроллера.
    /// </summary>
    /// <param name="parNewAxisValue"></param>
    public void SetAxisValue(double parNewAxisValue)
    {
      Delta = 0.0;
      if (Math.Abs(parNewAxisValue - Value) > GlobalDefinitionsConsts.FloatingPointCompTolerance)
      {
        Delta = parNewAxisValue - Value;
        Value = parNewAxisValue;
        AxisValueChanged?.Invoke(Value, Delta);
      }
    }

    /// <summary>
    /// Значение оси
    /// </summary>
    public double Value { get; private set; }

    /// <summary>
    /// Величина изменения значения оси
    /// </summary>
    public double Delta { get; private set; }
  }
}