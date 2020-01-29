using System;
using System.Collections.Generic;
using CoreUtil.Pool;
using Model.SPCore.GameplayModelDefinition.GameComponents.Universal.DS;
using Model.SPCore.GameplayModelDefinition.ObjectModel;
using Model.SPCore.Managers.Sound.Data;
using Model.SPCore.MGameActions;

namespace Model.SPCore.GameplayModelDefinition.GameComponents.Menus.MainMenu
{
  /// <summary>
  /// Компонент, ответственный за подменю музыкального проигрывателя саундтрека
  /// </summary>
  public class JukeboxMenuControlComponent : StandardMenuControlComponent
  {
    /// <summary>
    /// Вспомогательный массив, хранящий значения перечисления музыки
    /// </summary>
    private Array _enumAssetsValues;

    /// <summary>
    /// Конструктор для поддержки системы пулинга объектов
    /// </summary>
    /// <param name="parSupportData">Данные для работы с пулом</param>
    public JukeboxMenuControlComponent(ObjectPoolSupportData parSupportData) : base(parSupportData)
    {
    }

    /// <summary>
    /// Замена конструктора, процедура инициализации компонента
    /// </summary>
    /// <param name="parEntGameObject">Родительский игровой объект</param>
    public new JukeboxMenuControlComponent Init(GameObject parEntGameObject)
    {
      base.Init(parEntGameObject);

      _enumAssetsValues = Enum.GetValues(typeof(EAppMusicAssets));

      CurrentSelectedTrack = (EAppMusicAssets) _enumAssetsValues.GetValue(0);

      return this;
    }

    /// <summary>
    /// Процедура активации подменю
    /// </summary>
    public override void Activate()
    {
      //заполнение кнопок меню
      MenuUiElements = new LinkedList<UiElement>();

      LinkedList<KeyListenerData> buttonKeyListenerData = new LinkedList<KeyListenerData>();

      buttonKeyListenerData.AddLast(new KeyListenerData(EGameActionButton.Button_A,
        PlayerRef, PlayTrack, false));
      buttonKeyListenerData.AddLast(new KeyListenerData(EGameActionButton.Button_B,
        PlayerRef, PlayTrack, false));
      buttonKeyListenerData.AddLast(new KeyListenerData(EGameActionButton.Button_X,
        PlayerRef, PlayTrack, false));
      buttonKeyListenerData.AddLast(new KeyListenerData(EGameActionButton.Button_Start,
        PlayerRef, PlayTrack, false));
      buttonKeyListenerData.AddLast(new KeyListenerData(EGameActionButton.Dpad_Menu_Left,
        PlayerRef, PrevTrack, false));
      buttonKeyListenerData.AddLast(new KeyListenerData(EGameActionButton.Dpad_Menu_Right,
        PlayerRef, NextTrack, false));

      MusicPlayerControlButton = new UiElementButton(buttonKeyListenerData, ParentGameObject);
      MenuUiElements.AddLast(MusicPlayerControlButton);

      ReturnBackButton = CreateStandardButton(ReturnBack);
      MenuUiElements.AddLast(ReturnBackButton);

      SelectUiElement(MenuUiElements.First.Value);


      //активация управления
      DefineStdMenuHandlingControls();

      //выключение музыки главного меню
      ParentGameObject.LinkedAppModel.GetSoundManager().StopMusic();

      base.Activate();
    }

    /// <summary>
    /// Процедура деактивации подменю
    /// </summary>
    public override void Deactivate()
    {
      base.Deactivate();
    }

    /// <summary>
    /// Начать воспроизведение музыкального трека
    /// </summary>
    private void PlayTrack()
    {
      ParentGameObject.LinkedAppModel.GetSoundManager().PlayBgMusic(CurrentSelectedTrack, true);
    }

    /// <summary>
    /// Выбрать следующий трек
    /// </summary>
    private void NextTrack()
    {
      short possibleNextTrack = (short) ((short) CurrentSelectedTrack + 1);
      if (possibleNextTrack >= _enumAssetsValues.Length)
      {
        possibleNextTrack = 0;
      }

      CurrentSelectedTrack = (EAppMusicAssets) possibleNextTrack;

      ViewUpdateSignal(0.0);
    }

    /// <summary>
    /// Выбрать предыдущий трек
    /// </summary>
    private void PrevTrack()
    {
      short possibleNextTrack = (short) ((short) CurrentSelectedTrack - 1);
      if (possibleNextTrack < 0)
      {
        possibleNextTrack = (short) (_enumAssetsValues.Length - 1);
      }

      CurrentSelectedTrack = (EAppMusicAssets) possibleNextTrack;

      ViewUpdateSignal(0.0);
    }

    /// <summary>
    /// Возврат в предыдущее меню
    /// </summary>
    private void ReturnBack()
    {
      //включаем обратно музыку главного меню
      ParentGameObject.LinkedAppModel.GetSoundManager().PlayBgMusic(EAppMusicAssets.MainMenu, true);
      CloseSubmenu();
    }

    /// <summary>
    /// Кнопка управления текущим треком
    /// </summary>
    public UiElementButton MusicPlayerControlButton { get; private set; }

    /// <summary>
    /// Кнопка "Назад"
    /// </summary>
    public UiElementButton ReturnBackButton { get; private set; }

    /// <summary>
    /// Текущий выбранный музыкальный трек
    /// </summary>
    public EAppMusicAssets CurrentSelectedTrack { get; private set; }
  }
}