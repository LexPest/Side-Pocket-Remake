using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using CoreUtil.ResourceManager;
using OpenTK.Graphics.OpenGL;
using ViewOpenTK.OpenGL;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace ViewOpenTK.AssetData.DataTypes
{
  /// <summary>
  /// Данные ассета типа текстуры OpenTK
  /// </summary>
  public class AssetDataOpenTkTexture : AssetDataOpenTkParent
  {
    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parActualAssetMetadata">Метаданные об ассете</param>
    /// <param name="parBinaryData">Данные текстуры в бинарном виде</param>
    public AssetDataOpenTkTexture(AssetMetadata parActualAssetMetadata, byte[] parBinaryData) : base(
      parActualAssetMetadata)
    {
      OpenGlCommandsInternalHandler.AddGlCommand(() =>
      {
        using (MemoryStream ms = new MemoryStream(parBinaryData))
        {
          LinkedBitmap = new Bitmap(ms);
        }

        GlTextureId = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, GlTextureId);

        BitmapData data = LinkedBitmap.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadOnly,
          PixelFormat.Format32bppArgb);

        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
          OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

        LinkedBitmap.UnlockBits(data);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS,
          (int) TextureWrapMode.Clamp);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,
          (int) TextureWrapMode.Clamp);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
          (int) TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
          (int) TextureMinFilter.Linear);

        Console.WriteLine($"GL: Texture {GlTextureId} created in memory");
      });
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
        OpenGlCommandsInternalHandler.AddGlCommand(() =>
        {
          // освобождаем ресурсы
          // отменяем привязку текстуры и уничтожаем bitmap
          GL.BindTexture(TextureTarget.Texture2D, 0);
          GL.DeleteTexture(GlTextureId);
          LinkedBitmap.Dispose();

          Console.WriteLine($"GL: Texture {GlTextureId} removed from memory");
        });
      }

      IsDisposed = true;
    }

    /// <summary>
    /// Идентификатор текстуры в памяти OpenGL
    /// </summary>
    public int GlTextureId { get; private set; }

    /// <summary>
    /// Связанный сгенерированный Bitmap
    /// </summary>
    public Bitmap LinkedBitmap { get; private set; }

    /// <summary>
    /// Ширина текстуры
    /// </summary>
    public int Width => LinkedBitmap.Width;

    /// <summary>
    /// Высота текстуры
    /// </summary>
    public int Height => LinkedBitmap.Height;
  }
}