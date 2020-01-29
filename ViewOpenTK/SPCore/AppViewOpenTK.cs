using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Model.SPCore;
using Model.SPCore.Consts;
using Model.SPCore.DS;
using Model.SPCore.GameplayModelDefinition.GameComponents.ViewProviderBase;
using Model.SPCore.Managers.Serialization;
using Model.SPCore.Managers.Sound.Colleagues;
using View.SPCore;
using OpenTK;
using ViewOpenTK.OpenAL;
using ViewOpenTK.OpenGL;
using ViewOpenTK.OpenTKInput;
using ViewOpenTK.SPCore.DS;
using ViewOpenTK.SPCore.ViewProvider;
using ViewOpenTK.SPCore.ViewProvider.RenderingData;

namespace ViewOpenTK.SPCore
{
  /// <summary>
  /// Реализация компонента отображения приложения MVC OpenTK
  /// </summary>
  public sealed class AppViewOpenTk : AppViewBase
  {
    /// <summary>
    /// Название окна
    /// </summary>
    private static readonly string WindowName = $"{AppInfoConsts.AppName} {AppInfoConsts.Version}";

    /// <summary>
    /// Текущее окно OpenTK
    /// </summary>
    private GlAppWindow _currentGameWindow;

    /// <summary>
    /// "Сотрудник" аудио менеджера со стороны OpenTK
    /// </summary>
    private OpenAlSoundManagerColleague _actualOpenAlSoundManagerColleague = new OpenAlSoundManagerColleague();

    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parApp">Прикрепляемая модель приложения</param>
    /// <param name="parGraphicalSettingsPath">Путь к настройкам отображения</param>
    public AppViewOpenTk(AppModel parApp, string parGraphicalSettingsPath) : base(parApp, parGraphicalSettingsPath)
    {
    }

    /// <summary>
    /// События изменения последней нажатой текстовой кнопки на клавиатуре
    /// </summary>
    public event Action<string> KeyboardLetterKeyPressed;


    /// <summary>
    /// Вызов события изменения последней нажатой текстовой кнопки на клавиатуре
    /// </summary>
    /// <param name="parLetterKey">Данные нажатой текстовой кнопки на клавиатуре</param>
    public void InvokeLetterKeyPressed(string parLetterKey)
    {
      KeyboardLetterKeyPressed?.Invoke(parLetterKey);
    }

    /// <summary>
    /// Сигнал об обновлении предоставленной информации о количестве
    /// обработанных кадров за секунду приложением
    /// </summary>
    /// <param name="parFps">Количество кадров за секунду</param>
    public override void ProvideAppFpsInfo(string parFps)
    {
      _currentGameWindow.Title = $"Side Pocket {parFps}";
    }

    /// <summary>
    /// Запрос состояния устройства
    /// </summary>
    /// <param name="parRequest">Объект запроса</param>
    public void RequestDeviceState(StateRequest parRequest)
    {
      lock (InputSyncObj)
      {
        StateRequests.Enqueue(parRequest);
      }
    }

    /// <summary>
    /// Осуществляет загрузку настроек отображения
    /// </summary>
    public override void LoadSettings()
    {
      if (!File.Exists(GraphicalSettingsPath))
      {
        CreateDefaultGraphicalSettings();
      }

      var loaded = AppSerializationManager.LoadDataFromFile<GraphicsSettingsDataOpenTk>(GraphicalSettingsPath);
      CurrentGraphicsSettings = loaded;
      AppSerializationManager.SaveDataToFile(CurrentGraphicsSettings, GraphicalSettingsPath);
    }

    /// <summary>
    /// Осуществляет первоначальную инициализацию
    /// </summary>
    public override void InitializeView()
    {
      ViewReset();
      OpenGlWindowDisplay.Instance.UpdateViewport(ViewBehaviourConsts.BASE_SURFACE_WIDTH,
        ViewBehaviourConsts.BASE_SURFACE_HEIGHT);
      UpdateViewport();
    }

    /// <summary>
    /// Применяет установленные настройки графики
    /// </summary>
    public override void ApplyGraphicsSettings()
    {
      ViewReset();
    }

    /// <summary>
    /// Создает файл со стандартными настройками отображения
    /// </summary>
    public override void CreateDefaultGraphicalSettings()
    {
      GraphicsSettingsDataOpenTk graphicsSettingsDataOpenTk = new GraphicsSettingsDataOpenTk();
      Console.WriteLine(graphicsSettingsDataOpenTk.SettingScreenResolution.Width);
      CurrentGraphicsSettings = graphicsSettingsDataOpenTk;
      //сразу же сериализуем их
      AppSerializationManager.SaveDataToFile<GraphicsSettingsDataOpenTk>(graphicsSettingsDataOpenTk,
        GraphicalSettingsPath);
    }

    /// <summary>
    /// Производит очистку отображения
    /// </summary>
    public override void CleanView()
    {
    }

    /// <summary>
    /// Передает команду на рендеринг отображения модели
    /// </summary>
    public override void Render()
    {
      lock (_currentGameWindow.NextRenderTaskLock)
      {
        if (ActualViewEventsHandler.RendereringDataWasUpdatedFlag)
        {
          ActualViewEventsHandler.RendereringDataWasUpdatedFlag = false;

          //сортировка по глубине
          List<RenderingData?> renderingData =
            ActualViewEventsHandler.GetRenderingData();
          renderingData.RemoveAll(parX => parX == null);

          renderingData = renderingData.OrderByDescending(parX => parX.Value.Depth).ToList();


          foreach (var renderTaskDescr in renderingData)
          {
            foreach (var renderTask in renderTaskDescr.Value.RenderingTasks)
            {
              _currentGameWindow.AddTask(renderTask);
            }
          }
        }
      }

      while (!_currentGameWindow.RenderReady)
      {
      }

      //необходимо также дождаться обновления
      _currentGameWindow.WaitingForUpdateFlag = false;

      while (!_currentGameWindow.WaitingForUpdateFlag)
      {
      }
    }

    /// <summary>
    /// Полностью сбрасывает компонент вида MVC
    /// </summary>
    public override void ViewReset()
    {
      if (_currentGameWindow != null)
      {
        while (!_currentGameWindow.AtLeastOneRenderOpPerformed)
        {
        }
      }

      _currentGameWindow?.Close();
      _currentGameWindow?.Dispose();
      ViewAvailable = false;
    }

    /// <summary>
    /// Обновляет дисплей и окно приложения, в случае необходимости создает новое окно
    /// </summary>
    private void UpdateViewport()
    {
      while (App.CurrentAppState.CurrentBaseAppState == EBaseAppStates.Running)
      {
        ViewAvailable = false;
        _currentGameWindow?.Dispose();

        GraphicsSettingsDataOpenTk graphicsCastRef = (GraphicsSettingsDataOpenTk) CurrentGraphicsSettings;
        GameWindowFlags gwFlags =
          graphicsCastRef.IsFullscreen ? GameWindowFlags.Fullscreen : GameWindowFlags.FixedWindow;


        _currentGameWindow = new GlAppWindow(graphicsCastRef.SettingScreenResolution.Width,
          graphicsCastRef.SettingScreenResolution.Height,
          WindowName, gwFlags, this);
        _currentGameWindow.VSync = VSyncMode.Off;

        _currentGameWindow.Run();
      }
    }

    /// <summary>
    /// Производит все необходимые привязки специальных обработчиков и наблюдателей
    /// к модели
    /// </summary>
    public override void PerformModelBindings()
    {
      ActualViewEventsHandler = new ViewEventsOpenTkHandler(
        new ViewBehaviourOpenTkHandlersLinker(ViewBehaviourConsts.StandardViewProviderComponentsModelToView),
        App, this);
    }

    /// <summary>
    /// Получает "коллегу" для аудио менеджера
    /// </summary>
    /// <returns></returns>
    public override SoundManagerColleague GetSoundManagerColleague()
    {
      return _actualOpenAlSoundManagerColleague;
    }

    /// <summary>
    /// Получает "коллегу" для посредника модели и отображения
    /// </summary>
    /// <returns></returns>
    public override ViewProviderColleague GetViewProviderColleague()
    {
      return ActualViewEventsHandler.ViewSideProviderColleague;
    }

    /// <summary>
    /// Сигнал обновления компонентов вида
    /// </summary>
    /// <param name="parDeltaTime">Время кадра</param>
    public override void ViewUpdateSignal(double parDeltaTime)
    {
      ActualViewEventsHandler.ViewUpdate(parDeltaTime);
    }

    /// <summary>
    /// Очередь запросов состояний устройств
    /// </summary>
    public ConcurrentQueue<StateRequest> StateRequests { get; private set; } = new ConcurrentQueue<StateRequest>();

    /// <summary>
    /// Обработчик данных для рендеринга и событий OpenTK
    /// </summary>
    public ViewEventsOpenTkHandler ActualViewEventsHandler { get; private set; }
  }
}