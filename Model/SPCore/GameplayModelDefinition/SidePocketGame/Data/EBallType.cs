namespace Model.SPCore.GameplayModelDefinition.SidePocketGame.Data
{
  /// <summary>
  /// Тип шара, с внутренней нумерацией от 0 до 10
  /// </summary>
  public enum EBallType : byte
  {
    /// <summary>
    /// Белый шар игрока
    /// </summary>
    WhitePlayerBall = 0,

    /// <summary>
    /// Желтый
    /// </summary>
    Yellow = 1,

    /// <summary>
    /// Синий
    /// </summary>
    Blue = 2,

    /// <summary>
    /// Красный
    /// </summary>
    Red = 3,

    /// <summary>
    /// Фиолетовый
    /// </summary>
    Purple = 4,

    /// <summary>
    /// Оранжевый
    /// </summary>
    Orange = 5,

    /// <summary>
    /// Зеленый
    /// </summary>
    Green = 6,

    /// <summary>
    /// Коричневый
    /// </summary>
    Brown = 7,

    /// <summary>
    /// Черный
    /// </summary>
    Black = 8,

    /// <summary>
    /// Шар-девятка
    /// </summary>
    Yellow9Ball = 9,

    /// <summary>
    /// Шар-десятка
    /// </summary>
    Blue10Ball = 10
  }
}