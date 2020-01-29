using System;
using System.Collections.Generic;
using System.Diagnostics;
using CoreUtil.Coroutine;
using CoreUtil.Pool;
using CoreUtil.ResourceManager;
using CoreUtil.ResourceManager.AssetData.AssetDatabaseUpdateStrategies;
using CoreUtil.ResourceManager.AssetData.Builders;
using Model.SPCore.Consts;
using Model.SPCore.DS;
using Model.SPCore.GameplayModelDefinition.GameComponents.Launching;
using Model.SPCore.GameplayModelDefinition.GameComponents.ViewProviderBase;
using Model.SPCore.GameplayModelDefinition.ObjectModel;
using Model.SPCore.Managers.Serialization;
using Model.SPCore.Managers.Sound;
using Model.SPCore.Managers.Sound.Colleagues;
using Model.SPCore.MGameActions;
using Model.SPCore.MPlayers;

namespace Model.SPCore
{
    /// <summary>
    /// Главный класс, определяющий модель приложения
    /// </summary>
    public sealed class AppModel : IAppModel
    {
        /// <summary>
        /// Данные об настройках игрового процесса
        /// </summary>
        private GameplaySettingsData _appGameplaySettingsData;

        /// <summary>
        /// Данные о рекордах игроков
        /// </summary>
        private RecordsData _appRecordsData;

        /// <summary>
        /// Зарегистрированные фиксированно обновляемые объекты
        /// </summary>
        private List<IFixedUpdatable> _fixedUpdatableObjects =
            new List<IFixedUpdatable>(AppInfoConsts.FixedupdatableObjectsListStartCapacity);

        /// <summary>
        /// Зарегистрированные обновляемые объекты
        /// </summary>
        private List<IUpdatable> _updatableObjects =
            new List<IUpdatable>(AppInfoConsts.UpdatableObjectsListStartCapacity);

        /// <summary>
        /// Главный пул-менеджер приложения
        /// </summary>
        public PoolManager AppGamePool = new PoolManager();

        /// <summary>
        /// Главный менеджер ввода от игроков
        /// </summary>
        public MPlayersManager PlayersManager = new MPlayersManager();

        /// <summary>
        /// Стандартный конструктор без параметров
        /// </summary>
        public AppModel()
        {
            //загружаем основные настройки
            AppSettingsPath = AppInfoConsts.GameplaySettingsPath;
            bool settingsLoadingResult =
                AppSerializationManager.LoadDataFromFileSafe<GameplaySettingsData>(AppSettingsPath,
                    out _appGameplaySettingsData);
            if (!settingsLoadingResult)
            {
                _appGameplaySettingsData = GameplaySettingsData.GetDefaultSettings();
            }

            //настройки (даже свежезагруженные) нужно сохранить, чтобы убедится, что новые добавленные разработчиком
            //пункты также были отражены в файле
            AppSerializationManager.SaveDataToFile<GameplaySettingsData>(_appGameplaySettingsData, AppSettingsPath);


            //загружаем рекорды игроков
            bool recordsLoadingResult =
                AppSerializationManager.LoadDataFromFileSafe(AppRecordsPath, out _appRecordsData);
            if (!recordsLoadingResult)
            {
                _appRecordsData = RecordsData.GetStandardRecords();
            }

            _appRecordsData.CheckAndApplyConstraints();
            //рекорды (даже свежезагруженные) нужно сохранить, чтобы убедится, что новые добавленные разработчиком
            //дополнительные поля также были отражены в файле

            AppSerializationManager.SaveDataToFile(_appRecordsData, AppRecordsPath);
        }

        /// <summary>
        /// Конструктор с параметром, переопределяющим стандартный путь к настройкам игры
        /// </summary>
        /// <param name="parSettingsPath">Переопределенный путь к настройкам игры</param>
        public AppModel(string parSettingsPath)
        {
            AppSettingsPath = parSettingsPath;
            _appGameplaySettingsData = AppSerializationManager.LoadDataFromFile<GameplaySettingsData>(AppSettingsPath);
        }

        /// <summary>
        /// Конструктор с параметрами, переопределяющими данные о настройках игры и пути к настройкам игры
        /// </summary>
        /// <param name="parAppGameplaySettingsData"></param>
        /// <param name="parSettingsPath"></param>
        public AppModel(GameplaySettingsData parAppGameplaySettingsData, string parSettingsPath)
        {
            _appGameplaySettingsData = parAppGameplaySettingsData;
            AppSettingsPath = parSettingsPath;
        }

        /// <summary>
        /// Зарегистрировать новый обновляемый объект
        /// </summary>
        /// <param name="parUpdatableObject">Обновляемый объект для регистрации</param>
        public void RegisterUpdatableObject(IUpdatable parUpdatableObject)
        {
            _updatableObjects.Add(parUpdatableObject);
        }

        /// <summary>
        /// Зарегистрировать новый фиксированно обновляемый объект
        /// </summary>
        /// <param name="parFixedUpdatableObject">Фиксированно обновляемый объект для регистрации</param>
        public void RegisterFixedUpdatableObject(IFixedUpdatable parFixedUpdatableObject)
        {
            _fixedUpdatableObjects.Add(parFixedUpdatableObject);
        }

        /// <summary>
        /// Отменить регистрацию обновляемого объекта
        /// </summary>
        /// <param name="parUpdatableObject">Целевой обновляемый объект</param>
        public void UnregisterUpdatableObject(IUpdatable parUpdatableObject)
        {
            _updatableObjects.Remove(parUpdatableObject);
        }

        /// <summary>
        /// Отменить регистрацию фиксированно обновляемого объекта
        /// </summary>
        /// <param name="parFixedUpdatableObject">Целевой фиксированно обновляемый объект</param>
        public void UnregisterFixedUpdatableObject(IFixedUpdatable parFixedUpdatableObject)
        {
            _fixedUpdatableObjects.Remove(parFixedUpdatableObject);
        }

        /// <summary>
        /// Получить данные о текущий настройках игрового процесса
        /// </summary>
        /// <returns></returns>
        public GameplaySettingsData GetGameplaySettingsData()
        {
            return _appGameplaySettingsData;
        }

        /// <summary>
        /// Получить текущий путь к настройкам приложения
        /// </summary>
        /// <returns></returns>
        public string GetAppSettingsPath()
        {
            return AppSettingsPath;
        }

        /// <summary>
        /// Получить менеджер ввода от игроков
        /// </summary>
        /// <returns></returns>
        public MPlayersManager GetPlayersManager()
        {
            return PlayersManager;
        }

        /// <summary>
        /// Получить менеджер ресурсов
        /// </summary>
        /// <returns></returns>
        public ResourceManager GetResourcesManager()
        {
            return AppResourceManager;
        }

        /// <summary>
        /// Получить посредника для осуществления обмена сообщениями между компонентами модели и отображения
        /// </summary>
        /// <returns></returns>
        public ViewProviderMediator GetViewProviderMediator()
        {
            return AppViewProviderMediator;
        }

        /// <summary>
        /// Получить аудио менеджер приложения
        /// </summary>
        /// <returns></returns>
        public AppSoundManager GetSoundManager()
        {
            return ActualSoundManager;
        }

        /// <summary>
        /// Получить информацию о таблице рекордов игроков
        /// </summary>
        /// <returns></returns>
        public RecordsData GetRecordsData()
        {
            return AppRecordsData;
        }

        /// <summary>
        /// Получить путь сохранения данных о таблице рекордов игроков
        /// </summary>
        /// <returns></returns>
        public string GetRecordsDataPath()
        {
            return AppRecordsPath;
        }

        /// <summary>
        /// Осуществляет полный выход из приложения
        /// </summary>
        public void ExitApp()
        {
            GetSoundManager().ResetMusic();
            Environment.Exit(0);
            (Process.GetCurrentProcess()).Kill();
        }

        private void InitializeHumanPlayers()
        {
            MPlayer humanP1 = new MPlayer(AppInfoConsts.Player1DefaultName, EPlayerRoles.P1);
            InitializeStandardGameActions(humanP1);
            PlayersManager.AddPlayer(humanP1);
            PlayersManager.Player1 = humanP1;

            MPlayer humanP2 = new MPlayer(AppInfoConsts.Player2DefaultName, EPlayerRoles.P2);
            InitializeStandardGameActions(humanP2);
            PlayersManager.AddPlayer(humanP2);
            PlayersManager.Player2 = humanP2;
        }

        private void InitializeStandardGameActions(MPlayer parPlayer)
        {
            parPlayer.GameActionAxises.Clear();
            parPlayer.GameActionButtons.Clear();

            foreach (EGameActionAxis axisId in Enum.GetValues(typeof(EGameActionAxis)))
            {
                parPlayer.GameActionAxises.Add(axisId, new MGameActionAxis(AppInfoConsts.AxisDefaultValue));
            }

            foreach (EGameActionButton buttonId in Enum.GetValues(typeof(EGameActionButton)))
            {
                parPlayer.GameActionButtons.Add(buttonId, new MGameActionButton());
            }
        }

        //TODO combine/put all this data in separate data structure
        public void AppRun(AssetDataAbstractBuilder parInitialAssetBuilder,
            IAssetDatabaseUpdateStrategy parInitialAssetDatabaseUpdateStrategy,
            SoundManagerColleague parViewSoundManagerColleague, ViewProviderColleague parViewSideViewProviderColleague)
        {
            if (CurrentAppState.CurrentBaseAppState != EBaseAppStates.Created)
            {
                throw new Exception("Already launched application model cannot be launched again!");
            }

            CurrentAppState = new AppState()
            {
                CurrentBaseAppState = EBaseAppStates.Running
            };

            //initialize resources manager
            AppResourceManager = new ResourceManager(AppInfoConsts.ResourcesAssetsDirpath, parInitialAssetBuilder,
                parInitialAssetDatabaseUpdateStrategy);

            // AppResourceManager.LoadAssetPack("packTest");
            // AppResourceManager.LoadAssetPack("gfx_smd");

            //   Console.WriteLine($"{AppResourceManager.GetAssetData<AssetDataText>("packTest", "/data.txt").TextData[0]}");

            //initialize sound manager
            ActualSoundManager = new AppSoundManager(AppResourceManager, this);
            ActualSoundManager.AppSoundManagerMediator.ViewSoundManagerColleague = parViewSoundManagerColleague;
            parViewSoundManagerColleague.Mediator = ActualSoundManager.AppSoundManagerMediator;
            ActualSoundManager.UpdateLibrary();

            AppViewProviderMediator.ViewProviderViewSide = parViewSideViewProviderColleague;

            InitializeHumanPlayers();

            /*
            var testObj = AppGamePool.GetObject<GameObject>(typeof(GameObject));
            testObj.Init(this);
            Console.WriteLine($"{testObj.LinkedAppModel}");
            testObj.DisableAndSendToPool();
            Console.WriteLine($"{testObj.LinkedAppModel}");
            Console.WriteLine($"{testObj.Test}");
            */

            GameObject launchingObject = AppGamePool.GetObject<GameObject>(typeof(GameObject));
            launchingObject.Init(this);

            launchingObject.AddComponent<AppStartComponent>(AppGamePool
                .GetObject<AppStartComponent>(typeof(AppStartComponent)).Init(launchingObject));

            //var testComp = AppGamePool.GetObject<TestInputExitComponent>(typeof(TestInputExitComponent));
            //testComp.Init(testObj);

            //RegisterFixedUpdatableObject(new InputTestUpdatableObject(this));
            //new InputTestUpdatableObject(this);
            //TODO: Application startup logic
            //TODO: Settings loading 
            //TODO: App state machine definitions
            //TODO: Multithreading for physics
        }

        /// <summary>
        /// Сигнал обновления модели (для логики на каждый кадр)
        /// </summary>
        /// <param name="parDeltaTime">Время кадра в секундах</param>
        public void TickUpdate(double parDeltaTime)
        {
            List<IUpdatable> safeUpdatable = new List<IUpdatable>(_updatableObjects);

            foreach (var updatable in safeUpdatable)
            {
                if (updatable.IsActive())
                    updatable.Update(parDeltaTime);
            }


            //TODO there might be an optimization performed by events/messages
            //sound manager update tick
            ActualSoundManager.ManagerUpdateStep();
        }

        /// <summary>
        /// Сигнал окончания кадра
        /// </summary>w
        public void EndOfFrame()
        {
            CoroutineManager.Instance.ProcessOnTheEndOfFrame();
        }

        /// <summary>
        /// Предназначен для обработки физики и действий
        /// </summary>
        public void TickFixedUpdate(double parFixedDeltaTime)
        {
            List<IFixedUpdatable> safeFixedUpdatable = new List<IFixedUpdatable>(_fixedUpdatableObjects);

            foreach (var fixedUpdatable in safeFixedUpdatable)
            {
                if (fixedUpdatable.IsActive())
                    fixedUpdatable.FixedUpdate(parFixedDeltaTime);
            }
        }

        /// <summary>
        /// Получает путь к настройкам игрового процесса
        /// </summary>
        public string AppSettingsPath { get; private set; } = String.Empty;

        /// <summary>
        /// Получает данные настроек игрового процесса
        /// </summary>
        public GameplaySettingsData AppGameplaySettingsData
        {
            get { return _appGameplaySettingsData; }
            set { _appGameplaySettingsData = value; }
        }

        /// <summary>
        /// Получает данные таблицы рекордов игроков
        /// </summary>
        public RecordsData AppRecordsData
        {
            get { return _appRecordsData; }
            set { _appRecordsData = value; }
        }

        /// <summary>
        /// Получает путь к файлу с данными таблицы рекордов игроков
        /// </summary>
        public string AppRecordsPath { get; private set; } = AppInfoConsts.RecordsDataPath;

        /// <summary>
        /// Получает менеджер ресурсов приложениея
        /// </summary>
        public ResourceManager AppResourceManager { get; private set; }

        /// <summary>
        /// Получает аудио менеджер приложения
        /// </summary>
        public AppSoundManager ActualSoundManager { get; private set; }

        public ViewProviderMediator AppViewProviderMediator { get; private set; } = new ViewProviderMediator();

        /// <summary>
        /// Текущее общее системное состояние приложения
        /// </summary>
        public AppState CurrentAppState { get; private set; } =
            new AppState() {CurrentBaseAppState = EBaseAppStates.Created};
    }
}