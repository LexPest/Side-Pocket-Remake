using System.Collections.Generic;

namespace ViewOpenTK.AssetData.DataTypes.Subassets
{
  /// <summary>
  /// Производный ассет - анимация
  /// </summary>
  public class SubassetDataAnimation : SubassetData
  {
    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parSpriteFramesInAnimation">Данные о кадрах в анимации</param>
    public SubassetDataAnimation(List<SubassetDataSprite> parSpriteFramesInAnimation)
    {
      SpriteFramesInAnimation = parSpriteFramesInAnimation;
    }

    /// <summary>
    /// Данные о кадрах в анимации
    /// </summary>
    public List<SubassetDataSprite> SpriteFramesInAnimation { get; private set; }

    /// <summary>
    /// Получить общее количество кадров анимации
    /// </summary>
    public int FramesCount => SpriteFramesInAnimation.Count;
  }
}