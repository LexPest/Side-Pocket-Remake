namespace Model.SPCore.Managers.Sound.Data
{
    /// <summary>
    /// Типы фоновой музыки
    /// </summary>
    public enum EAppMusicAssets : short
    {
        /// <summary>
        /// Музыка главного меню
        /// </summary>
        MainMenu,
        /// <summary>
        /// Музыка экрана лобби одиночной игры перед началом сессии
        /// </summary>
        LoungeBeforeLevelStart,
        /// <summary>
        /// Музыка уровня 1
        /// </summary>
        SPLevel1,
        /// <summary>
        /// Музыка уровня 2
        /// </summary>
        SPLevel2,
        /// <summary>
        /// Музыка уровня 3
        /// </summary>
        SPLevel3,
        /// <summary>
        /// Музыка уровня 4
        /// </summary>
        SPLevel4,
        /// <summary>
        /// Музыка уровня 5
        /// </summary>
        SPLevel5,
        /// <summary>
        /// Музыка успешного завершения уровня
        /// </summary>
        SPLevelEnd,
        /// <summary>
        /// Музыка экрана результатов завершения уровня
        /// </summary>
        LevelResults,
        /// <summary>
        /// Бонусный уровень-соревнования
        /// </summary>
        ChallengeShotLevel,
        /// <summary>
        /// Стадия одиночной игры пройдена
        /// </summary>
        LoungeStagePassed,
        /// <summary>
        /// Стадия одиночной игры не пройдена
        /// </summary>
        LoungeStageTryAgain,
        /// <summary>
        /// Музыка экрана лобби игры вдвоем перед началом сессии
        /// </summary>
        LoungeCouchPlayBeforeLevelStart,
        /// <summary>
        /// Музыка для уровня игры вдвоем
        /// </summary>
        CPLevel1,
        /// <summary>
        /// Титры
        /// </summary>
        Credits,
        /// <summary>
        /// Полная победа
        /// </summary>
        Victory,
        /// <summary>
        /// Бонусное ускорение
        /// </summary>
        ZoneBonus,
        /// <summary>
        /// Игра окончена, игрок проиграл все
        /// </summary>
        GameOver
    }
}