using System.Collections.Generic;

namespace Model.SPCore.Managers.Sound.Data
{
  /// <summary>
  /// Аудио фонотека
  /// </summary>
  public class AppAudioLibrary
  {
    /// <summary>
    /// Стандартный конструктор без параметров для создания пустой фонотеки
    /// </summary>
    public AppAudioLibrary()
    {
    }

    /// <summary>
    /// Дополнительный конструтор для создания заполненной фонотеки
    /// </summary>
    /// <param name="parMusic">Словарь с музыкой</param>
    /// <param name="parSoundEffects">Словарь со звуками</param>
    public AppAudioLibrary(Dictionary<EAppMusicAssets, AppSoundAsset> parMusic,
      Dictionary<EAppSfxAssets, AppSoundAsset> parSoundEffects)
    {
      Music = parMusic;
      SoundEffects = parSoundEffects;
    }

    /// <summary>
    /// Словарь с музыкой
    /// </summary>
    public Dictionary<EAppMusicAssets, AppSoundAsset> Music { get; private set; }
    
    /// <summary>
    /// Словарь со звуками
    /// </summary>
    public Dictionary<EAppSfxAssets, AppSoundAsset> SoundEffects { get; private set; }
  }
}