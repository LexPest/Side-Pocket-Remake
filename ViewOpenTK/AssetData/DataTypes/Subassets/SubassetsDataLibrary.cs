using System.Collections.Generic;
using CoreUtil.ResourceManager;
using ViewOpenTK.AssetData.DataTypes.Subassets.Strategies;

namespace ViewOpenTK.AssetData.DataTypes.Subassets
{
  /// <summary>
  /// Библиотека производных ассетов OpenTK
  /// </summary>
  public class SubassetsDataLibrary
  {
    /// <summary>
    /// Стандартный конструктор без параметров
    /// </summary>
    public SubassetsDataLibrary()
    {
    }

    /// <summary>
    /// Обновить коллекции библиотеки
    /// </summary>
    /// <param name="parUpdateCollectionLibraryStrategy">Реализация стратегии обновления коллекции</param>
    /// <param name="parResManager">Менеджер ресурсов</param>
    public void UpdateCollectionsData(IUpdateCollectionLibraryStrategy parUpdateCollectionLibraryStrategy,
      ResourceManager parResManager)
    {
      parUpdateCollectionLibraryStrategy.UpdateCollectionLibrary(parResManager,
        out Dictionary<string, SubassetDataSprite> spritesCollection,
        out Dictionary<string, SubassetDataAnimation> animationsCollection,
        out Dictionary<string, SubassetDataFont> fontsCollection);
      SpritesCollection = spritesCollection;
      AnimationsCollection = animationsCollection;
      FontsCollection = fontsCollection;
    }

    /// <summary>
    /// Получить спрайт из коллекции
    /// </summary>
    /// <param name="parKey">Идентификатор-название спрайта</param>
    /// <returns></returns>
    public SubassetDataSprite GetSprite(string parKey)
    {
      return SpritesCollection.TryGetValue(parKey, out SubassetDataSprite retSprite) ? retSprite : null;
    }

    /// <summary>
    /// Получить анимацию из коллекции
    /// </summary>
    /// <param name="parKey">Идентификатор-название анимации</param>
    /// <returns></returns>
    public SubassetDataAnimation GetAnimation(string parKey)
    {
      return AnimationsCollection.TryGetValue(parKey, out SubassetDataAnimation retAnim) ? retAnim : null;
    }

    /// <summary>
    /// Получить шрифт из коллекции
    /// </summary>
    /// <param name="parKey">Идентификатор-название шрифта</param>
    /// <returns></returns>
    public SubassetDataFont GetFont(string parKey)
    {
      return FontsCollection.TryGetValue(parKey, out SubassetDataFont retFont) ? retFont : null;
    }

    /// <summary>
    /// Коллекция спрайтов
    /// </summary>
    public Dictionary<string, SubassetDataSprite> SpritesCollection { get; private set; } =
      new Dictionary<string, SubassetDataSprite>();

    /// <summary>
    /// Коллекция анимаций
    /// </summary>
    public Dictionary<string, SubassetDataAnimation> AnimationsCollection { get; private set; } =
      new Dictionary<string, SubassetDataAnimation>();

    /// <summary>
    /// Коллекция шрифтов
    /// </summary>
    public Dictionary<string, SubassetDataFont> FontsCollection { get; private set; } =
      new Dictionary<string, SubassetDataFont>();
  }
}