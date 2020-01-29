using System.Collections.Generic;
using System.Drawing;
using ViewOpenTK.AssetData.DataTypes.Subassets;
using ViewOpenTK.OpenGL;
using ViewOpenTK.SPCore.ViewProvider.InternalGraphicalDataStructures;

namespace ViewOpenTK.SPCore.ViewProvider.RenderingData
{
  /// <summary>
  /// Данные о текстовой строке для рендеринга
  /// </summary>
  public struct RenderingString
  {
    /// <summary>
    /// Спрайты из шрифта для рендеринга
    /// </summary>
    public LinkedList<RenderingSprite> SpritesToRender { get; private set; }

    /// <summary>
    /// Глубина
    /// </summary>
    public double Depth { get; private set; }

    /// <summary>
    /// Координата X
    /// </summary>
    public double X { get; private set; }
    /// <summary>
    /// Координата Y
    /// </summary>
    public double Y { get; private set; }

    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parDepth">Глубина</param>
    /// <param name="parX">Координата X</param>
    /// <param name="parY">Координата Y</param>
    /// <param name="parStringToRender">Текстовая строка</param>
    /// <param name="parFont">Шрифт</param>
    /// <param name="parColor">Цвет</param>
    /// <param name="parScaleX">Масштабирование по X</param>
    /// <param name="parScaleY">Масштабирование по Y</param>
    /// <param name="parHAlign">Горизонтальное выравнивание</param>
    /// <param name="parVAlign">Вертикальное выравнивание</param>
    public RenderingString(double parDepth, double parX, double parY, string parStringToRender, SubassetDataFont parFont,
      Color parColor, double parScaleX = 1.0, double parScaleY = 1.0, EHorizontalAlign parHAlign = EHorizontalAlign.Left,
      EVerticalAlign parVAlign = EVerticalAlign.Top)
    {
      LinkedList<SubassetDataSprite> usedSprites = parFont.GetSymbolsSprites(parStringToRender);
      SpritesToRender = new LinkedList<RenderingSprite>();

      double totalWidth = 0;
      double totalHeight = 0;
      foreach (var usedSprite in usedSprites)
      {
        totalWidth += usedSprite.Width;
        if (usedSprite.Height > totalHeight)
        {
          totalHeight = usedSprite.Height;
        }
      }

      X = OpenGlUtil.GetLeftXCoordBasedOnHorizontalAlign(parX, totalWidth, parHAlign, parScaleX);
      Y = OpenGlUtil.GetTopYCoordBasedOnVerticalAlign(parY, totalHeight, parVAlign, parScaleY);

      totalWidth = 0;
      //totalHeight = 0;
      foreach (var usedSprite in usedSprites)
      {
        SpritesToRender.AddLast(new RenderingSprite(usedSprite, X + totalWidth, Y, 0, parColor,
          parDepth, parScaleX, parScaleY, EHorizontalAlign.Left, EVerticalAlign.Top));

        totalWidth += usedSprite.Width * parScaleX;
      }

      Depth = parDepth;
    }
  }
}