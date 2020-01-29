using System;
using System.Collections.Generic;
using System.Drawing;
using CoreUtil.Pool;
using Model.SPCore.GameplayModelDefinition.GameComponents.Menus;
using Model.SPCore.GameplayModelDefinition.GameComponents.Menus.MainMenu;
using Model.SPCore.GameplayModelDefinition.GameComponents.ViewProviderBase;
using Model.SPCore.Managers.Serialization;
using ViewOpenTK.SPCore.DS;
using ViewOpenTK.SPCore.ViewProvider.InternalGraphicalDataStructures;
using ViewOpenTK.SPCore.ViewProvider.RenderingData;

namespace ViewOpenTK.SPCore.ViewProvider.ViewComponents.Binds.Menus
{
  /// <summary>
  /// Компонент для рендеринга данных меню настроек
  /// </summary>
  public class OptionsMenuControlComponentViewRenderableComponent : ViewRenderableComponent
  {
    /// <summary>
    /// Позиция для смещения меню по левому краю по X
    /// </summary>
    public const int MENU_LEFT_SIDE_X_OFFSET = 190;

    /// <summary>
    /// Позиция для смещения меню по правому краю по X
    /// </summary>
    public const int MENU_RIGHT_SIDE_X_OFFSET = 290;

    /// <summary>
    /// Данные для рендеринга элементов меню
    /// </summary>
    private List<RenderingData.RenderingData?> _currentMenuElements;

    /// <summary>
    /// Компонент на стороне модели
    /// </summary>
    private OptionsMenuControlComponent _modelProviderComponent;

    /// <summary>
    /// Конструктор для поддержки системы пулинга объектов
    /// </summary>
    /// <param name="parSupportData">Данные для работы с пулом</param>
    public OptionsMenuControlComponentViewRenderableComponent(ObjectPoolSupportData parSupportData) : base(parSupportData)
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
    public override void InitAndLink(
      ViewProviderComponent parModelSideProviderComponent, ViewEventsOpenTkHandler parLinkedEventsHandler)
    {
      base.InitAndLink(parModelSideProviderComponent, parLinkedEventsHandler);
      _modelProviderComponent = (OptionsMenuControlComponent) parModelSideProviderComponent;

      _modelProviderComponent.OnBecomeActive += OnActivatedPerform;
      _modelProviderComponent.OnDeactivated += OnDeactivatedPerform;
    }

    /// <summary>
    /// Действия при активации меню
    /// </summary>
    private void OnActivatedPerform()
    {
      _modelProviderComponent.OnNewScreenModeSet += OnScreenModeChanged;
      ProcessUiElementsList();
      Console.WriteLine("Options Menu activated");
    }

    /// <summary>
    /// Действия при деактвации меню
    /// </summary>
    private void OnDeactivatedPerform()
    {
      _modelProviderComponent.OnNewScreenModeSet -= OnScreenModeChanged;
      _currentMenuElements = null;
      Console.WriteLine("Options Menu disabled");
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
        new RenderingSprite(
          ActualSubassetsDataLibrary.GetSprite(MainMenuControlComponentViewRenderableComponent.MAIN_MENU_BG),
          0, 0, 0, Color.White, 1)));

      //добавить кнопки опций с текущими значениями
      int currentMenuY = MainMenuControlComponentViewRenderableComponent.Y_OFFSET_MENU_START;

      if (_modelProviderComponent.MenuUiElements != null)
      {
        string ChooseFontKey(UiElementButton parButton)
        {
          return parButton.IsHovered
            ? ViewBehaviourConsts.DEFAULT_APP_FONT
            : ViewBehaviourConsts
              .MENU_DARK_INACTIVE_APP_FONT;
        }


        //опция режима отображения
        UiElementButton targetButton = _modelProviderComponent.ScreenModeButton;
        string chosenFontKey = ChooseFontKey(targetButton);

        _currentMenuElements.Add(new RenderingData.RenderingData(new RenderingString(-1, MENU_LEFT_SIDE_X_OFFSET,
          currentMenuY,
          "Windowed", ActualSubassetsDataLibrary.GetFont(chosenFontKey), Color.White, 1, 1,
          EHorizontalAlign.Left, EVerticalAlign.Top)));

        var graphicsSettings =
          (GraphicsSettingsDataOpenTk) LinkedViewEventsHandler.LinkedView.CurrentGraphicsSettings;
        bool isFullscreenEnabled = graphicsSettings.IsFullscreen;

        string valueCaption = !isFullscreenEnabled ? "Yes" : "No";

        _currentMenuElements.Add(new RenderingData.RenderingData(new RenderingString(-1, MENU_RIGHT_SIDE_X_OFFSET,
          currentMenuY,
          valueCaption, ActualSubassetsDataLibrary.GetFont(chosenFontKey), Color.White, 1, 1,
          EHorizontalAlign.Right, EVerticalAlign.Top)));

        currentMenuY += MainMenuControlComponentViewRenderableComponent.Y_OFFSET_MENU_ELEMENTS;

        //опция режима музыки
        targetButton = _modelProviderComponent.MusicModeButton;
        chosenFontKey = ChooseFontKey(targetButton);

        _currentMenuElements.Add(new RenderingData.RenderingData(new RenderingString(-1, MENU_LEFT_SIDE_X_OFFSET,
          currentMenuY,
          "Music", ActualSubassetsDataLibrary.GetFont(chosenFontKey), Color.White, 1, 1,
          EHorizontalAlign.Left, EVerticalAlign.Top)));

        var gameSettings = _modelProviderComponent.ParentGameObject.LinkedAppModel.GetGameplaySettingsData();
        bool isMusicEnabled = gameSettings.IsMusicEnabled;

        valueCaption = isMusicEnabled ? "Yes" : "No";

        _currentMenuElements.Add(new RenderingData.RenderingData(new RenderingString(-1, MENU_RIGHT_SIDE_X_OFFSET,
          currentMenuY,
          valueCaption, ActualSubassetsDataLibrary.GetFont(chosenFontKey), Color.White, 1, 1,
          EHorizontalAlign.Right, EVerticalAlign.Top)));

        currentMenuY += MainMenuControlComponentViewRenderableComponent.Y_OFFSET_MENU_ELEMENTS;

        //опция режима звуковых эффектов
        targetButton = _modelProviderComponent.SfxModeButton;
        chosenFontKey = ChooseFontKey(targetButton);

        _currentMenuElements.Add(new RenderingData.RenderingData(new RenderingString(-1, MENU_LEFT_SIDE_X_OFFSET,
          currentMenuY,
          "Sound", ActualSubassetsDataLibrary.GetFont(chosenFontKey), Color.White, 1, 1,
          EHorizontalAlign.Left, EVerticalAlign.Top)));

        bool isSfxEnabled = gameSettings.IsSfxEnabled;

        valueCaption = isSfxEnabled ? "Yes" : "No";

        _currentMenuElements.Add(new RenderingData.RenderingData(new RenderingString(-1, MENU_RIGHT_SIDE_X_OFFSET,
          currentMenuY,
          valueCaption, ActualSubassetsDataLibrary.GetFont(chosenFontKey), Color.White, 1, 1,
          EHorizontalAlign.Right, EVerticalAlign.Top)));

        currentMenuY += MainMenuControlComponentViewRenderableComponent.Y_OFFSET_MENU_ELEMENTS;

        //опция режима стартовой заставки
        targetButton = _modelProviderComponent.IntroCutsceneModeButton;
        chosenFontKey = ChooseFontKey(targetButton);

        _currentMenuElements.Add(new RenderingData.RenderingData(new RenderingString(-1, MENU_LEFT_SIDE_X_OFFSET,
          currentMenuY,
          "Intro", ActualSubassetsDataLibrary.GetFont(chosenFontKey), Color.White, 1, 1,
          EHorizontalAlign.Left, EVerticalAlign.Top)));

        bool isIntroEnabled = !gameSettings.IsIntroDisabled;

        valueCaption = isIntroEnabled ? "Yes" : "No";

        _currentMenuElements.Add(new RenderingData.RenderingData(new RenderingString(-1, MENU_RIGHT_SIDE_X_OFFSET,
          currentMenuY,
          valueCaption, ActualSubassetsDataLibrary.GetFont(chosenFontKey), Color.White, 1, 1,
          EHorizontalAlign.Right, EVerticalAlign.Top)));

        currentMenuY += MainMenuControlComponentViewRenderableComponent.Y_OFFSET_MENU_ELEMENTS;

        //кнопка возврата
        targetButton = _modelProviderComponent.ReturnBackButton;
        chosenFontKey = ChooseFontKey(targetButton);

        _currentMenuElements.Add(new RenderingData.RenderingData(new RenderingString(-1,
          MainMenuControlComponentViewRenderableComponent.X_CENTERED_OFFSET_MENU, currentMenuY,
          "Back", ActualSubassetsDataLibrary.GetFont(chosenFontKey), Color.White, 1, 1,
          EHorizontalAlign.Middle, EVerticalAlign.Top)));

        currentMenuY += MainMenuControlComponentViewRenderableComponent.Y_OFFSET_MENU_ELEMENTS;
      }
    }

    /// <summary>
    /// Действия при изменении режима отображения
    /// </summary>
    private void OnScreenModeChanged()
    {
      var graphicsSettings =
        (GraphicsSettingsDataOpenTk) LinkedViewEventsHandler.LinkedView.CurrentGraphicsSettings;
      bool isFullscreenEnabledNewValue = !graphicsSettings.IsFullscreen;
      graphicsSettings.IsFullscreen = isFullscreenEnabledNewValue;

      AppSerializationManager.SaveDataToFile(graphicsSettings,
        LinkedViewEventsHandler.LinkedView.GraphicalSettingsPath);
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