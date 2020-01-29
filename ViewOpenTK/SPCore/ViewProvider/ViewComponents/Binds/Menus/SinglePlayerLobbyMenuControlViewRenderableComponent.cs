using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using CoreUtil.Coroutine;
using CoreUtil.Pool;
using Model.SPCore.GameplayModelDefinition.GameComponents.Menus;
using Model.SPCore.GameplayModelDefinition.GameComponents.Menus.MainMenu;
using Model.SPCore.GameplayModelDefinition.GameComponents.ViewProviderBase;
using ViewOpenTK.SPCore.ViewProvider.InternalGraphicalDataStructures;
using ViewOpenTK.SPCore.ViewProvider.RenderingData;

namespace ViewOpenTK.SPCore.ViewProvider.ViewComponents.Binds.Menus
{
  /// <summary>
  /// Компонент для рендеринга данных подменю выбора параметров одиночной игры
  /// </summary>
  public class SinglePlayerLobbyMenuControlComponentViewRenderableComponent : ViewRenderableComponent
  {
    /// <summary>
    /// Название спрайта заднего фона
    /// </summary>
    public const string LOBBY_MENU_BG = "/stage_start_screen/spr_stage_start_screen.png";

    /// <summary>
    /// Позиция X для кнопок
    /// </summary>
    private const int BUTTONS_MIDDLE_X = 226;
    
    /// <summary>
    /// Позиция Y для кнопки выбора уровня
    /// </summary>
    private const int BUTTON_LEVEL_SELECT_MIDDLE_Y = 80;
    /// <summary>
    /// Позиция Y для кнопки возврата назад
    /// </summary>
    private const int BUTTON_RETURN_BACK_MIDDLE_Y = 104;
    
    /// <summary>
    /// Данные рендеринга анимации черного экрана
    /// </summary>
    private RenderingData.RenderingData? _currentBlackScreen;

    /// <summary>
    /// Данные для рендеринга элементов меню
    /// </summary>
    private List<RenderingData.RenderingData?> _currentMenuElements;

    /// <summary>
    /// Компонент на стороне модели
    /// </summary>
    private SinglePlayerLobbyMenuControlComponent _modelProviderComponent;

    /// <summary>
    /// Конструктор для поддержки системы пулинга объектов
    /// </summary>
    /// <param name="parSupportData">Данные для работы с пулом</param>
    public SinglePlayerLobbyMenuControlComponentViewRenderableComponent(ObjectPoolSupportData parSupportData) : base(
      parSupportData)
    {
    }

    /// <summary>
    /// Является ли компонент автоматически обновляемым?
    /// </summary>
    public override bool IsUpdatable()
    {
      return false;
    }

    /// <summary>
    /// Замена конструктора, процедура инициализации компонента отображения
    /// </summary>
    /// <param name="parModelSideProviderComponent">Связанный компонент на стороне модели</param>
    /// <param name="parLinkedEventsHandler">Связанный обработчик событий отображения</param>
    public override void InitAndLink(ViewProviderComponent parModelSideProviderComponent,
      ViewEventsOpenTkHandler parLinkedEventsHandler)
    {
      base.InitAndLink(parModelSideProviderComponent, parLinkedEventsHandler);

      _modelProviderComponent = (SinglePlayerLobbyMenuControlComponent) parModelSideProviderComponent;

      _modelProviderComponent.OnBecomeActive += OnActivatedPerform;
      _modelProviderComponent.OnDeactivated += OnDeactivatedPerform;
      _modelProviderComponent.TransitionToTheGameStageStarted += ProcessTransitionToTheGameStageViaBlackScreen;
    }

    /// <summary>
    /// Действия при активации меню
    /// </summary>
    private void OnActivatedPerform()
    {
      ProcessUiElementsList();
      Console.WriteLine("Single Player Lobby activated");
    }

    /// <summary>
    /// Действия при деактвации меню
    /// </summary>
    private void OnDeactivatedPerform()
    {
      _currentMenuElements = null;
      Console.WriteLine("Single Player Lobby disabled");
    }

    /// <summary>
    /// Обновление данных компонента отображения
    /// </summary>
    /// <param name="parDeltaTime">Время кадра</param>
    /// <returns>Флаг необходимости перерисовки</returns>
    public override bool ViewUpdate(double parDeltaTime)
    {
      return false;
    }

    /// <summary>
    /// Обработка состояния элементов интерфейса
    /// </summary>
    private void ProcessUiElementsList()
    {
      //добавить фон
      _currentMenuElements = new List<RenderingData.RenderingData?>();

      _currentMenuElements.Add(new RenderingData.RenderingData(
        new RenderingSprite(ActualSubassetsDataLibrary.GetSprite(LOBBY_MENU_BG), 0, 0, 0, Color.White, 0)));


      //добавить кнопки
      void ProcessButton(UiElementButton parButton, int parX, int parY, string parCaption)
      {
        string chosenFontKey = parButton.IsHovered
          ? ViewBehaviourConsts.DEFAULT_APP_FONT
          : ViewBehaviourConsts
            .MENU_DARK_INACTIVE_APP_FONT;

        _currentMenuElements.Add(new RenderingData.RenderingData(new RenderingString(-1, parX, parY,
          parCaption, ActualSubassetsDataLibrary.GetFont(chosenFontKey), Color.White, 1, 1,
          EHorizontalAlign.Middle, EVerticalAlign.Middle)));
      }

      ProcessButton(_modelProviderComponent.SelectStartLevelButton, BUTTONS_MIDDLE_X, BUTTON_LEVEL_SELECT_MIDDLE_Y,
        _modelProviderComponent.CurrentSelectedLevel.LevelName);

      ProcessButton(_modelProviderComponent.ReturnBackButton, BUTTONS_MIDDLE_X, BUTTON_RETURN_BACK_MIDDLE_Y,
        "Back");
    }

    /// <summary>
    /// Начать воспроизведение анимации начала игры через затемнение
    /// </summary>
    private void ProcessTransitionToTheGameStageViaBlackScreen()
    {
      CoroutineManager.Instance.StartCoroutine(BlackScreenOnAnimation(() =>
      {
        _modelProviderComponent.ViewNotifiesThatTransitionEnded();
      }));
    }

    /// <summary>
    /// Короутина анимации затемнения
    /// </summary>
    /// <param name="parAfterBlackScreenPerform">Действия после затемнения</param>
    /// <returns></returns>
    private IEnumerator BlackScreenOnAnimation(Action parAfterBlackScreenPerform)
    {
      double currentTime = 0.0;
      Stopwatch watch = new Stopwatch();
      watch.Start();
      while (currentTime < MainMenuControlComponentViewRenderableComponent.BLACK_SCREEN_APPEAR_TIME)
      {
        double div = currentTime / MainMenuControlComponentViewRenderableComponent.BLACK_SCREEN_APPEAR_TIME;
        _currentBlackScreen =
          new RenderingData.RenderingData(
            new RenderingFillColorScreen(-100, Color.FromArgb((int) (div * 255), 0, 0, 0)));
        currentTime = (double) watch.ElapsedMilliseconds / 1000;
        UpdateRenderingData();
        LinkedViewEventsHandler.RendereringDataWasUpdatedFlag = true;
        yield return null;
      }

      parAfterBlackScreenPerform?.Invoke();
    }

    /// <summary>
    /// Обновить данные для рендеринга
    /// </summary>
    public override void UpdateRenderingData()
    {
      if (_currentMenuElements != null)
      {
        ProcessUiElementsList();
      }
    }

    /// <summary>
    /// Получить данные для рендеринга
    /// </summary>
    public override List<RenderingData.RenderingData?> GetRenderingData()
    {
      if (_modelProviderComponent.IsActive)
      {
        List<RenderingData.RenderingData?> retList = new List<RenderingData.RenderingData?>();
        if (_currentMenuElements != null)
          retList.AddRange(_currentMenuElements);
        if (_currentBlackScreen != null)
        {
          retList.Add(_currentBlackScreen);
        }

        return retList;
      }
      else
      {
        return null;
      }
    }

    /// <summary>
    /// Выполнить действия перед отправкой в пул
    /// </summary>
    public override void OnBeforeResetPerform()
    {
      base.OnBeforeResetPerform();

      if (_modelProviderComponent != null)
      {
        _modelProviderComponent.OnBecomeActive -= OnActivatedPerform;
        _modelProviderComponent.OnDeactivated -= OnDeactivatedPerform;
      }
    }
  }
}