using Model.SPCore.Managers.Sound.Data;

namespace Model.SPCore.Managers.Sound.Requests
{
  /// <summary>
  /// Базовый класс запроса "сотруднику"-аудио менеджеру
  /// </summary>
  public abstract class SoundManagerRequest
  {
    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parRequestType">Тип запроса</param>
    /// <param name="parSoundAsset">Связанный звуковой ресурс</param>
    protected SoundManagerRequest(ESoundManagerRequestType parRequestType, AppSoundAsset parSoundAsset)
    {
      RequestType = parRequestType;
      SoundAsset = parSoundAsset;
    }

    /// <summary>
    /// Тип запроса
    /// </summary>
    public ESoundManagerRequestType RequestType { get; private set; }
    
    /// <summary>
    /// Связанный звуковой ресурс
    /// </summary>
    public AppSoundAsset SoundAsset { get; private set; }
  }
}