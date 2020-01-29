namespace CoreUtil.ResourceManager.AssetData.DataTypes
{
  /// <summary>
  /// Игровой ресурс, содержащий бинарные данные
  /// </summary>
  public class AssetDataBinary : AssetDataParent
  {
    /// <summary>
    /// Конструктор ресурса
    /// </summary>
    /// <param name="parAssetMetadata">Метаданные ассета</param>
    /// <param name="parBinaryData">Бинарные данные</param>
    public AssetDataBinary(AssetMetadata parAssetMetadata, byte[] parBinaryData) : base(parAssetMetadata)
    {
      BinaryData = parBinaryData;
    }

    /// <summary>
    /// Бинарные данные
    /// </summary>
    public byte[] BinaryData { get; protected set; }
  }
}