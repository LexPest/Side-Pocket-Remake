using Model.SPCore.Managers.Sound.Data;

namespace Model.SPCore.Managers.Sound.Requests
{
  /// <summary>
  /// Запрос "сотруднику"-аудио менеджеру с запрашиваемым параметром типа bool
  /// </summary>
  public class SoundManagerRequestB : SoundManagerRequest
  {
    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parRequestType">Тип запроса</param>
    /// <param name="parSoundAsset">Связанный звуковой ресурс</param>
    public SoundManagerRequestB(ESoundManagerRequestType parRequestType, AppSoundAsset parSoundAsset) : base(parRequestType,
      parSoundAsset)
    {
    }

    /// <summary>
    /// Запрашиваемый параметр типа bool
    /// </summary>
    public bool RequestDataBool { get; set; }
  }
}