using System;
using System.Collections.Generic;
using CoreUtil.ResourceManager.AssetData.DataTypes;

namespace CoreUtil.ResourceManager
{
  /// <summary>
  /// Данные о загруженном в память пакете ресурсов
  /// </summary>
  public class AssetPackLoadedData : IDisposable
  {
    /// <summary>
    /// Флаг-признак уничтожения
    /// </summary>
    protected bool IsDisposed = false;

    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parContent">Содержимое пакета ресурсов</param>
    public AssetPackLoadedData(Dictionary<string, AssetDataParent> parContent)
    {
      Content = parContent;
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

      if (parDisposing)
      {
        foreach (var assetDataParent in Content)
        {
          assetDataParent.Value.Dispose();
        }
      }

      IsDisposed = true;
    }

    /// <summary>
    /// Содержимое пакета ресурсов
    /// </summary>
    public Dictionary<string, AssetDataParent> Content { get; private set; }
  }
}