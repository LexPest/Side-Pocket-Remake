using CoreUtil.ResourceManager;
using Model.SPCore.Managers.Sound.Colleagues;
using Model.SPCore.Managers.Sound.Messages;
using Model.SPCore.Managers.Sound.Requests;

namespace Model.SPCore.Managers.Sound.Mediators
{
  /// <summary>
  /// Реализация посредника для менеджера аудио для взаимосвязи аудиоподсистем модели и отображения
  /// </summary>
  public class AppSoundManagerMediator : SoundManagerMediator
  {
    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parActualResourceManager">Менеджер ресурсов</param>
    public AppSoundManagerMediator(ResourceManager parActualResourceManager) : base(parActualResourceManager)
    {
    }

    /// <summary>
    /// Отправить сообщение "сотруднику"
    /// </summary>
    /// <param name="parSoundManagerMessage">Сообщение</param>
    /// <param name="parColleague">Целевой "сотрудник"</param>
    public override void Send(SoundManagerMessage parSoundManagerMessage, SoundManagerColleague parColleague)
    {
      if (parColleague == ViewSoundManagerColleague)
      {
        ViewSoundManagerColleague?.ProcessMessage(parSoundManagerMessage);
      }
      else if (parColleague == MainAppSoundManagerColleague)
      {
        MainAppSoundManagerColleague?.ProcessMessage(parSoundManagerMessage);
      }
    }

    /// <summary>
    /// Отправить запрос "сотруднику"
    /// </summary>
    /// <param name="parSoundManagerRequest">Запрос</param>
    /// <param name="parColleague">Целевой "сотрудник"</param>
    public override void Request(SoundManagerRequest parSoundManagerRequest, SoundManagerColleague parColleague)
    {
      if (parColleague == ViewSoundManagerColleague)
      {
        ViewSoundManagerColleague?.ProcessRequest(parSoundManagerRequest);
      }
      else if (parColleague == MainAppSoundManagerColleague)
      {
        MainAppSoundManagerColleague?.ProcessRequest(parSoundManagerRequest);
      }
    }

    /// <summary>
    /// "Сотрудник"-аудио менеджер со стороны модели
    /// </summary>
    public SoundManagerColleague MainAppSoundManagerColleague { get; set; }

    /// <summary>
    /// "Сотрудник"-аудио менеджер со стороны отображения
    /// </summary>
    public SoundManagerColleague ViewSoundManagerColleague { get; set; }
  }
}