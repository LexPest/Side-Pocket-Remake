using System.Drawing;

namespace ViewOpenTK.SPCore.ViewProvider.ViewComponents.Binds.Game
{
  /// <summary>
  /// Данные для рендеринга бильярдного шара
  /// </summary>
  public class BallGraphicsBind
  {
    /// <summary>
    /// Цвет
    /// </summary>
    public Color BlendColor;
    /// <summary>
    /// Имя спрайта шара
    /// </summary>
    public string StillSpriteKey;

    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parBlendColor">Цвет</param>
    /// <param name="parStillSpriteKey">Имя спрайта шара</param>
    public BallGraphicsBind(Color parBlendColor, string parStillSpriteKey)
    {
      BlendColor = parBlendColor;
      StillSpriteKey = parStillSpriteKey;
    }
  }
}