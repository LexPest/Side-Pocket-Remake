using CoreUtil.ResourceManager.AssetData.DataTypes;

namespace CoreUtil.ResourceManager.AssetData.Builders
{
  /// <summary>
  /// Абстрактный класс для "строителей"-загрузчиков игровых ресурсов
  /// </summary>
  public abstract class AssetDataAbstractBuilder
  {
    /// <summary>
    /// Загрузить и подготовить данные игрового ресурса
    /// </summary>
    /// <param name="parAssetMetadata">Метаданные ресурса</param>
    /// <typeparam name="T">Тип ресурса</typeparam>
    /// <returns>Обработчик данных игрового ресурса</returns>
    public abstract T LoadAssetData<T>(AssetMetadata parAssetMetadata) where T : AssetDataParent;
  }
}