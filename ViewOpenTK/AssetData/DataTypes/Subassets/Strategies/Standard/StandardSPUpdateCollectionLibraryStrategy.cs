using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using CoreUtil.APIExtensions;
using CoreUtil.ResourceManager;
using CoreUtil.ResourceManager.AssetData.DataTypes;
using Newtonsoft.Json;
using ViewOpenTK.OpenGL;
using ViewOpenTK.SPCore.ViewProvider;

namespace ViewOpenTK.AssetData.DataTypes.Subassets.Strategies.Standard
{
  /// <summary>
  /// Стандартная реализация стратегии обновления внутренней библиотеки ресурсов OpenTK
  /// </summary>
  public class StandardSpUpdateCollectionLibraryStrategy : IUpdateCollectionLibraryStrategy
  {
    /// <summary>
    /// Внутренняя структура данных с шаблоном информации о спрайте в спрайтовом атласе
    /// </summary>
    [Serializable]
    private struct SpriteSheetInfo
    {
      /// <summary>
      /// Имя спрайта
      /// </summary>
      [JsonProperty(SPRITESHEET_SPRITE_NAME)]
      public string SpriteName;

      /// <summary>
      /// Позиция X в атласе
      /// </summary>
      [JsonProperty(SPRITESHEET_X)] public int X;

      /// <summary>
      /// Позиция Y в атласе
      /// </summary>
      [JsonProperty(SPRITESHEET_Y)] public int Y;

      /// <summary>
      /// Ширина в атласе
      /// </summary>
      [JsonProperty(SPRITESHEET_WIDTH)] public int Width;

      /// <summary>
      /// Высота в атласе
      /// </summary>
      [JsonProperty(SPRITESHEET_HEIGHT)] public int Height;
    }

    /// <summary>
    /// Внутренняя структура данных с шаблоном представления спрайтового атласа 
    /// </summary>
    [Serializable]
    private struct SpriteSheetDataTuple
    {
      /// <summary>
      /// Метаданные атласа
      /// </summary>
      [JsonProperty(METADATATYPE_KEY)] public string Metadata;

      /// <summary>
      /// Спрайты в атласе
      /// </summary>
      [JsonProperty(SPRITESHEET_ARRAY)] public SpriteSheetInfo[] Sprites;
    }

    /// <summary>
    /// Ключ значения метаданных (ключ)
    /// </summary>
    private const string METADATATYPE_KEY = "metadata_type";

    /// <summary>
    /// Метаданные типа атласа спрайтовой анимации (ключ)
    /// </summary>
    private const string METADATATYPE_ANIM_SHEET = "anim_sheet";

    /// <summary>
    /// Метаданные типа спрайтового атласа (ключ)
    /// </summary>
    private const string METADATATYPE_SPRITE_SHEET = "sprite_sheet";

    /// <summary>
    /// Метаданные типа шрифтового спрайтового атласа (ключ)
    /// </summary>
    private const string METADATATYPE_FONT_SPRITE_SHEET = "font_sprite_sheet";

    /// <summary>
    /// Метаданные ширины кадра анимации (ключ)
    /// </summary>
    private const string METADATA_ANIM_WIDTH = "width";

    /// <summary>
    /// Метаданные высоты кадра анимации (ключ)
    /// </summary>
    private const string METADATA_ANIM_HEIGHT = "height";

    /// <summary>
    /// Метаданные об общем числе кадров анимации (ключ)
    /// </summary>
    private const string METADATA_ANIM_FRAMESCOUNT = "frames_count";

    /// <summary>
    /// Постфикс для имен внутренних ресурсов, связанных со спрайтовыми анимациями (ключ)
    /// </summary>
    private const string ANIMATION_POSTFIX = "_anim_";

    /// <summary>
    /// Метаданные о спрайтах в атласе (ключ)
    /// </summary>
    private const string SPRITESHEET_ARRAY = "sprites_info";

    /// <summary>
    /// Метаданные об имени спрайта (ключ)
    /// </summary>
    private const string SPRITESHEET_SPRITE_NAME = "sprite_name";

    /// <summary>
    /// Метаданные о позиции спрайта в атласе X (ключ)
    /// </summary>
    private const string SPRITESHEET_X = "x";

    /// <summary>
    /// Метаданные о позиции спрайта в атласе Y (ключ)
    /// </summary>
    private const string SPRITESHEET_Y = "y";

    /// <summary>
    /// Метаданные о ширине спрайта в атласе (ключ)
    /// </summary>
    private const string SPRITESHEET_WIDTH = "width";

    /// <summary>
    /// Метаданные о высоте спрайта в атласе (ключ)
    /// </summary>
    private const string SPRITESHEET_HEIGHT = "height";

    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parAssetPackName">Имя пакета ресурсов для обработки</param>
    public StandardSpUpdateCollectionLibraryStrategy(string parAssetPackName)
    {
      AssetPackName = parAssetPackName;
    }

    /// <summary>
    /// Реализация метода обновления коллекций ресурсов библиотеки
    /// </summary>
    /// <param name="parResManager">Менеджер ресурсов приложения</param>
    /// <param name="outDataSprites">Выходная коллекция спрайтов</param>
    /// <param name="outDataAnimations">Выходная коллекция анимаций</param>
    /// <param name="outDataFonts">Выходная коллекция шрифтов</param>
    public void UpdateCollectionLibrary(ResourceManager parResManager,
      out Dictionary<string, SubassetDataSprite> outDataSprites,
      out Dictionary<string, SubassetDataAnimation> outDataAnimations,
      out Dictionary<string, SubassetDataFont> outDataFonts)
    {
      outDataSprites = new Dictionary<string, SubassetDataSprite>();
      outDataAnimations = new Dictionary<string, SubassetDataAnimation>();
      outDataFonts = new Dictionary<string, SubassetDataFont>();

      var loadedContent = parResManager.GetLoadedAssetPackContent(AssetPackName);

      while (!OpenGlCommandsInternalHandler.AreAllActionsPerformed())
      {
      }

      foreach (var assetContentElementKeyValuePair in loadedContent)
      {
        Console.WriteLine($"Processing {assetContentElementKeyValuePair.Value.ActualAssetMetadata.FilePath}");
        if (assetContentElementKeyValuePair.Value is AssetDataOpenTkTexture workingWithTexture)
        {
          Console.WriteLine("Texture detected!");
          string textureName = Path.GetFileName(workingWithTexture.ActualAssetMetadata.FilePath);

          parResManager.GetAssetInfo(workingWithTexture.ActualAssetMetadata, out string assetPack,
            out string assetName);
          string possibleMetadataAssetName = $"{assetName}.meta.json";

          Console.WriteLine($"Checking for metadata {possibleMetadataAssetName}");

          //проверим, что имеются метаданные
          if (loadedContent.ContainsKey(
            possibleMetadataAssetName))
          {
            AssetDataText metadataAsset = (AssetDataText) loadedContent[possibleMetadataAssetName];
            string jsonFullText = string.Join(" ",
              metadataAsset.TextData);
            Dictionary<string, object> jsonData =
              JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonFullText);

            string metadataType = (string) jsonData[METADATATYPE_KEY];

            if (metadataType == METADATATYPE_ANIM_SHEET)
            {
              //поделим текстуру на спрайты и создадим анимации
              int width = (int) (long) jsonData[METADATA_ANIM_WIDTH];
              int height = (int) (long) jsonData[METADATA_ANIM_HEIGHT];

              Dictionary<string, SubassetDataSprite> spritesInAnim =
                new Dictionary<string, SubassetDataSprite>();

              // TODO be carefull with x'es and y'es

              int currentX = 1;
              int currentY = 1;

              int animCounter = 0;

              int framesCountConstraint = int.MaxValue;

              if (jsonData.ContainsKey(METADATA_ANIM_FRAMESCOUNT))
              {
                framesCountConstraint = (int) (long) jsonData[METADATA_ANIM_FRAMESCOUNT];
              }

              while (currentY < workingWithTexture.Height && animCounter < framesCountConstraint)
              {
                currentX = 1;
                while (currentX < workingWithTexture.Width && animCounter < framesCountConstraint)
                {
                  spritesInAnim.Add($"{assetName}{ANIMATION_POSTFIX}{animCounter}",
                    new SubassetDataSprite(workingWithTexture,
                      new Rectangle(currentX - 1, currentY - 1, width, height)));

                  animCounter++;
                  currentX += width;
                }

                currentY += height;
              }

              Console.WriteLine(
                $"Animation sprite sheet processed, name {assetName} frames {animCounter}");

              //создание анимации и объединение спрайтов
              SubassetDataAnimation animation =
                new SubassetDataAnimation(new List<SubassetDataSprite>(spritesInAnim.Values));

              outDataAnimations.Add(assetName, animation);
              outDataSprites.AddRange(spritesInAnim);
            }
            else if (metadataType == METADATATYPE_SPRITE_SHEET)
            {
              SpriteSheetDataTuple deserializedActualJsonData =
                JsonConvert.DeserializeObject<SpriteSheetDataTuple>(jsonFullText);

              foreach (var sprite in deserializedActualJsonData.Sprites)
              {
                outDataSprites.Add($"{assetName}/{sprite.SpriteName}",
                  new SubassetDataSprite(workingWithTexture,
                    new Rectangle(sprite.X - 1, sprite.Y - 1, sprite.Width, sprite.Height)));
                Console.WriteLine(
                  $"Subasset sprite in sheet processed, name {assetName}/{sprite.SpriteName}");
              }
            }
            else if (metadataType == METADATATYPE_FONT_SPRITE_SHEET)
            {
              SpriteSheetDataTuple deserializedActualJsonData =
                JsonConvert.DeserializeObject<SpriteSheetDataTuple>(jsonFullText);

              Dictionary<string, SubassetDataSprite> fontSymbolsToSprites =
                new Dictionary<string, SubassetDataSprite>();


              foreach (var sprite in deserializedActualJsonData.Sprites)
              {
                SubassetDataSprite createdSprite = new SubassetDataSprite(workingWithTexture,
                  new Rectangle(sprite.X - 1, sprite.Y - 1, sprite.Width, sprite.Height));

                outDataSprites.Add($"{assetName}/{sprite.SpriteName}",
                  createdSprite);

                string fontSymbolNameOnly =
                  sprite.SpriteName.Replace(
                    AssetDataRelatedConsts.FONT_ASSET_SYMBOL_DEFINITION_PREFIX, "");

                if (fontSymbolNameOnly == ViewBehaviourConsts.SPECIAL_SYMBOL_SPACE)
                {
                  fontSymbolNameOnly = " ";
                }

                fontSymbolsToSprites.Add(fontSymbolNameOnly, createdSprite);

                Console.WriteLine(
                  $"Font sprite in sheet processed, name {assetName}/{sprite.SpriteName} symbol {fontSymbolNameOnly}");
              }

              outDataFonts.Add(assetName, new SubassetDataFont(fontSymbolsToSprites));
            }
          }
          else
          {
            outDataSprites.Add($"{assetName}",
              new SubassetDataSprite(workingWithTexture,
                new Rectangle(0, 0, workingWithTexture.Width, workingWithTexture.Height)));
            Console.WriteLine($"Sprite without metadata added {assetName}");
          }
        }
      }
    }

    /// <summary>
    /// Имя связанного с коллекцией пакета ресурсов
    /// </summary>
    private string AssetPackName { get; set; }
  }
}