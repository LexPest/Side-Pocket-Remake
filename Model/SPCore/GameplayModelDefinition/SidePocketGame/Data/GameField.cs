namespace Model.SPCore.GameplayModelDefinition.SidePocketGame.Data
{
  /// <summary>
  /// Игровое поле-стол
  /// </summary>
  public class GameField
  {
    /// <summary>
    /// Линии коллизий для бортов
    /// </summary>
    public CollisionLine[] CollisionLines { get; }

    /// <summary>
    /// Высота
    /// </summary>
    public double Height { get; }

    /// <summary>
    /// Ширина
    /// </summary>
    public double Width { get; }

    /// <summary>
    /// Лузы
    /// </summary>
    public Pocket[] Pockets { get; }

    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parWidth">Ширина</param>
    /// <param name="parHeight">Высота</param>
    /// <param name="parPockets">Лузы</param>
    /// <param name="parCollisionLines">Линии коллизий для бортов</param>
    public GameField(double parWidth, double parHeight, Pocket[] parPockets, CollisionLine[] parCollisionLines)
    {
      Width = parWidth;
      Height = parHeight;
      Pockets = parPockets;
      CollisionLines = parCollisionLines;
    }
  }
}