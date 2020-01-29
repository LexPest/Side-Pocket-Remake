using System;
using System.Collections.Generic;
using System.Drawing;
using CoreUtil.Pool;
using Model.SPCore.GameplayModelDefinition.GameComponents.Menus;
using Model.SPCore.GameplayModelDefinition.GameComponents.Menus.MainMenu;
using Model.SPCore.GameplayModelDefinition.GameComponents.ViewProviderBase;
using Model.SPCore.Managers.Sound.Data;
using ViewOpenTK.SPCore.ViewProvider.InternalGraphicalDataStructures;
using ViewOpenTK.SPCore.ViewProvider.RenderingData;

namespace ViewOpenTK.SPCore.ViewProvider.ViewComponents.Binds.Menus
{
  /// <summary>
  /// Компонент для рендеринга данных подменю прослушивания саундтрека
  /// </summary>
  public class JukeboxMenuControlComponentViewRenderableComponent : ViewRenderableComponent
  {
    /// <summary>
    /// Название спрайта заднего фона
    /// </summary>
    public const string JUKEBOX_MENU_BG = "/jukebox_menu/spr_jukebox_background.png";

    /// <summary>
    /// Название спрайта наложения переднего плана
    /// </summary>
    public const string JUKEBOX_MENU_FG = "/jukebox_menu/spr_jukebox_foreground.png";

    /// <summary>
    /// Позиция X для кнопок
    /// </summary>
    private const int BUTTONS_MIDDLE_X = 154;

    /// <summary>
    /// Позиция Y для кнопки смены композиции
    /// </summary>
    private const int BUTTON_MUSIC_MIDDLE_Y = 156;

    /// <summary>
    /// Позиция Y для кнопки возврата назад
    /// </summary>
    private const int BUTTON_BACK_MIDDLE_Y = 177;

    /// <summary>
    /// Данные для рендеринга элементов меню
    /// </summary>
    private List<RenderingData.RenderingData?> _currentMenuElements;

    /// <summary>
    /// Компонент на стороне модели
    /// </summary>
    private JukeboxMenuControlComponent _modelProviderComponent;

    /// <summary>
    /// Конструктор для поддержки системы пулинга объектов
    /// </summary>
    /// <param name="parSupportData">Данные для работы с пулом</param>
    public JukeboxMenuControlComponentViewRenderableComponent(ObjectPoolSupportData parSupportData) : base(
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

      _modelProviderComponent = (JukeboxMenuControlComponent) parModelSideProviderComponent;

      _modelProviderComponent.OnBecomeActive += OnActivatedPerform;
      _modelProviderComponent.OnDeactivated += OnDeactivatedPerform;
    }

    /// <summary>
    /// Действия при активации меню
    /// </summary>
    private void OnActivatedPerform()
    {
      ProcessUiElementsList();
      Console.WriteLine("Jukebox Menu activated");
    }

    /// <summary>
    /// Действия при деактвации меню
    /// </summary>
    private void OnDeactivatedPerform()
    {
      _currentMenuElements = null;
      Console.WriteLine("Jukebox Menu disabled");
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
        new RenderingSprite(ActualSubassetsDataLibrary.GetSprite(JUKEBOX_MENU_BG), 0, 0, 0, Color.White, 0)));

      //добавить передний план
      _currentMenuElements.Add(new RenderingData.RenderingData(
        new RenderingSprite(ActualSubassetsDataLibrary.GetSprite(JUKEBOX_MENU_FG), 0, 0, 0, Color.White, 0)));

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

      ProcessButton(_modelProviderComponent.MusicPlayerControlButton, BUTTONS_MIDDLE_X, BUTTON_MUSIC_MIDDLE_Y,
        AppAudioLibraryConsts.JukeboxMusicSmdLibrary[_modelProviderComponent.CurrentSelectedTrack]);

      ProcessButton(_modelProviderComponent.ReturnBackButton, BUTTONS_MIDDLE_X, BUTTON_BACK_MIDDLE_Y, "Back");
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