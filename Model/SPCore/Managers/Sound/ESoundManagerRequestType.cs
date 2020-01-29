namespace Model.SPCore.Managers.Sound
{
    /// <summary>
    /// Типы запросов для аудио менеджера
    /// </summary>
    public enum ESoundManagerRequestType
    {
        /// <summary>
        /// Воспроизводится ли сейчас
        /// </summary>
        IsSoundPlaying,
        /// <summary>
        /// Приостановлен ли сейчас
        /// </summary>
        IsSoundPaused
    }
}