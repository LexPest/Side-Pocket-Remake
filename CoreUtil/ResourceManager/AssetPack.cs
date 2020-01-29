using System.Collections.Generic;

namespace CoreUtil.ResourceManager
{
  /// <summary>
  /// Пакет ресурсов
  /// </summary>
  public sealed class AssetPack
  {
    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parContent">Содержимое пакета ресурсов</param>
    public AssetPack(Dictionary<string, AssetMetadata> parContent)
    {
      Content = parContent;
    }

    /// <summary>
    /// Содержимое пакета ресурсов
    /// </summary>
    public Dictionary<string, AssetMetadata> Content { get; private set; }
  }
}