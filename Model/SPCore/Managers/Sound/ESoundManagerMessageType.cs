namespace Model.SPCore.Managers.Sound
{
    /// <summary>
    /// Тип сообщения аудио менеджера
    /// </summary>
    public enum ESoundManagerMessageType
    {
        /// <summary>
        /// Воспроизвести
        /// </summary>
        SoundPlay,
        /// <summary>
        /// Приостановить воспроизведение
        /// </summary>
        SoundPause,
        /// <summary>
        /// Остановить воспроизведение
        /// </summary>
        SoundStop,
        /// <summary>
        /// Сброс воспроизведения
        /// </summary>
        SoundReset,
        /// <summary>
        /// Установка флага зацикленности воспроизведения
        /// </summary>
        SoundSetLoop
    }
}