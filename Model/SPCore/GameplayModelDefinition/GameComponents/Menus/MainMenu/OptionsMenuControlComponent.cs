using System;
using System.Collections.Generic;
using CoreUtil.Pool;
using Model.SPCore.GameplayModelDefinition.ObjectModel;
using Model.SPCore.Managers.Serialization;
using Model.SPCore.Managers.Sound.Data;

namespace Model.SPCore.GameplayModelDefinition.GameComponents.Menus.MainMenu
{
  /// <summary>
  /// Компонент, ответственный за подменю настроек игры
  /// </summary>
  public class OptionsMenuControlComponent : StandardMenuControlComponent
  {
    /// <summary>
    /// Конструктор для поддержки системы пулинга объектов
    /// </summary>
    /// <param name="parSupportData">Данные для работы с пулом</param>
    public OptionsMenuControlComponent(ObjectPoolSupportData parSupportData) : base(parSupportData)
    {
    }

    /// <summary>
    /// Замена конструктора, процедура инициализации компонента
    /// </summary>
    /// <param name="parEntGameObject">Родительский игровой объект</param>
    public new OptionsMenuControlComponent Init(GameObject parEntGameObject)
    {
      base.Init(parEntGameObject);

      return this;
    }

    /// <summary>
    /// Событие установки нового графического режима экрана
    /// </summary>
    public event Action OnNewScreenModeSet;

    /// <summary>
    /// Процедура активации подменю
    /// </summary>
    public override void Activate()
    {
      base.Activate();

      //Fill menu buttons
      MenuUiElements = new LinkedList<UiElement>();

      ScreenModeButton = CreateStandardButton(ChangeScreenMode);
      MenuUiElements.AddLast(ScreenModeButton);

      MusicModeButton = CreateStandardButton(ChangeMusicMode);
      MenuUiElements.AddLast(MusicModeButton);

      SfxModeButton = CreateStandardButton(ChangeSfxMode);
      MenuUiElements.AddLast(SfxModeButton);

      IntroCutsceneModeButton = CreateStandardButton(ChangeIntroCutsceneMode);
      MenuUiElements.AddLast(IntroCutsceneModeButton);

      ReturnBackButton = CreateStandardButton(ReturnBack);
      MenuUiElements.AddLast(ReturnBackButton);

      SelectUiElement(MenuUiElements.First.Value);

      //активация управления

      DefineStdMenuHandlingControls();
    }

    /// <summary>
    /// Действия по вызову смены режима экрана
    /// </summary>
    private void ChangeScreenMode()
    {
      OnNewScreenModeSet?.Invoke();
      ViewUpdateSignal(0.0);
    }

    /// <summary>
    /// Действия по вызову смены настройки музыки
    /// </summary>
    private void ChangeMusicMode()
    {
      bool newValue = !ParentGameObject.LinkedAppModel.GetGameplaySettingsData().IsMusicEnabled;


      ParentGameObject.LinkedAppModel.GetGameplaySettingsData().IsMusicEnabled =
        newValue;

      AppSerializationManager.SaveDataToFile(ParentGameObject.LinkedAppModel.GetGameplaySettingsData(),
        ParentGameObject.LinkedAppModel.GetAppSettingsPath());

      if (!newValue)
      {
        ParentGameObject.LinkedAppModel.GetSoundManager().StopMusic();
      }
      else
      {
        ParentGameObject.LinkedAppModel.GetSoundManager().PlayBgMusic(EAppMusicAssets.MainMenu, true);
      }

      ViewUpdateSignal(0.0);
    }

    /// <summary>
    /// Действия по вызову смены настройки звуковых эффектов
    /// </summary>
    private void ChangeSfxMode()
    {
      bool newValue = !ParentGameObject.LinkedAppModel.GetGameplaySettingsData().IsSfxEnabled;

      ParentGameObject.LinkedAppModel.GetGameplaySettingsData().IsSfxEnabled =
        newValue;
      AppSerializationManager.SaveDataToFile(ParentGameObject.LinkedAppModel.GetGameplaySettingsData(),
        ParentGameObject.LinkedAppModel.GetAppSettingsPath());

      ViewUpdateSignal(0.0);
    }

    /// <summary>
    /// Действия по вызову смены настройки стартовой заставки
    /// </summary>
    private void ChangeIntroCutsceneMode()
    {
      bool newValue = !ParentGameObject.LinkedAppModel.GetGameplaySettingsData().IsIntroDisabled;
      ParentGameObject.LinkedAppModel.GetGameplaySettingsData().IsIntroDisabled =
        newValue;
      AppSerializationManager.SaveDataToFile(ParentGameObject.LinkedAppModel.GetGameplaySettingsData(),
        ParentGameObject.LinkedAppModel.GetAppSettingsPath());
      ViewUpdateSignal(0.0);
    }

    /// <summary>
    /// Возврат в предыдущее меню
    /// </summary>
    private void ReturnBack()
    {
      CloseSubmenu();
    }

    /// <summary>
    /// Кнопка настройки графического режима экрана
    /// </summary>
    public UiElementButton ScreenModeButton { get; private set; }
    /// <summary>
    /// Кнопка настройки музыки
    /// </summary>
    public UiElementButton MusicModeButton { get; private set; }
    /// <summary>
    /// Кнопка настройки звуковых эффектов
    /// </summary>
    public UiElementButton SfxModeButton { get; private set; }
    /// <summary>
    /// Кнопка настройки стартовой заставки
    /// </summary>
    public UiElementButton IntroCutsceneModeButton { get; private set; }
    /// <summary>
    /// Кнопка для возврата в предыдущее меню
    /// </summary>
    public UiElementButton ReturnBackButton { get; private set; }
  }
}