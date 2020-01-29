using CoreUtil.ResourceManager;
using CoreUtil.ResourceManager.AssetData.DataTypes;

namespace ViewOpenTK.AssetData.DataTypes
{
  /// <summary>
  /// Базовый абстрактный класс для всех данных ассетов OpenTK
  /// </summary>
  public abstract class AssetDataOpenTkParent : AssetDataParent
  {
    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parActualAssetMetadata">Метаданные об ассете</param>
    protected AssetDataOpenTkParent(AssetMetadata parActualAssetMetadata) : base(parActualAssetMetadata)
    {
    }
  }
}