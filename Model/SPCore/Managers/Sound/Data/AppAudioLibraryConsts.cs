using System.Collections.Generic;

namespace Model.SPCore.Managers.Sound.Data
{
  /// <summary>
  /// Константы, связанные с аудио фонотекой приложения
  /// </summary>
  public static class AppAudioLibraryConsts
  {
    /// <summary>
    /// Название пакета ресурсов со звуковыми эффектами
    /// </summary>
    public const string SFX_SMD_PACKAGE_NAME = "sfx_smd";

    /// <summary>
    /// Стандартный словарь доступных звуковых эффектов
    /// </summary>
    public static readonly Dictionary<EAppSfxAssets, string> SfxAssetToAssetName =
      new Dictionary<EAppSfxAssets, string>()
      {
        {EAppSfxAssets.IntroCutscene, "/data_east_logo.wav"},
        {EAppSfxAssets.BallCollision, "/ball_collision.wav"},
        {EAppSfxAssets.BallCollisionWallSolid, "/ball_collision_solid.wav"},
        {EAppSfxAssets.BallPocketed, "/BallToPocket.wav"},
        {EAppSfxAssets.BallShotByCue, "/ballshot_cue.wav"}
      };

    /// <summary>
    /// Стандартный словарь доступной фоновой музыки
    /// </summary>
    public static readonly Dictionary<EAppMusicAssets, string> JukeboxMusicSmdLibrary =
      new Dictionary<EAppMusicAssets, string>()
      {
        {EAppMusicAssets.MainMenu, "Whispers"},
        {EAppMusicAssets.LoungeBeforeLevelStart, "The Lounge"},
        {EAppMusicAssets.SPLevel1, "Groovin'"},
        {EAppMusicAssets.SPLevel2, "Play'n It Cool!"},
        {EAppMusicAssets.SPLevel3, "Cue Ball Boogie"},
        {EAppMusicAssets.SPLevel4, "Las Vegas"},
        {EAppMusicAssets.SPLevel5, "Come on Over"},
        {EAppMusicAssets.LoungeStagePassed, "Sunny Morning"},
        {EAppMusicAssets.ZoneBonus, "Meteor-Man"},
        {EAppMusicAssets.LevelResults, "Round And Round"},
        {EAppMusicAssets.LoungeCouchPlayBeforeLevelStart, "Dinner for Two"},
        {EAppMusicAssets.CPLevel1, "Stylin'"},
        {EAppMusicAssets.ChallengeShotLevel, "Chillin'"},
        {EAppMusicAssets.Credits, "California Lemonade"},
        {EAppMusicAssets.LoungeStageTryAgain, "Midnight Kiss"},
        {EAppMusicAssets.Victory, "Fight It Out"},
        {EAppMusicAssets.SPLevelEnd, "Wrappin' Up"},
        {EAppMusicAssets.GameOver, "Don't Go, Baby"}
      };

    /// <summary>
    /// Словарь соответствия типа музыки и метаданных музыкального ресурса (будет заполнен в конструкторе)
    /// </summary>
    public static readonly Dictionary<EAppMusicAssets, MusicAssetInfo> MusicAssetToAssetInfoSmd =
      new Dictionary<EAppMusicAssets, MusicAssetInfo>();

    /// <summary>
    /// Статический конструктор
    /// </summary>
    static AppAudioLibraryConsts()
    {
      foreach (var smdLibMusic in JukeboxMusicSmdLibrary)
      {
        MusicAssetToAssetInfoSmd.Add(smdLibMusic.Key, new MusicAssetInfo(
          $"msx_smd_{smdLibMusic.Key.ToString()}",
          $"/{smdLibMusic.Value}.ogg"));
      }
    }
  }
}