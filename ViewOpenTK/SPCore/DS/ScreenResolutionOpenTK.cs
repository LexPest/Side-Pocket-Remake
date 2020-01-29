#region

using System;

#endregion

namespace ViewOpenTK.SPCore.DS
{
  /// <summary>
  /// Разрешение экрана
  /// </summary>
  [Serializable]
  public sealed class ScreenResolutionOpenTk
  {
    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parWidth">Ширина</param>
    /// <param name="parHeight">Высота</param>
    public ScreenResolutionOpenTk(int parWidth, int parHeight)
    {
      Width = parWidth;
      Height = parHeight;
    }

    /// <summary>
    /// Ширина в пикселях
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Высота в пикселях
    /// </summary>
    public int Height { get; }
  }
}