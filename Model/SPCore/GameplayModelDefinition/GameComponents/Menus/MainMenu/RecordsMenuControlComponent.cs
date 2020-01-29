using System.Collections.Generic;
using CoreUtil.Pool;
using Model.SPCore.GameplayModelDefinition.ObjectModel;
using Model.SPCore.Managers.Serialization;

namespace Model.SPCore.GameplayModelDefinition.GameComponents.Menus.MainMenu
{
  /// <summary>
  /// Компонент, ответственный за подменю экрана рекордов
  /// </summary>
  public class RecordsMenuControlComponent : StandardMenuControlComponent
  {
    /// <summary>
    /// Конструктор для поддержки системы пулинга объектов
    /// </summary>
    /// <param name="parSupportData">Данные для работы с пулом</param>
    public RecordsMenuControlComponent(ObjectPoolSupportData parSupportData) : base(parSupportData)
    {
    }

    /// <summary>
    /// Замена конструктора, процедура инициализации компонента
    /// </summary>
    /// <param name="parEntGameObject">Родительский игровой объект</param>
    public new RecordsMenuControlComponent Init(GameObject parEntGameObject)
    {
      base.Init(parEntGameObject);

      return this;
    }

    /// <summary>
    /// Процедура активации подменю
    /// </summary>
    public override void Activate()
    {
      base.Activate();

      //Fill menu buttons
      MenuUiElements = new LinkedList<UiElement>();

      RecordOne = CreateStandardButton(() => { });
      MenuUiElements.AddLast(RecordOne);

      RecordTwo = CreateStandardButton(() => { });
      MenuUiElements.AddLast(RecordTwo);

      RecordThree = CreateStandardButton(() => { });
      MenuUiElements.AddLast(RecordThree);

      RecordsClearButton = CreateStandardButton(RecordsClear);
      MenuUiElements.AddLast(RecordsClearButton);

      ReturnBackButton = CreateStandardButton(ReturnBack);
      MenuUiElements.AddLast(ReturnBackButton);

      SelectUiElement(MenuUiElements.First.Value);

      //активация управления

      DefineStdMenuHandlingControls();
    }

    /// <summary>
    /// Действия для очистки таблицы рекордов
    /// </summary>
    private void RecordsClear()
    {
      ParentGameObject.LinkedAppModel.GetRecordsData().SetDefaultValues();
      AppSerializationManager.SaveDataToFile(ParentGameObject.LinkedAppModel.GetRecordsData(),
        ParentGameObject.LinkedAppModel.GetRecordsDataPath());
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
    /// Псевдо-кнопка первого рекорда
    /// </summary>
    public UiElementButton RecordOne { get; private set; }
    /// <summary>
    /// Псевдо-кнопка второго рекорда
    /// </summary>
    public UiElementButton RecordTwo { get; private set; }
    /// <summary>
    /// Псевдо-кнопка третьего рекорда
    /// </summary>
    public UiElementButton RecordThree { get; private set; }
    /// <summary>
    /// Кнопка очистки таблицы рекордов
    /// </summary>
    public UiElementButton RecordsClearButton { get; private set; }
    /// <summary>
    /// Кнопка для возврата в предыдущее меню
    /// </summary>
    public UiElementButton ReturnBackButton { get; private set; }
  }
}