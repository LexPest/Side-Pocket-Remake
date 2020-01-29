using System.Collections.Generic;
using CoreUtil.ResourceManager;

namespace ViewOpenTK.AssetData.DataTypes.Subassets.Strategies
{
  /// <summary>
  /// Интерфейс для стратегии обновления внутренней библиотеки ресурсов OpenTK
  /// </summary>
  public interface IUpdateCollectionLibraryStrategy
  {
    /// <summary>
    /// Контрактный метод обновления коллекций ресурсов библиотеки
    /// </summary>
    /// <param name="parResManager">Менеджер ресурсов приложения</param>
    /// <param name="outDataSprites">Выходная коллекция спрайтов</param>
    /// <param name="outDataAnimations">Выходная коллекция анимаций</param>
    /// <param name="outDataFonts">Выходная коллекция шрифтов</param>
    void UpdateCollectionLibrary(ResourceManager parResManager,
      out Dictionary<string, SubassetDataSprite> outDataSprites,
      out Dictionary<string, SubassetDataAnimation> outDataAnimations,
      out Dictionary<string, SubassetDataFont> outDataFonts);
  }
}