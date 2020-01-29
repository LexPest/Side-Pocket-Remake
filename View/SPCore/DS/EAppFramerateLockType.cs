namespace View.SPCore.DS
{
    /// <summary>
    /// Тип настройки закрепления количества кадров в секунду
    /// </summary>
    public enum EAppFramerateLockType
    {
        /// <summary>
        /// Количество кадров неограничено
        /// </summary>
        Unlocked,
        /// <summary>
        /// Количество кадров закреплено не более 24
        /// </summary>
        Locked24,
        /// <summary>
        /// Количество кадров закреплено не более 30
        /// </summary>
        Locked30,
        /// <summary>
        /// Количество кадров закреплено не более 50
        /// </summary>
        Locked50,
        /// <summary>
        /// Количество кадров закреплено не более 60
        /// </summary>
        Locked60,
        /// <summary>
        /// Количество кадров закреплено не более 120
        /// </summary>
        Locked120,
        /// <summary>
        /// Количество кадров закреплено не более 240
        /// </summary>
        Locked240
    }
}