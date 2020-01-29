using CoreUtil.ResourceManager;

namespace Model.SPCore.Managers.Sound.Data
{
  /// <summary>
  /// Аудио ресурс приложения
  /// </summary>
  public class AppSoundAsset
  {
    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parLinkedAssetMetadata">Связанные метаданные ассета</param>
    public AppSoundAsset(AssetMetadata parLinkedAssetMetadata)
    {
      LinkedAssetMetadata = parLinkedAssetMetadata;
    }

    /// <summary>
    /// Связанные метаданные ассета
    /// </summary>
    public AssetMetadata LinkedAssetMetadata { get; private set; }
  }
}