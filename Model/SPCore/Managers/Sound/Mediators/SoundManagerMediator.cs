using CoreUtil.ResourceManager;
using Model.SPCore.Managers.Sound.Colleagues;
using Model.SPCore.Managers.Sound.Messages;
using Model.SPCore.Managers.Sound.Requests;

namespace Model.SPCore.Managers.Sound.Mediators
{
  /// <summary>
  /// Базовый класс для имплементации шаблона "посредник" для связи аудио-подсистем
  /// </summary>
  public abstract class SoundManagerMediator
  {
    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parActualResourceManager">Менеджер ресурсов</param>
    protected SoundManagerMediator(ResourceManager parActualResourceManager)
    {
      ActualResourceManager = parActualResourceManager;
    }

    /// <summary>
    /// Отправить сообщение "сотруднику"
    /// </summary>
    /// <param name="parSoundManagerMessage">Сообщение</param>
    /// <param name="parColleague">Целевой "сотрудник"</param>
    public abstract void Send(SoundManagerMessage parSoundManagerMessage, SoundManagerColleague parColleague);
    
    /// <summary>
    /// Отправить запрос "сотруднику"
    /// </summary>
    /// <param name="parSoundManagerRequest">Запрос</param>
    /// <param name="parColleague">Целевой "сотрудник"</param>
    public abstract void Request(SoundManagerRequest parSoundManagerRequest, SoundManagerColleague parColleague);
    
    /// <summary>
    /// Ссылка на используемый общий менеджер ресурсов
    /// </summary>
    public ResourceManager ActualResourceManager { get; }
  }
}