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
  /// Компонент для рендеринга данных главного меню
  /// </summary>
  public class MainMenuControlComponentViewRenderableComponent : ViewRenderableComponent
  {
    /// <summary>
    /// Время анимации черного экрана
    /// </summary>
    public const double BLACK_SCREEN_APPEAR_TIME = 1.0;
    /// <summary>
    /// Название спрайта заднего фона главного меню
    /// </summary>
    public const string MAIN_MENU_BG = "/main_menu_screen/spr_main_menu.png";

    /// <summary>
    /// Позиция X для кнопок
    /// </summary>
    public const int X_CENTERED_OFFSET_MENU = 240;
    
    /// <summary>
    /// Стартовая позиция Y для кнопок
    /// </summary>
    public const int Y_OFFSET_MENU_START = 42;

    /// <summary>
    /// Позиция смещения по Y для кнопок
    /// </summary>
    public const int Y_OFFSET_MENU_ELEMENTS = 14;

    /// <summary>
    /// Данные для рендеринга черного экрана
    /// </summary>
    private RenderingData.RenderingData? _currentBlackScreen;

    /// <summary>
    /// Данные для рендеринга элементов меню
    /// </summary>
    private List<RenderingData.RenderingData?> _currentMenuElements;

    /// <summary>
    /// Компонент на стороне модели
    /// </summary>
    private MainMenuControlComponent _modelProviderComponent;

    /// <summary>
    /// Флаг-признак, что запрошено обновление анимацией черного экрана
    /// </summary>
    private bool _updateByBlackScreenAnimRequested = false;

    /// <summary>
    /// Конструктор для поддержки системы пулинга объектов
    /// </summary>
    /// <param name="parSupportData">Данные для работы с пулом</param>
    public MainMenuControlComponentViewRenderableComponent(ObjectPoolSupportData parSupportData) : base(parSupportData)
    {
    }

    /// <summary>
    /// Является ли компонент автоматически обновляемым?
    /// </summary>
    public override bool IsUpdatable()
    {
      return true;
    }

    /// <summary>
    /// Замена конструктора, процедура инициализации компонента отображения
    /// </summary>
    /// <param name="parModelSideProviderComponent">Связанный компонент на стороне модели</param>
    /// <param name="parLinkedEventsHandler">Связанный обработчик событий отображения</param>
    public override void InitAndLink(
      ViewProviderComponent parModelSideProviderComponent, ViewEventsOpenTkHandler parLinkedEventsHandler)
    {
      base.InitAndLink(parModelSideProviderComponent, parLinkedEventsHandler);
      _modelProviderComponent = (MainMenuControlComponent) parModelSideProviderComponent;

      _modelProviderComponent.OnBecomeActive += RunAppearFromBlackScreen;
      _modelProviderComponent.OnBecomeActive += OnActivatedPerform;
      _modelProviderComponent.OnDeactivated += OnDeactivatedPerform;
    }

    /// <summary>
    /// Запуск анимации появления меню из черного экрана
    /// </summary>
    private void RunAppearFromBlackScreen()
    {
      _modelProviderComponent.OnBecomeActive -= RunAppearFromBlackScreen;
      _currentBlackScreen =
        new RenderingData.RenderingData(new RenderingFillColorScreen(-100, Color.FromArgb((int) 255, 0, 0, 0)));
      CoroutineManager.Instance.StartCoroutine(AppearAnimation());
    }

    /// <summary>
    /// Короутина анимации появления меню из черного экрана
    /// </summary>
    /// <returns></returns>
    private IEnumerator AppearAnimation()
    {
      double currentTime = BLACK_SCREEN_APPEAR_TIME;
      Stopwatch watch = new Stopwatch();
      watch.Start();
      while (currentTime > 0.0)
      {
        double div = currentTime / BLACK_SCREEN_APPEAR_TIME;
        _currentBlackScreen =
          new RenderingData.RenderingData(
            new RenderingFillColorScreen(-100, Color.FromArgb((int) (div * 255), 0, 0, 0)));
        currentTime = BLACK_SCREEN_APPEAR_TIME - (double) watch.ElapsedMilliseconds / 1000;
        UpdateRenderingData();
        _updateByBlackScreenAnimRequested = true;
        yield return null;
      }

      _currentBlackScreen = null;
    }

    /// <summary>
    /// Действия при активации меню
    /// </summary>
    private void OnActivatedPerform()
    {
      ProcessUiElementsList();
      Console.WriteLine("Menu activated");
    }

    /// <summary>
    /// Действия при деактвации меню
    /// </summary>
    private void OnDeactivatedPerform()
    {
      _currentMenuElements = null;
      Console.WriteLine("Menu disabled");
    }

    /// <summary>
    /// Обновление данных компонента отображения
    /// </summary>
    /// <param name="parDeltaTime">Время кадра</param>
    /// <returns>Флаг необходимости перерисовки</returns>
    public override bool ViewUpdate(double parDeltaTime)
    {
      if (_updateByBlackScreenAnimRequested)
      {
        _updateByBlackScreenAnimRequested = false;
        return true;
      }

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
        new RenderingSprite(ActualSubassetsDataLibrary.GetSprite(MAIN_MENU_BG), 0, 0, 0, Color.White, 1)));


      //добавить кнопки
      int currentMenuY = Y_OFFSET_MENU_START;
      if (_modelProviderComponent.MenuUiElements != null)
      {
        if (_modelProviderComponent.MenuUiElements.Count > 0)
        {
          void ProcessButton(UiElementButton parButton, int parY, string parCaption)
          {
            string chosenFontKey = parButton.IsHovered
              ? ViewBehaviourConsts.DEFAULT_APP_FONT
              : ViewBehaviourConsts
                .MENU_DARK_INACTIVE_APP_FONT;

            _currentMenuElements.Add(new RenderingData.RenderingData(new RenderingString(-1, X_CENTERED_OFFSET_MENU, parY,
              parCaption, ActualSubassetsDataLibrary.GetFont(chosenFontKey), Color.White, 1, 1,
              EHorizontalAlign.Middle, EVerticalAlign.Top)));
          }

          ProcessButton(_modelProviderComponent.NewGame1PPocketButton, currentMenuY, "1P Pocket Game");

          currentMenuY += Y_OFFSET_MENU_ELEMENTS;

          ProcessButton(_modelProviderComponent.OptionsButton, currentMenuY, "Options");

          currentMenuY += Y_OFFSET_MENU_ELEMENTS;

          ProcessButton(_modelProviderComponent.RecordsButton, currentMenuY, "Records");

          currentMenuY += Y_OFFSET_MENU_ELEMENTS;

          ProcessButton(_modelProviderComponent.JukeboxButton, currentMenuY, "Jukebox");

          currentMenuY += Y_OFFSET_MENU_ELEMENTS;

          ProcessButton(_modelProviderComponent.ExitButton, currentMenuY, "Exit");

          currentMenuY += Y_OFFSET_MENU_ELEMENTS;
        }
      }
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
        if (_currentBlackScreen != null)
          retList.Add(_currentBlackScreen);
        if (_currentMenuElements != null)
          retList.AddRange(_currentMenuElements);
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