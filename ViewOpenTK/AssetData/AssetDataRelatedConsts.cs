using System.Collections.Generic;

namespace ViewOpenTK.AssetData
{
  /// <summary>
  /// Константы, относящиеся к ассетам/ресурсам
  /// </summary>
  public static class AssetDataRelatedConsts
  {
    /// <summary>
    /// Префикс обозначения символа шрифтового ассета
    /// </summary>
    public const string FONT_ASSET_SYMBOL_DEFINITION_PREFIX = "font_letter ";

    /// <summary>
    /// Словарь соответствия расширений файлов и типам ассетов OpenTK
    /// </summary>
    public static readonly Dictionary<string, EOpenTkAssetType> AssetFileExtensionToOpenTkAssetType =
      new Dictionary<string, EOpenTkAssetType>()
      {
        {
          ".png", EOpenTkAssetType.Texture
        },
        {
          ".wav", EOpenTkAssetType.WaveSound
        },
        // MP3 is deprecated!
        {
          ".ogg", EOpenTkAssetType.WaveSound
        },
        {
          ".json", EOpenTkAssetType.TextMetadata
        }
      };
  }
}