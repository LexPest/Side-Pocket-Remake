using System.Drawing;
using ViewOpenTK.AssetData.DataTypes.Subassets;
using ViewOpenTK.OpenGL;
using ViewOpenTK.SPCore.ViewProvider.InternalGraphicalDataStructures;

namespace ViewOpenTK.SPCore.ViewProvider.RenderingData
{
  /// <summary>
  /// Данные о спрайте для рендеринга
  /// </summary>
  public struct RenderingSprite
  {
    /// <summary>
    /// Глубина
    /// </summary>
    public double Depth { get; private set; }
    /// <summary>
    /// Производный ассет спрайта
    /// </summary>
    public SubassetDataSprite Sprite { get; private set; }

    /// <summary>
    /// Координата X
    /// </summary>
    public double X { get; private set; }
    /// <summary>
    /// Координата Y
    /// </summary>
    public double Y { get; private set; }

    /// <summary>
    /// Поворот в градусах
    /// </summary>
    public double RotationDegrees { get; private set; }

    /// <summary>
    /// Цвет
    /// </summary>
    public Color BlendColor { get; private set; }

    /// <summary>
    /// Масштабирование по X
    /// </summary>
    public double ScaleX { get; private set; }
    /// <summary>
    /// Масштабирование по Y
    /// </summary>
    public double ScaleY { get; private set; }

    /// <summary>
    /// Точка опоры для поворота, координата X
    /// </summary>
    public double PivotX { get; private set; }
    /// <summary>
    /// Точка опоры для поворота, координата Y
    /// </summary>
    public double PivotY { get; private set; }

    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parSprite">Производный ассет спрайта</param>
    /// <param name="parX">Координата X</param>
    /// <param name="parY">Координата Y</param>
    /// <param name="parRotationDegrees">Поворот в градусах</param>
    /// <param name="parBlendColor">Цвет</param>
    /// <param name="parDepth">Глубина</param>
    /// <param name="parScaleX">Масштабирование по X</param>
    /// <param name="parScaleY">Масштабирование по Y</param>
    /// <param name="parHAlign">Горизонтальное выравнивание</param>
    /// <param name="parVAlign">Вертикальное выравнивание</param>
    /// <param name="parRotationPivotX">Точка опоры для поворота, координата X</param>
    /// <param name="parRotationPivotY">Точка опоры для поворота, координата Y</param>
    public RenderingSprite(SubassetDataSprite parSprite, double parX, double parY, double parRotationDegrees, Color parBlendColor,
      double parDepth,
      double parScaleX = 1.0, double parScaleY = 1.0, EHorizontalAlign parHAlign = EHorizontalAlign.Left,
      EVerticalAlign parVAlign = EVerticalAlign.Top, double parRotationPivotX = 0, double parRotationPivotY = 0)
    {
      Sprite = parSprite;

      X = OpenGlUtil.GetLeftXCoordBasedOnHorizontalAlign(parX, parSprite.Width, parHAlign, parScaleX);
      Y = OpenGlUtil.GetTopYCoordBasedOnVerticalAlign(parY, parSprite.Height, parVAlign, parScaleY);

      Depth = parDepth;
      RotationDegrees = parRotationDegrees;
      BlendColor = parBlendColor;
      ScaleX = parScaleX;
      ScaleY = parScaleY;
      PivotX = parRotationPivotX;
      PivotY = parRotationPivotY;
    }
  }
}