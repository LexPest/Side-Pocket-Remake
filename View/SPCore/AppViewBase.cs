using Model.SPCore;
using Model.SPCore.Consts;
using Model.SPCore.GameplayModelDefinition.GameComponents.ViewProviderBase;
using Model.SPCore.Managers.Sound.Colleagues;
using View.SPCore.DS;

namespace View.SPCore
{
    /// <summary>
    /// Базовый класс для компонента отображения приложения MVC
    /// </summary>
    public abstract class AppViewBase
    {
        /// <summary>
        /// Объект для синхронизации обработки событий ввода многопоточного приложения
        /// </summary>
        public object InputSyncObj = new object();
        
        /// <summary>
        /// Является ли отображение сейчас доступным и активным
        /// </summary>
        public bool ViewAvailable { get; set; } = false;
        
        /// <summary>
        /// Путь к настройкам графики и отображения
        /// </summary>
        public string GraphicalSettingsPath { get; private set; }

        /// <summary>
        /// Данные текущих настроек графики и отображения
        /// </summary>
        public GraphicsSettingsDataBase CurrentGraphicsSettings { get; protected set; }

        /// <summary>
        /// Прикрепленная модель приложения
        /// </summary>
        public AppModel App { get; private set; }
        
        /// <summary>
        /// Стандартный конструктор
        /// </summary>
        /// <param name="parApp">Прикрепляемая модель приложения</param>
        /// <param name="parGraphicalSettingsPath">Путь к настройкам графики и отображения</param>
        public AppViewBase(AppModel parApp, string parGraphicalSettingsPath)
        {
            App = parApp;
            GraphicalSettingsPath = parGraphicalSettingsPath;
            if (parGraphicalSettingsPath == null)
            {
                GraphicalSettingsPath = AppInfoConsts.GraphicsSettingsPath;
            }
            LoadSettings();
        }

        /// <summary>
        /// Создает файл со стандартными настройками отображения
        /// </summary>
        public abstract void CreateDefaultGraphicalSettings();

        /// <summary>
        /// Осуществляет загрузку настроек отображения
        /// </summary>
        public abstract void LoadSettings();
        
        /// <summary>
        /// Осуществляет первоначальную инициализацию
        /// </summary>
        public abstract void InitializeView();
        
        /// <summary>
        /// Производит очистку отображения
        /// </summary>
        public abstract void CleanView();

        /// <summary>
        /// Передает команду на рендеринг отображения модели
        /// </summary>
        public abstract void Render();

        /// <summary>
        /// Полностью сбрасывает компонент вида MVC
        /// </summary>
        public abstract void ViewReset();

        /// <summary>
        /// Применяет установленные настройки графики
        /// </summary>
        public abstract void ApplyGraphicsSettings();

        /// <summary>
        /// Сигнал обновления компонентов вида
        /// </summary>
        /// <param name="parDeltaTime">Время кадра</param>
        public abstract void ViewUpdateSignal(double parDeltaTime);

        /// <summary>
        /// Сигнал об обновлении предоставленной информации о количестве
        /// обработанных кадров за секунду приложением
        /// </summary>
        /// <param name="parFps">Количество кадров за секунду</param>
        public abstract void ProvideAppFpsInfo(string parFps);
        
        
        /// <summary>
        /// Производит все необходимые привязки специальных обработчиков и наблюдателей
        /// к модели
        /// </summary>
        public abstract void PerformModelBindings();

        /// <summary>
        /// Получает "коллегу" для аудио менеджера
        /// </summary>
        /// <returns></returns>
        public abstract SoundManagerColleague GetSoundManagerColleague();

        /// <summary>
        /// Получает "коллегу" для посредника модели и отображения
        /// </summary>
        /// <returns></returns>
        public abstract ViewProviderColleague GetViewProviderColleague();

    }
}