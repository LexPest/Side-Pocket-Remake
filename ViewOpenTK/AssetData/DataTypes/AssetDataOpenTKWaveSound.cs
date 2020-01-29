using System.IO;
using ViewOpenTK.AssetData.DataTypes.AudioFormatProviders;
using ViewOpenTK.OpenAL.BasicManager;

namespace ViewOpenTK.AssetData.DataTypes
{
  /// <summary>
  /// Данные ассета аудио типа OpenTK
  /// </summary>
  public class AssetDataOpenTkWaveSound : AssetDataOpenTkParent
  {
    /// <summary>
    /// Реализация интерфейса для поддержки формата аудио файла
    /// </summary>
    public IAudioFormatProvider AudioFormatProvider;

    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parAssetMetadata">Метаданные об ассете</param>
    /// <param name="parBinaryData">Аудио данные в бинарном формате</param>
    public AssetDataOpenTkWaveSound(AssetMetadataOpenTk parAssetMetadata, byte[] parBinaryData) : base(parAssetMetadata)
    {
      OpenAlManager.GetInstance();

      if (Path.GetExtension(parAssetMetadata.FilePath) == ".ogg")
      {
        AudioFormatProvider = new AudioFormatProviderOgg();
      }
      else
      {
        AudioFormatProvider = new AudioFormatProviderWav(parBinaryData);
      }

      AudioFormatProvider.InitAudioData(this);
    }

    /// <summary>
    /// Переопределение деструктора
    /// </summary>
    /// <param name="parDisposing">Флаг-признак уничтожения (формальный параметр)</param>
    protected override void Dispose(bool parDisposing)
    {
      if (IsDisposed)
      {
        return;
      }

      if (parDisposing)
      {
        // освобождаем ресурсы
        AudioFormatProvider.Dispose();
      }

      IsDisposed = true;
    }
  }
}