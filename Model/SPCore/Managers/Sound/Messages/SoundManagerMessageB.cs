using Model.SPCore.Managers.Sound.Data;

namespace Model.SPCore.Managers.Sound.Messages
{
  /// <summary>
  /// Сообщение "сотруднику"-аудио менеджеру с параметром bool
  /// </summary>
  public class SoundManagerMessageB : SoundManagerMessage
  {
    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parMessageType">Тип сообшения</param>
    /// <param name="parSoundAsset">Связанный звуковой ресурс</param>
    /// <param name="parAmBool">Параметр сообщения bool</param>
    public SoundManagerMessageB(ESoundManagerMessageType parMessageType, AppSoundAsset parSoundAsset, bool parAmBool) : base(
      parMessageType, parSoundAsset)
    {
      ParamBool = parAmBool;
    }

    /// <summary>
    /// Параметр сообщения bool
    /// </summary>
    public bool ParamBool { get; private set; }
  }
}