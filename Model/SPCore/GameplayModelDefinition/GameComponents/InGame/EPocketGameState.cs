namespace Model.SPCore.GameplayModelDefinition.GameComponents.InGame
{
    /// <summary>
    /// Состояние игровой сессии
    /// </summary>
    public enum EPocketGameState
    {
        /// <summary>
        /// Инициализация
        /// </summary>
        Init,
        /// <summary>
        /// Прицеливание
        /// </summary>
        Aiming,
        /// <summary>
        /// Выбор силы удара
        /// </summary>
        ChooseShotPower,
        /// <summary>
        /// Движение шаров
        /// </summary>
        BallsMovingInProgress,
        /// <summary>
        /// Пауза
        /// </summary>
        Paused,
        /// <summary>
        /// Уровень завершен
        /// </summary>
        LevelEnd
    }
}