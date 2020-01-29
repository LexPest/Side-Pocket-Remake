using Model.SPCore.Managers.Sound.Data;

namespace Model.SPCore.Managers.Sound.Messages
{
  /// <summary>
  /// Базовый класс сообщения "сотруднику"-аудио менеджеру
  /// </summary>
  public class SoundManagerMessage
  {
    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parMessageType">Тип сообшения</param>
    /// <param name="parSoundAsset">Связанный звуковой ресурс</param>
    public SoundManagerMessage(ESoundManagerMessageType parMessageType, AppSoundAsset parSoundAsset)
    {
      MessageType = parMessageType;
      SoundAsset = parSoundAsset;
    }

    /// <summary>
    /// Тип сообщения
    /// </summary>
    public ESoundManagerMessageType MessageType { get; private set; }
    
    /// <summary>
    /// Связанный звуковой ресурс
    /// </summary>
    public AppSoundAsset SoundAsset { get; private set; }
  }
}