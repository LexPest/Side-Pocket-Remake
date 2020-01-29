using System;
using Model.SPCore;
using Model.SPCore.DS;
using View.SPCore;
using System.Diagnostics;
using System.Threading;
using Controller.MInput.MPlayerBinds;
using CoreUtil.ResourceManager.AssetData.AssetDatabaseUpdateStrategies;
using CoreUtil.ResourceManager.AssetData.Builders;
using CoreUtil.Time;

namespace Controller.SPCore
{
  /// <summary>
  /// Базовый класс главного контроллера приложения
  /// </summary>
  public abstract class AppControllerBase
  {
    /// <summary>
    /// Поток для главного цикла приложения
    /// </summary>
    private Thread _applicationMainThread;

    /// <summary>
    /// Конструктор по умолчанию
    /// </summary>
    /// <param name="parApp">Модель приложения для привязки</param>
    public AppControllerBase(AppModel parApp)
    {
      App = parApp;
    }

    /// <summary>
    /// Наблюдатель за привязками игроков в модели к контроллеру
    /// </summary>
    protected MPlayerBindsControllerWatchdog PlayerBindsControllerWatchdog { get; set; }

    /// <summary>
    /// Целевое приложение-модель
    /// </summary>
    public AppModel App { get; private set; }

    /// <summary>
    /// Целевое отображение-вид
    /// </summary>
    public AppViewBase AppView { get; protected set; }

    /// <summary>
    /// Получает строитель для обработки игровых ассетов и ресурсов
    /// </summary>
    /// <returns></returns>
    protected abstract AssetDataAbstractBuilder GetConcreteAssetDataBuilder();

    /// <summary>
    /// Получает стратегию для построения базы данных игровых ассетов и ресурсов
    /// </summary>
    /// <returns></returns>
    protected abstract IAssetDatabaseUpdateStrategy GetConcreteAssetDatabaseUpdateStrategy();


    /// <summary>
    /// Запускает исполнение экзепляра приложения
    /// </summary>
    /// <exception cref="ApplicationException">Компонент вида должен быть обязательно объявлен</exception>
    public void Run()
    {
      if (AppView == null)
      {
        throw new ApplicationException("View has not been defined for the application controller!");
      }

      //Осуществление привязок контроллера
      PerformModelBindings();

      //Осуществление привязок вида
      AppView.PerformModelBindings();

      //Старт приложения
      App.AppRun(GetConcreteAssetDataBuilder(), GetConcreteAssetDatabaseUpdateStrategy(),
        AppView.GetSoundManagerColleague(), AppView.GetViewProviderColleague());

      _applicationMainThread = new Thread(UpdateLoop);

      _applicationMainThread.Start();

      //Базовая инициализация

      AppView.InitializeView();
    }

    /// <summary>
    /// Осуществление всех необходимых привязок контроллера к модели
    /// </summary>
    protected virtual void PerformModelBindings()
    {
      //Старт работы наблюдателя за игроками
      PlayerBindsControllerWatchdog = new MPlayerBindsControllerWatchdog(App);
    }

    /// <summary>
    /// Запускает исполнение главного цикла приложения
    /// </summary>
    private void UpdateLoop()
    {
      while (!AppView.ViewAvailable)
      {
      }

      AppView.CleanView();
      Stopwatch timeWatch = new Stopwatch();
      long frametime = 0;
      long currentLag = 0;
      long currentFrametimeSpent = 0;
      long fixedAppTimestamp = (long) Math.Round(App.AppGameplaySettingsData.AppFixedTimestamp * 1000);
      long fixedRenderFrameTimestamp =
        (long) Math.Round(AppView.CurrentGraphicsSettings.FramerateTimestamp * 1000);
      while (App.CurrentAppState.CurrentBaseAppState == EBaseAppStates.Running)
      {
        if (AppView.ViewAvailable)
        {
          frametime = timeWatch.ElapsedMilliseconds;

          timeWatch.Restart();
          currentLag += frametime;

          while (currentLag >= fixedAppTimestamp)
          {
            lock (AppView.InputSyncObj)
            {
              InputHandle(frametime);

              ProcessPlayersInput();
              ModelFixedUpdate(fixedAppTimestamp);
            }

            currentLag -= fixedAppTimestamp;
          }

          ModelUpdate(frametime);
          ViewUpdate(frametime);
          Render();
          ModelEndOfFrame();

          currentFrametimeSpent = timeWatch.ElapsedMilliseconds;


          if (currentFrametimeSpent < fixedRenderFrameTimestamp)
          {
            Thread.Sleep((int) (fixedRenderFrameTimestamp - currentFrametimeSpent));
          }
        }
      }
    }

    /// <summary>
    /// Обработка ввода на стороне контроллера
    /// </summary>
    /// <param name="parDeltaTime">Время кадра обработки устройств в миллисекундах</param>
    protected abstract void InputHandle(double parDeltaTime);

    /// <summary>
    /// Вызов обработки ввода игроков
    /// </summary>
    private void ProcessPlayersInput()
    {
      foreach (MPlayerController playerController in PlayerBindsControllerWatchdog.PlayerControllers)
      {
        playerController.UpdateInput();
      }
    }

    /// <summary>
    /// Вызов обновления модели
    /// </summary>
    /// <param name="parDeltaTime">Величина времени кадра в миллисекундах</param>
    private void ModelUpdate(long parDeltaTime)
    {
      App.TickUpdate(Time.MillisecondsToSeconds(parDeltaTime));
    }

    /// <summary>
    /// Вызов фиксированного обновления модели (происходит определенное заданное количество раз в секунду)
    /// </summary>
    /// <param name="parFixedDeltaTime">Величина времени фиксированного обновления в миллисекундах</param>
    private void ModelFixedUpdate(long parFixedDeltaTime)
    {
      App.TickFixedUpdate(Time.MillisecondsToSeconds(parFixedDeltaTime));
    }

    /// <summary>
    /// Вызов обновления вида
    /// </summary>
    /// <param name="parDeltaTime">Величина времени кадра в миллисекундах</param>
    private void ViewUpdate(long parDeltaTime)
    {
      AppView.ViewUpdateSignal(Time.MillisecondsToSeconds(parDeltaTime));
    }

    /// <summary>
    /// Вызов отрисовки модели
    /// </summary>
    protected abstract void Render();

    /// <summary>
    /// Вызов, оповещающий об окончании кадра модель
    /// </summary>
    protected void ModelEndOfFrame()
    {
      App.EndOfFrame();
    }
  }
}