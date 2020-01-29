using System;
using ViewOpenTK.SPCore.ViewProvider.InternalGraphicalDataStructures;

namespace ViewOpenTK.OpenGL
{
  /// <summary>
  /// Утилитарный вспомогательный класс OpenGL
  /// </summary>
  public static class OpenGlUtil
  {
    /// <summary>
    /// Получить координату X в пространстве OpenGL
    /// </summary>
    /// <param name="parX">X в обычной системе координат</param>
    /// <param name="parWidth">Ширина</param>
    /// <returns></returns>
    public static double GetOpenGlCoordX(double parX, double parWidth)
    {
      return (2.0 * parX) / parWidth - 1.0;
    }

    /// <summary>
    /// Получить координату Y в пространстве OpenGL
    /// </summary>
    /// <param name="parY">Y в обычной системе координат</param>
    /// <param name="parHeight">Высота</param>
    /// <returns></returns>
    public static double GetOpenGlCoordY(double parY, double parHeight)
    {
      return 1.0 - (2.0 * parY) / parHeight;
    }

    /// <summary>
    /// Получить нормализованное значение величины ширины OpenGL
    /// </summary>
    /// <param name="parWidth">Величина ширины в обычной системе координат</param>
    /// <param name="parViewportWidth">Ширина вида</param>
    /// <returns></returns>
    public static double GetNormalizedWidthCoordValue(double parWidth, double parViewportWidth)
    {
      return GetOpenGlCoordX(parWidth, parViewportWidth) - (-1.0);
    }

    /// <summary>
    /// Получить нормализованное значение величины высоты OpenGL
    /// </summary>
    /// <param name="parWidth">Величина высоты в обычной системе координат</param>
    /// <param name="parViewportWidth">Высота вида</param>
    /// <returns></returns>
    public static double GetNormalizedHeightCoordValue(double parHeight, double parViewportHeight)
    {
      return GetOpenGlCoordY(parViewportHeight - parHeight, parViewportHeight) - (-1.0);
    }

    /// <summary>
    /// Получить координату X в пространстве текстуры OpenGL
    /// </summary>
    /// <param name="parX">X в обычной системе текстурных координат</param>
    /// <param name="parWidth">Ширина</param>
    /// <returns></returns>
    public static double GetNormalizedTextureCoordX(double parX, double parWidth)
    {
      return parX / parWidth;
    }

    /// <summary>
    /// Получить координату Y в пространстве текстуры OpenGL
    /// </summary>
    /// <param name="parY">Y в обычной системе текстурных координат</param>
    /// <param name="parHeight">Ширина</param>
    /// <returns></returns>
    public static double GetNormalizedTextureCoordY(double parY, double parHeight)
    {
      return parY / parHeight;
    }

    /// <summary>
    /// Получить координату X в пространстве OpenGL с учетом выбранного горизонтального выравнивания
    /// </summary>
    /// <param name="parX">X в обычной системе координат</param>
    /// <param name="parWidth">Ширина</param>
    /// <param name="parHAlign">Выбранное горизонтальное выравнивание</param>
    /// <param name="parScale">Величина масштабирования</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">Неподдерживаемый формат выравнивания</exception>
    public static double GetLeftXCoordBasedOnHorizontalAlign(double parX, double parWidth, EHorizontalAlign parHAlign,
      double parScale)
    {
      switch (parHAlign)
      {
        case EHorizontalAlign.Left:
          return parX;
          break;
        case EHorizontalAlign.Middle:
          return parX - (parWidth * parScale / 2);
          break;
        case EHorizontalAlign.Right:
          return parX - (parWidth * parScale);
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(parHAlign), parHAlign, null);
      }
    }

    /// <summary>
    /// Получить координату Y в пространстве OpenGL с учетом выбранного вертикального выравнивания
    /// </summary>
    /// <param name="parY">Y в обычной системе координат</param>
    /// <param name="parHeight">Высота</param>
    /// <param name="parVAlign">Выбранное вертикальное выравнивание</param>
    /// <param name="parScale">Величина масштабирования</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">Неподдерживаемый формат выравнивания</exception>
    public static double GetTopYCoordBasedOnVerticalAlign(double parY, double parHeight, EVerticalAlign parVAlign,
      double parScale)
    {
      switch (parVAlign)
      {
        case EVerticalAlign.Top:
          return parY;
          break;
        case EVerticalAlign.Middle:
          return parY - (parHeight * parScale / 2);
          break;
        case EVerticalAlign.Bottom:
          return parY - (parHeight * parScale);
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(parVAlign), parVAlign, null);
      }
    }
  }
}