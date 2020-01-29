using System;

namespace CoreUtil.ResourceManager.AssetData.DataTypes
{
  /// <summary>
  /// Родительский класс для всех классов, представляющих различные типы игровых ресурсов
  /// </summary>
  public abstract class AssetDataParent : IDisposable
  {
    /// <summary>
    /// Флаг-признак уничтожения
    /// </summary>
    protected bool IsDisposed = false;

    /// <summary>
    /// Связанные метаданные ассета
    /// </summary>
    public AssetMetadata ActualAssetMetadata { get; protected set; }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="parActualAssetMetadata">Связанные метаданные ассета</param>
    protected AssetDataParent(AssetMetadata parActualAssetMetadata)
    {
      ActualAssetMetadata = parActualAssetMetadata;
    }

    /// <summary>
    /// Деструктор
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Обработчик освобождения занятой ресурсом памяти
    /// </summary>
    /// <param name="parDisposing"></param>
    protected virtual void Dispose(bool parDisposing)
    {
      if (IsDisposed)
      {
        return;
      }

      IsDisposed = true;
    }
  }
}