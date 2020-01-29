using Model.SPCore.Managers.Sound.Mediators;
using Model.SPCore.Managers.Sound.Messages;
using Model.SPCore.Managers.Sound.Requests;

namespace Model.SPCore.Managers.Sound.Colleagues
{
  /// <summary>
  /// Реализация "сотрудника" для аудио менеджера со стороны модели
  /// </summary>
  public class AppSoundManagerColleague : SoundManagerColleague
  {
    /// <summary>
    /// Стандартный конструктор без параметров
    /// </summary>
    public AppSoundManagerColleague()
    {
    }

    /// <summary>
    /// Конструктор с параметром посредника
    /// </summary>
    /// <param name="parMediator">Посредник</param>
    public AppSoundManagerColleague(SoundManagerMediator parMediator) : base(parMediator)
    {
    }

    /// <summary>
    /// Обработать сообщение
    /// </summary>
    /// <param name="parSoundManagerMessage">Сообщение</param>
    public override void ProcessMessage(SoundManagerMessage parSoundManagerMessage)
    {
      // в этой версии обработка сообщений AppSoundManager'ом не требуется и не поддерживается,
      // но с большой долей вероятности она потребуется в будущем
      return;
    }

    /// <summary>
    /// Обработать запрос
    /// </summary>
    /// <param name="parSoundManagerRequest">Запрос</param>
    public override void ProcessRequest(SoundManagerRequest parSoundManagerRequest)
    {
      // в этой версии обработка сообщений AppSoundManager'ом не требуется и не поддерживается,
      // но с большой долей вероятности она потребуется в будущем
      return;
    }
  }
}