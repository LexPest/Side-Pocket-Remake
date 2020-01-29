using System.Drawing;

namespace ViewOpenTK.SPCore.ViewProvider.RenderingData
{
  /// <summary>
  /// Данные о заполняющем экран одноцветном примитиве
  /// </summary>
  public struct RenderingFillColorScreen
  {
    /// <summary>
    /// Глубина
    /// </summary>
    public double Depth { get; }

    /// <summary>
    /// Цвет
    /// </summary>
    public Color Color { get; }


    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parDepth">Глубина</param>
    /// <param name="parColor">Цвет</param>
    public RenderingFillColorScreen(double parDepth, Color parColor)
    {
      Depth = parDepth;
      Color = parColor;
    }
  }
}