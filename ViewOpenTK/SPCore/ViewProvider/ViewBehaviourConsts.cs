using System;
using System.Collections.Generic;
using Model.SPCore.GameplayModelDefinition.GameComponents.InGame.ResultScreens.ViewProviders;
using Model.SPCore.GameplayModelDefinition.GameComponents.InGame.ViewProviders;
using Model.SPCore.GameplayModelDefinition.GameComponents.Launching.ViewProviders;
using Model.SPCore.GameplayModelDefinition.GameComponents.Menus.MainMenu;
using ViewOpenTK.AssetData.DataTypes.Subassets.Strategies;
using ViewOpenTK.AssetData.DataTypes.Subassets.Strategies.Standard;
using ViewOpenTK.SPCore.ViewProvider.ViewComponents.Binds;
using ViewOpenTK.SPCore.ViewProvider.ViewComponents.Binds.Game;
using ViewOpenTK.SPCore.ViewProvider.ViewComponents.Binds.Menus;
using ViewOpenTK.SPCore.ViewProvider.ViewComponents.Binds.ResultsScreen;

namespace ViewOpenTK.SPCore.ViewProvider
{
  /// <summary>
  /// Некоторые константы отображения
  /// </summary>
  public static class ViewBehaviourConsts
  {
    /// <summary>
    /// Основной размер дисплея игры (ширина)
    /// </summary>
    public const int BASE_SURFACE_WIDTH = 320;
    /// <summary>
    /// Основной размер дисплея игры (высота)
    /// </summary>
    public const int BASE_SURFACE_HEIGHT = 240;

    /// <summary>
    /// Начало escape-последовательности в шрифтах
    /// </summary>
    public const string ESCAPE_CHARACTER_SEQUENCE_START = "$|{";
    /// <summary>
    /// Конец escape-последовательности в шрифтах
    /// </summary>
    public const string ESCAPE_CHARACTER_SEQUENCE_END = "}|$";

    /// <summary>
    /// Специальный символ "пробел" в шрифтах
    /// </summary>
    public const string SPECIAL_SYMBOL_SPACE = "SPACE";
    /// <summary>
    /// Стандартный символ-заменитель в шрифтах
    /// </summary>
    public const string DEFAULT_SYMBOL = " ";

    /// <summary>
    /// Шрифт приложения по умолчанию
    /// </summary>
    public const string DEFAULT_APP_FONT = "/game_fonts/spr_gamefont_wb.png";
    /// <summary>
    /// Шрифт с красным затенением
    /// </summary>
    public const string RED_SHADOW_APP_FONT = "/game_fonts/spr_gamefont_redshadow.png";
    /// <summary>
    /// Шрифт для неактивных пунктов меню
    /// </summary>
    public const string MENU_DARK_INACTIVE_APP_FONT = "/game_fonts/spr_gamefont_dark.png";

    /// <summary>
    /// Стандартная стратегия для обновления внутренней библиотеки ресурсов OpenTK
    /// </summary>
    public static IUpdateCollectionLibraryStrategy DefaultUpdatingSubassetsLibStrategy =
      new StandardSpUpdateCollectionLibraryStrategy("gfx_smd");

    /// <summary>
    /// Стандартные привязки типов компонентов модели к соответствующим типам компонентов отображения
    /// </summary> 
    public static readonly Dictionary<Type, Type> StandardViewProviderComponentsModelToView =
      new Dictionary<Type, Type>()
      {
        {typeof(IntroViewProviderComponent), typeof(IntroViewRenderableComponent)},
        {typeof(MainMenuControlComponent), typeof(MainMenuControlComponentViewRenderableComponent)},
        {typeof(OptionsMenuControlComponent), typeof(OptionsMenuControlComponentViewRenderableComponent)},
        {typeof(RecordsMenuControlComponent), typeof(RecordsMenuControlComponentViewRenderableComponent)},
        {typeof(JukeboxMenuControlComponent), typeof(JukeboxMenuControlComponentViewRenderableComponent)},
        {
          typeof(SinglePlayerLobbyMenuControlComponent),
          typeof(SinglePlayerLobbyMenuControlComponentViewRenderableComponent)
        },
        {typeof(PocketGameViewProvider), typeof(PocketGameViewRenderableComponent)},
        {typeof(GameOverScreenComponent), typeof(GameOverViewRenderableComponent)},
        {typeof(ResultsScreenComponent), typeof(ResultsViewRenderableComponent)}
      };
  }
}