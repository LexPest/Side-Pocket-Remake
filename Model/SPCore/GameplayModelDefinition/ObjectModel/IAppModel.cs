using CoreUtil.ResourceManager;
using Model.SPCore.DS;
using Model.SPCore.GameplayModelDefinition.GameComponents.ViewProviderBase;
using Model.SPCore.Managers.Sound;
using Model.SPCore.MPlayers;

namespace Model.SPCore.GameplayModelDefinition.ObjectModel
{
  /// <summary>
  /// Интерфейс для доступа к основным ресурсам и данным модели
  /// </summary>
  public interface IAppModel
  {
    /// <summary>
    /// Получает путь к файлу настроек игрового процесса
    /// </summary>
    /// <returns></returns>
    GameplaySettingsData GetGameplaySettingsData();

    /// <summary>
    /// Получает путь к файлу настроек приложения
    /// </summary>
    /// <returns></returns>
    string GetAppSettingsPath();

    /// <summary>
    /// Получает данные о таблице рекордов
    /// </summary>
    /// <returns></returns>
    RecordsData GetRecordsData();

    /// <summary>
    /// Получает путь к файлу с данными таблицы рекордов
    /// </summary>
    /// <returns></returns>
    string GetRecordsDataPath();

    /// <summary>
    /// Получает менеджера ввода игроков
    /// </summary>
    /// <returns></returns>
    MPlayersManager GetPlayersManager();

    /// <summary>
    /// Получает менеджер ресурсов
    /// </summary>
    /// <returns></returns>
    ResourceManager GetResourcesManager();

    /// <summary>
    /// Получает посредника между моделью и отображением
    /// </summary>
    /// <returns></returns>
    ViewProviderMediator GetViewProviderMediator();

    /// <summary>
    /// Получает аудио менеджер
    /// </summary>
    /// <returns></returns>
    AppSoundManager GetSoundManager();

    /// <summary>
    /// Производит регистрацию обновляемого объекта в модели для последующих отправок сигналов об обновлении
    /// </summary>
    /// <param name="parUpdatableObject">Целевой обновляемый объект</param>
    void RegisterUpdatableObject(IUpdatable parUpdatableObject);

    /// <summary>
    /// Производит регистрацию фиксированно обновляемого объекта в модели для последующих
    /// отправок сигналов о фиксированных обновлениях
    /// </summary>
    /// <param name="parFixedUpdatableObject">Целевой фиксированно обновляемый объект</param>
    void RegisterFixedUpdatableObject(IFixedUpdatable parFixedUpdatableObject);

    /// <summary>
    /// Отменяет регистрацию обновляемого объекта в модели для последующих отправок сигналов об обновлении
    /// </summary>
    /// <param name="parUpdatableObject">Целевой объект</param>
    void UnregisterUpdatableObject(IUpdatable parUpdatableObject);
    
    /// <summary>
    /// Отменяет регистрацию фиксированно обновляемого объекта в модели для последующих
    /// отправок сигналов о фиксированных обновлениях
    /// </summary>
    /// <param name="parFixedUpdatableObject">Целевой объект</param>
    void UnregisterFixedUpdatableObject(IFixedUpdatable parFixedUpdatableObject);

    void ExitApp();
  }
}