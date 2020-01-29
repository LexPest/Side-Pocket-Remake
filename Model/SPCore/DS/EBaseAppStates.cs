namespace Model.SPCore.DS
{
    /// <summary>
    /// Состояния приложения
    /// </summary>
    public enum EBaseAppStates
    {
        /// <summary>
        /// Создано
        /// </summary>
        Created,
        /// <summary>
        /// Запущено
        /// </summary>
        Running,
        /// <summary>
        /// Приостановлено
        /// </summary>
        Suspended,
        /// <summary>
        /// Осуществлен выход
        /// </summary>
        Quit
    }
}