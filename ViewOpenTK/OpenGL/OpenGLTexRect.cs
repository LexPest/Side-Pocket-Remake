using System.Drawing;
using ViewOpenTK.AssetData.DataTypes;

namespace ViewOpenTK.OpenGL
{
  /// <summary>
  /// Данные о текстурных координатах OpenGL
  /// </summary>
  public class OpenGlTexRect
  {
    /// <summary>
    /// Стандартный конструктор, расчитывающий текстурные координаты
    /// </summary>
    /// <param name="parRect">Прямоугольник в обычной системе координат</param>
    /// <param name="parTex">Ассет текстуры</param>
    public OpenGlTexRect(Rectangle parRect, AssetDataOpenTkTexture parTex)
    {
      X1 = 1.0 / parTex.Width * parRect.X;
      Y1 = 1.0 / parTex.Height * parRect.Y;
      X2 = X1 + 1.0 / parTex.Width * parRect.Width;
      Y2 = Y1 + 1.0 / parTex.Height * parRect.Height;
    }

    /// <summary>
    /// Нормализованная текстурная координата OpenGL X1
    /// </summary>
    public double X1 { get; set; }
    /// <summary>
    /// Нормализованная текстурная координата OpenGL Y1
    /// </summary>
    public double Y1 { get; set; }

    /// <summary>
    /// Нормализованная текстурная координата OpenGL X2
    /// </summary>
    public double X2 { get; set; }
    /// <summary>
    /// Нормализованная текстурная координата OpenGL Y2
    /// </summary>
    public double Y2 { get; set; }
  }
}