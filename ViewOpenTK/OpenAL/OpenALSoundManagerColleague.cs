using System;
using CoreUtil.ResourceManager;
using Model.SPCore.Managers.Sound;
using Model.SPCore.Managers.Sound.Colleagues;
using Model.SPCore.Managers.Sound.Mediators;
using Model.SPCore.Managers.Sound.Messages;
using Model.SPCore.Managers.Sound.Requests;
using ViewOpenTK.AssetData.DataTypes;
using ViewOpenTK.OpenAL.BasicManager;

namespace ViewOpenTK.OpenAL
{
  /// <summary>
  /// Реализация "сотрудника" со стороны отображения и использующего OpenTK
  /// </summary>
  public class OpenAlSoundManagerColleague : SoundManagerColleague
  {
    /// <summary>
    /// Стандартный конструктор без параметров
    /// </summary>
    public OpenAlSoundManagerColleague()
    {
    }

    /// <summary>
    /// Конструктор с параметром посредника
    /// </summary>
    /// <param name="parMediator">Посредник аудио менеджера</param>
    public OpenAlSoundManagerColleague(SoundManagerMediator parMediator) : base(parMediator)
    {
    }

    /// <summary>
    /// Обработать сообщение
    /// </summary>
    /// <param name="parSoundManagerMessage">Сообщение</param>
    public override void ProcessMessage(SoundManagerMessage parSoundManagerMessage)
    {
      switch (parSoundManagerMessage.MessageType)
      {
        case ESoundManagerMessageType.SoundPlay:
        {
          bool isLooped = false;
          if (parSoundManagerMessage is SoundManagerMessageB b)
          {
            isLooped = b.ParamBool;
          }

          OpenAlManager.Instance.Play(GetWaveSoundAssetData(Mediator.ActualResourceManager,
            parSoundManagerMessage.SoundAsset.LinkedAssetMetadata), isLooped);

          break;
        }
        case ESoundManagerMessageType.SoundPause:
          OpenAlManager.Instance.Pause(GetWaveSoundAssetData(Mediator.ActualResourceManager,
            parSoundManagerMessage.SoundAsset.LinkedAssetMetadata));
          break;
        case ESoundManagerMessageType.SoundStop:
          OpenAlManager.Instance.Stop(GetWaveSoundAssetData(Mediator.ActualResourceManager,
            parSoundManagerMessage.SoundAsset.LinkedAssetMetadata));
          break;
        case ESoundManagerMessageType.SoundReset:
          OpenAlManager.Instance.Rewind(GetWaveSoundAssetData(Mediator.ActualResourceManager,
            parSoundManagerMessage.SoundAsset.LinkedAssetMetadata));
          break;
        case ESoundManagerMessageType.SoundSetLoop:
        {
          bool isLooped = false;
          if (parSoundManagerMessage is SoundManagerMessageB b)
          {
            isLooped = b.ParamBool;
          }

          OpenAlManager.Instance.SetIsLooped(GetWaveSoundAssetData(Mediator.ActualResourceManager,
            parSoundManagerMessage.SoundAsset.LinkedAssetMetadata), isLooped);
          break;
        }
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    /// <summary>
    /// Обработать запрос
    /// </summary>
    /// <param name="parSoundManagerRequest">Запрос</param>
    public override void ProcessRequest(SoundManagerRequest parSoundManagerRequest)
    {
      switch (parSoundManagerRequest.RequestType)
      {
        case ESoundManagerRequestType.IsSoundPlaying:
        {
          if (parSoundManagerRequest is SoundManagerRequestB bRequest)
          {
            bRequest.RequestDataBool = OpenAlManager.Instance.IsPlaying(GetWaveSoundAssetData(
              Mediator.ActualResourceManager,
              parSoundManagerRequest.SoundAsset.LinkedAssetMetadata));
          }

          break;
        }
        case ESoundManagerRequestType.IsSoundPaused:
        {
          if (parSoundManagerRequest is SoundManagerRequestB bRequest)
          {
            bRequest.RequestDataBool = OpenAlManager.Instance.IsPaused(GetWaveSoundAssetData(
              Mediator.ActualResourceManager,
              parSoundManagerRequest.SoundAsset.LinkedAssetMetadata));
          }

          break;
        }
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    /// <summary>
    /// Получить ассет аудио данных OpenTK
    /// </summary>
    /// <param name="parResourceManager">Менеджер ресурсов</param>
    /// <param name="parAssetMetadata">Метаданные получаемого ассета</param>
    /// <returns></returns>
    private AssetDataOpenTkWaveSound GetWaveSoundAssetData(ResourceManager parResourceManager,
      AssetMetadata parAssetMetadata)
    {
      return parResourceManager.GetAssetData<AssetDataOpenTkWaveSound>(parAssetMetadata);
    }
  }
}