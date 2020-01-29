using Model.SPCore.Managers.Sound.Data;

namespace Model.SPCore.Managers.Sound.Messages
{
  /// <summary>
  /// Сообщение "сотруднику"-аудио менеджеру с параметром float
  /// </summary>
  public class SoundManagerMessageF : SoundManagerMessage
  {
    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parMessageType">Тип сообшения</param>
    /// <param name="parSoundAsset">Связанный звуковой ресурс</param>
    /// <param name="parAmFloat">Параметр сообщения float</param>
    public SoundManagerMessageF(ESoundManagerMessageType parMessageType, AppSoundAsset parSoundAsset, float parAmFloat) :
      base(parMessageType, parSoundAsset)
    {
      ParamFloat = parAmFloat;
    }

    /// <summary>
    /// Параметр сообщения float
    /// </summary>
    public float ParamFloat { get; private set; }
  }
}