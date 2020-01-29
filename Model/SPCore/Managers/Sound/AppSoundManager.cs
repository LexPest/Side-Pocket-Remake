using System.Collections.Generic;
using CoreUtil.ResourceManager;
using Model.SPCore.GameplayModelDefinition.ObjectModel;
using Model.SPCore.Managers.Sound.Colleagues;
using Model.SPCore.Managers.Sound.Data;
using Model.SPCore.Managers.Sound.Mediators;
using Model.SPCore.Managers.Sound.Messages;
using Model.SPCore.Managers.Sound.Requests;

namespace Model.SPCore.Managers.Sound
{
  /// <summary>
  /// Аудио менеджер приложения
  /// </summary>
  public class AppSoundManager
  {
    /// <summary>
    /// Данные о проигрываемой фоновой музыке
    /// </summary>
    private class PlayingBackgroundMusicData
    {
      /// <summary>
      /// Стандартный конструктор
      /// </summary>
      /// <param name="parLinkedAppSoundAsset">Связанный аудио-ресурс</param>
      /// <param name="parLinkedMusicAssetDefinition">Тип музыкальной композиции</param>
      public PlayingBackgroundMusicData(AppSoundAsset parLinkedAppSoundAsset,
        EAppMusicAssets parLinkedMusicAssetDefinition)
      {
        LinkedAppSoundAsset = parLinkedAppSoundAsset;
        LinkedMusicAssetDefinition = parLinkedMusicAssetDefinition;
      }

      /// <summary>
      /// Связанный аудио-ресурс
      /// </summary>
      public AppSoundAsset LinkedAppSoundAsset { get; private set; }

      /// <summary>
      /// Тип музыкальной композиции
      /// </summary>
      public EAppMusicAssets LinkedMusicAssetDefinition { get; private set; }
    }

    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parResourceManager">Менеджер ресурсов</param>
    /// <param name="parModel">Модель</param>
    public AppSoundManager(ResourceManager parResourceManager, IAppModel parModel)
    {
      AppSoundManagerMediator = new AppSoundManagerMediator(parResourceManager);
      AppSoundManagerMediator.MainAppSoundManagerColleague =
        AppSoundManagerColleague = new AppSoundManagerColleague(AppSoundManagerMediator);
      ActualAppModel = parModel;
    }

    /// <summary>
    /// Обновление данных менеджера. Предназначен для вызова каждый кадр в модели.
    /// </summary>
    public void ManagerUpdateStep()
    {
      //обновление состояния проигрывания звуковых эффектов
      for (var i = 0; i < PlayingSfx.Count; i++)
      {
        AppSoundAsset appSoundAsset = PlayingSfx[i];
        //используем запросы для получения данных
        SoundManagerRequestB requestBIsPlaying =
          new SoundManagerRequestB(ESoundManagerRequestType.IsSoundPlaying, appSoundAsset);
        AppSoundManagerMediator.Request(requestBIsPlaying, AppSoundManagerMediator.ViewSoundManagerColleague);

        SoundManagerRequestB requestBIsPaused =
          new SoundManagerRequestB(ESoundManagerRequestType.IsSoundPaused, appSoundAsset);
        AppSoundManagerMediator.Request(requestBIsPaused, AppSoundManagerMediator.ViewSoundManagerColleague);


        if (!requestBIsPlaying.RequestDataBool && !requestBIsPaused.RequestDataBool)
        {
          PlayingSfx.Remove(appSoundAsset);
          i--;
        }
      }
    }

    /// <summary>
    /// Предназначен для инициализации аудио-фонотеки и первичной загрузки ресурсов
    /// </summary>
    public void UpdateLibrary()
    {
      Dictionary<EAppMusicAssets, AppSoundAsset> musicToAssetsDict =
        new Dictionary<EAppMusicAssets, AppSoundAsset>();
      Dictionary<EAppSfxAssets, AppSoundAsset> sfxToAssetsDict = new Dictionary<EAppSfxAssets, AppSoundAsset>();

      foreach (var musicAssetInfo in AppAudioLibraryConsts.MusicAssetToAssetInfoSmd)
      {
        AssetMetadata assetMetadata = LinkedResourceManager.GetAssetMetadata(musicAssetInfo.Value.AssetPackName,
          musicAssetInfo.Value.AssetName);
        if (assetMetadata != null)
        {
          musicToAssetsDict.Add(musicAssetInfo.Key, new AppSoundAsset(assetMetadata));
        }
      }

      foreach (var soundAssetInfo in AppAudioLibraryConsts.SfxAssetToAssetName)
      {
        AssetMetadata assetMetadata = LinkedResourceManager.GetAssetMetadata(
          AppAudioLibraryConsts.SFX_SMD_PACKAGE_NAME,
          soundAssetInfo.Value);
        if (assetMetadata != null)
        {
          sfxToAssetsDict.Add(soundAssetInfo.Key, new AppSoundAsset(assetMetadata));
        }
      }

      CurrentAudioLibrary = new AppAudioLibrary(musicToAssetsDict, sfxToAssetsDict);

      LoadSfxPacks();
    }

    /// <summary>
    /// Загрузить пакеты ресурсов со звуковыми эффектами. Предназначен для вызова после инициализации
    /// </summary>
    public void LoadSfxPacks()
    {
      UnloadSfxPacks();
      foreach (var appSoundAssetPair in CurrentAudioLibrary.SoundEffects)
      {
        LinkedResourceManager.GetAssetInfo(appSoundAssetPair.Value.LinkedAssetMetadata, out string assetPackName,
          out string assetName);

        if (!LoadedSfxAssetPacks.Contains(assetPackName))
        {
          LoadedSfxAssetPacks.Add(assetPackName);
          LinkedResourceManager.LoadAssetPack(assetPackName);
        }
      }
    }

    /// <summary>
    /// Выгрузить из памяти пакеты ресурсов со звуковыми эффектами
    /// </summary>
    private void UnloadSfxPacks()
    {
      foreach (var loadedSfxAssetPack in LoadedSfxAssetPacks)
      {
        LinkedResourceManager.UnloadAssetPack(loadedSfxAssetPack);
      }
    }

    /// <summary>
    /// Воспроизвести звуковой эффект
    /// </summary>
    /// <param name="parSfxAsset">Тип звукового эффекта</param>
    /// <param name="parIsLooped">Зациклить воспроизведение?</param>
    public void PlaySfx(EAppSfxAssets parSfxAsset, bool parIsLooped)
    {
      //play sound
      if (ActualAppModel.GetGameplaySettingsData().IsSfxEnabled)
      {
        if (CurrentAudioLibrary.SoundEffects.TryGetValue(parSfxAsset, out AppSoundAsset soundAsset))
        {
          AppSoundManagerMediator.Send(
            new SoundManagerMessage(ESoundManagerMessageType.SoundPlay, soundAsset),
            AppSoundManagerMediator.ViewSoundManagerColleague);
          PlayingSfx.Add(soundAsset);
        }
      }
    }

    /// <summary>
    /// Осуществляет проверку, что требуемый тип музыки играет в данный момент, если не
    /// играет, то начинает его воспроизведение.
    /// </summary>
    /// <param name="parMusicAsset">Требуемый тип фоновой музыки</param>
    /// <param name="parIsLooped">Зациклить воспроизведение?</param>
    public void EnsureBgMusicIsPlaying(EAppMusicAssets parMusicAsset, bool parIsLooped)
    {
      if (PlayingBackgroundMusic == null)
      {
        PlayBgMusic(parMusicAsset, parIsLooped);
      }
      else if (PlayingBackgroundMusic.LinkedMusicAssetDefinition != parMusicAsset)
      {
        PlayBgMusic(parMusicAsset, parIsLooped);
      }
      else
      {
        if (IsMusicPaused())
        {
          PlayBgMusic(parMusicAsset, parIsLooped);
        }
      }
    }

    /// <summary>
    /// Начать воспроизведение фоновой музыки
    /// </summary>
    /// <param name="parMusicAsset">Тип фоновой музыки</param>
    /// <param name="parIsLooped">Зациклить воспроизведение?</param>
    public void PlayBgMusic(EAppMusicAssets parMusicAsset, bool parIsLooped)
    {
      //выгрузим старый ресурс фоновой музыки
      if (PlayingBackgroundMusic != null)
      {
        AppSoundManagerMediator.Send(
          new SoundManagerMessage(ESoundManagerMessageType.SoundReset, PlayingBackgroundMusic.LinkedAppSoundAsset),
          AppSoundManagerMediator.ViewSoundManagerColleague);

        while (true)
        {
          SoundManagerRequestB requestBIsPlaying =
            new SoundManagerRequestB(ESoundManagerRequestType.IsSoundPlaying,
              PlayingBackgroundMusic.LinkedAppSoundAsset);
          AppSoundManagerMediator.Request(requestBIsPlaying, AppSoundManagerMediator.ViewSoundManagerColleague);

          if (!requestBIsPlaying.RequestDataBool)
          {
            break;
          }
        }

        LinkedResourceManager.GetAssetInfo(PlayingBackgroundMusic.LinkedAppSoundAsset.LinkedAssetMetadata,
          out string assetPackName,
          out string assetName);
        if (assetPackName != null && assetName != null)
        {
          //выгрузка
          LinkedResourceManager.UnloadAssetPack(assetPackName);
        }
      }

      if (ActualAppModel.GetGameplaySettingsData().IsMusicEnabled)
      {
        //загружаемый новый ресурс фоновой музыки
        if (CurrentAudioLibrary.Music.TryGetValue(parMusicAsset, out AppSoundAsset soundAsset))
        {
          LinkedResourceManager.GetAssetInfo(soundAsset.LinkedAssetMetadata, out string assetPackName,
            out string assetName);
          if (assetPackName != null && assetName != null)
          {
            //загрузка
            LinkedResourceManager.LoadAssetPack(assetPackName);

            //воспроизведение
            AppSoundManagerMediator.Send(
              new SoundManagerMessageB(ESoundManagerMessageType.SoundPlay, soundAsset, parIsLooped),
              AppSoundManagerMediator.ViewSoundManagerColleague);
            PlayingBackgroundMusic = new PlayingBackgroundMusicData(soundAsset, parMusicAsset);
          }
        }
      }
    }

    /// <summary>
    /// Приостанавливает воспроизведение звукового эффекта
    /// </summary>
    /// <param name="parSfxAsset">Тип звукового эффекта</param>
    public void PauseSfx(EAppSfxAssets parSfxAsset)
    {
      //пытаемся приостановить звук
      if (CurrentAudioLibrary.SoundEffects.TryGetValue(parSfxAsset, out AppSoundAsset soundAsset))
      {
        PauseSound(soundAsset);
      }
    }

    /// <summary>
    /// Приостанавливает воспроизведение фоновой музыки
    /// </summary>
    public void PauseMusic()
    {
      //пытаемся приостановить музыку
      if (PlayingBackgroundMusic != null)
      {
        PauseSound(PlayingBackgroundMusic.LinkedAppSoundAsset);
      }
    }

    /// <summary>
    /// Вспомогательный метод приостановки воспроизведения аудио ресурса
    /// </summary>
    /// <param name="parAppSoundAsset">Целевой аудио ресурс</param>
    public void PauseSound(AppSoundAsset parAppSoundAsset)
    {
      AppSoundManagerMediator.Send(new SoundManagerMessage(ESoundManagerMessageType.SoundPause, parAppSoundAsset),
        AppSoundManagerMediator.ViewSoundManagerColleague);
    }

    /// <summary>
    /// Сбросить процесс воспроизведения звукового эффекта
    /// </summary>
    /// <param name="parSfxAsset">Тип звукового эффекта</param>
    public void ResetSfx(EAppSfxAssets parSfxAsset)
    {
      if (CurrentAudioLibrary.SoundEffects.TryGetValue(parSfxAsset, out AppSoundAsset soundAsset))
      {
        ResetSound(soundAsset);
      }
    }

    /// <summary>
    /// Сбросить процесс воспроизведения фоновой музыки
    /// </summary>
    public void ResetMusic()
    {
      if (PlayingBackgroundMusic != null)
      {
        ResetSound(PlayingBackgroundMusic.LinkedAppSoundAsset);
      }
    }

    /// <summary>
    /// Вспомогательный метод для сброса процесса воспроизведения аудио ресурса
    /// </summary>
    /// <param name="parAppSoundAsset">Целевой аудио ресурс</param>
    public void ResetSound(AppSoundAsset parAppSoundAsset)
    {
      AppSoundManagerMediator.Send(new SoundManagerMessage(ESoundManagerMessageType.SoundReset, parAppSoundAsset),
        AppSoundManagerMediator.ViewSoundManagerColleague);
    }

    /// <summary>
    /// Остановить воспроизведение звукового эффекта
    /// </summary>
    /// <param name="parSfxAsset">Тип звукового эффекта</param>
    public void StopSfx(EAppSfxAssets parSfxAsset)
    {
      if (CurrentAudioLibrary.SoundEffects.TryGetValue(parSfxAsset, out AppSoundAsset soundAsset))
      {
        StopSound(soundAsset);
      }
    }

    /// <summary>
    /// Остановить воспроизведение фоновой музыки
    /// </summary>
    public void StopMusic()
    {
      if (PlayingBackgroundMusic != null)
      {
        StopSound(PlayingBackgroundMusic.LinkedAppSoundAsset);
        PlayingBackgroundMusic = null;
      }
    }

    /// <summary>
    /// Вспомогательный метод для остановки воспроизведения аудио ресурса
    /// </summary>
    /// <param name="parAppSoundAsset">Целевой аудио ресурс</param>
    public void StopSound(AppSoundAsset parAppSoundAsset)
    {
      AppSoundManagerMediator.Send(new SoundManagerMessage(ESoundManagerMessageType.SoundStop, parAppSoundAsset),
        AppSoundManagerMediator.ViewSoundManagerColleague);
    }

    /// <summary>
    /// Проигрывается ли сейчас данный звуковой эффект?
    /// </summary>
    /// <param name="parSfxAsset">Тип звукового эффекта</param>
    /// <returns>True, если проигрывается</returns>
    public bool IsSfxPlaying(EAppSfxAssets parSfxAsset)
    {
      if (CurrentAudioLibrary.SoundEffects.TryGetValue(parSfxAsset, out AppSoundAsset soundAsset))
      {
        return PlayingSfx.Contains(soundAsset);
      }

      return false;
    }

    /// <summary>
    /// Проигрывается ли сейчас фоновая музыка?
    /// </summary>
    /// <returns>True, если проигрывается</returns>
    public bool IsMusicPlaying()
    {
      if (PlayingBackgroundMusic == null)
      {
        return false;
      }

      SoundManagerRequestB requestBMusicIsPlaying =
        new SoundManagerRequestB(ESoundManagerRequestType.IsSoundPlaying, PlayingBackgroundMusic.LinkedAppSoundAsset);
      AppSoundManagerMediator.Request(requestBMusicIsPlaying, AppSoundManagerMediator.ViewSoundManagerColleague);
      return requestBMusicIsPlaying.RequestDataBool;
    }

    /// <summary>
    /// Приостановлено ли воспроизведение данного звукового эффекта?
    /// </summary>
    /// <param name="parSfxAsset">Тип звукового эффекта</param>
    /// <returns>True, если воспроизведение приостановлено</returns>
    public bool IsSfxPaused(EAppSfxAssets parSfxAsset)
    {
      if (CurrentAudioLibrary.SoundEffects.TryGetValue(parSfxAsset, out AppSoundAsset soundAsset))
      {
        return IsSoundPaused(soundAsset);
      }

      return false;
    }

    /// <summary>
    /// Приостановлено ли воспроизведение фоновой музыки?
    /// </summary>
    /// <returns>True, если воспроизведение приостановлено</returns>
    public bool IsMusicPaused()
    {
      if (PlayingBackgroundMusic != null)
      {
        return IsSoundPaused(PlayingBackgroundMusic.LinkedAppSoundAsset);
      }

      return false;
    }

    /// <summary>
    /// Вспомогательный метод для проверки приостановки воспроизведения аудио ресурса
    /// </summary>
    /// <param name="parAppSoundAsset">Целевой аудио ресурс для проверки</param>
    /// <returns>True, если воспроизведение приостановлено</returns>
    public bool IsSoundPaused(AppSoundAsset parAppSoundAsset)
    {
      SoundManagerRequestB request = new SoundManagerRequestB(ESoundManagerRequestType.IsSoundPaused, parAppSoundAsset);
      return request.RequestDataBool;
    }

    /// <summary>
    /// Приостановить воспроизведение всех звуковых эффектов
    /// </summary>
    public void PauseAllSfxSounds()
    {
      foreach (var appSoundAsset in PlayingSfx)
      {
        AppSoundManagerMediator.Send(new SoundManagerMessage(ESoundManagerMessageType.SoundPause, appSoundAsset),
          AppSoundManagerMediator.ViewSoundManagerColleague);
      }
    }

    /// <summary>
    /// Возобновить воспроизведение всех звуковых эффектов
    /// </summary>
    public void ResumeAllSfxSounds()
    {
      foreach (var appSoundAsset in PlayingSfx)
      {
        AppSoundManagerMediator.Send(new SoundManagerMessage(ESoundManagerMessageType.SoundPause, appSoundAsset),
          AppSoundManagerMediator.ViewSoundManagerColleague);
      }
    }

    /// <summary>
    /// Связанный посредник для взаимодействия с отображением
    /// </summary>
    public AppSoundManagerMediator AppSoundManagerMediator { get; private set; }

    /// <summary>
    /// Связанный с аудио менеджером "сотрудник"
    /// </summary>
    public AppSoundManagerColleague AppSoundManagerColleague { get; private set; }

    /// <summary>
    /// Текущие проигрываемые звуковые эффекты
    /// </summary>
    public List<AppSoundAsset> PlayingSfx { get; private set; } = new List<AppSoundAsset>();

    /// <summary>
    /// Текущая проигрываемая фоновая музыка
    /// </summary>
    private PlayingBackgroundMusicData PlayingBackgroundMusic { get; set; } = null;

    /// <summary>
    /// Текущая аудио фонотека
    /// </summary>
    private AppAudioLibrary CurrentAudioLibrary { get; set; } = new AppAudioLibrary();

    /// <summary>
    /// Связанный менеджер ресурсов
    /// </summary>
    private ResourceManager LinkedResourceManager => AppSoundManagerMediator.ActualResourceManager;

    /// <summary>
    /// Загруженные пакеты ресурсов, содержащих звуковые эффекты
    /// </summary>
    private List<string> LoadedSfxAssetPacks { get; set; } = new List<string>();

    /// <summary>
    /// Связанная модель
    /// </summary>
    private IAppModel ActualAppModel { get; set; }
  }
}