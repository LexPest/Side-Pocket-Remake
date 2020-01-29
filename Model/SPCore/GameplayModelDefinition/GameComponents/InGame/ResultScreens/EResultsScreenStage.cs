namespace Model.SPCore.GameplayModelDefinition.GameComponents.InGame.ResultScreens
{
    /// <summary>
    /// Состояние экрана результата при успешном прохождении уровня
    /// </summary>
    public enum EResultsScreenStage
    {
        /// <summary>
        /// Выбор первоначального действия
        /// </summary>
        ChooseNextAction,
        /// <summary>
        /// Ввод имени для таблицы рекордов
        /// </summary>
        ChooseNameForTheRecords
    }
}