using Model.SPCore.Managers.Sound.Mediators;
using Model.SPCore.Managers.Sound.Messages;
using Model.SPCore.Managers.Sound.Requests;

namespace Model.SPCore.Managers.Sound.Colleagues
{
  /// <summary>
  /// Базовый класс для сотрудников шаблона "Посредник" для аудио менеджера
  /// </summary>
  public abstract class SoundManagerColleague
  {
    /// <summary>
    /// Стандартный конструктор без параметров
    /// </summary>
    protected SoundManagerColleague()
    {
    }

    /// <summary>
    /// Конструктор с параметром посредника
    /// </summary>
    /// <param name="parMediator">Посредник</param>
    protected SoundManagerColleague(SoundManagerMediator parMediator)
    {
      Mediator = parMediator;
    }

    /// <summary>
    /// Обработать сообщение
    /// </summary>
    /// <param name="parSoundManagerMessage">Сообщение</param>
    public abstract void ProcessMessage(SoundManagerMessage parSoundManagerMessage);

    /// <summary>
    /// Обработать запрос
    /// </summary>
    /// <param name="parSoundManagerRequest">Запрос</param>
    public abstract void ProcessRequest(SoundManagerRequest parSoundManagerRequest);
    
    /// <summary>
    /// Посредник для сотрудника
    /// </summary>
    public SoundManagerMediator Mediator { get; set; }
  }
}