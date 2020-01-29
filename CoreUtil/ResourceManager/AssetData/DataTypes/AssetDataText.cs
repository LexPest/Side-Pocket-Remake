namespace CoreUtil.ResourceManager.AssetData.DataTypes
{
  /// <summary>
  /// Игровой ресурс с текстовыми данными
  /// </summary>
  public class AssetDataText : AssetDataParent
  {
    /// <summary>
    /// Конструктор ресурса
    /// </summary>
    /// <param name="parAssetMetadata">Метаданные ассета</param>
    /// <param name="parTextData">Текстовые данные</param>
    public AssetDataText(AssetMetadata parAssetMetadata, string[] parTextData) : base(parAssetMetadata)
    {
      TextData = parTextData;
    }

    /// <summary>
    /// Текстовые данные
    /// </summary>
    public string[] TextData { get; private set; }
  }
}