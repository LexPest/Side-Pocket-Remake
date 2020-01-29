using System.Drawing;
using ViewOpenTK.OpenGL;

namespace ViewOpenTK.AssetData.DataTypes.Subassets
{
  /// <summary>
  /// Производный ассет - спрайт
  /// </summary>
  public sealed class SubassetDataSprite : SubassetData
  {
    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parLinkedAssetDataTexture">Данные об ассете текстуры OpenTK</param>
    /// <param name="parSpriteRect">Область спрайта на текстуре</param>
    public SubassetDataSprite(AssetDataOpenTkTexture parLinkedAssetDataTexture, Rectangle parSpriteRect)
    {
      LinkedAssetDataTexture = parLinkedAssetDataTexture;
      SpriteRect = parSpriteRect;

      OpenTkTextureRect = new OpenGlTexRect(SpriteRect, LinkedAssetDataTexture);
    }

    /// <summary>
    /// Данные об ассете текстуры OpenTK
    /// </summary>
    public AssetDataOpenTkTexture LinkedAssetDataTexture { get; private set; }

    /// <summary>
    /// Прямоуголная область спрайта на текстуре
    /// </summary>
    public Rectangle SpriteRect { get; private set; }

    /// <summary>
    /// Прямоуголная область спрайта на текстуре в координатах OpenGL
    /// </summary>
    public OpenGlTexRect OpenTkTextureRect { get; private set; }

    /// <summary>
    /// Ширина спрайта
    /// </summary>
    public int Width => SpriteRect.Width;
    
    /// <summary>
    /// Высота спрайта
    /// </summary>
    public int Height => SpriteRect.Height;
  }
}