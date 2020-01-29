using System.Collections.Generic;
using System.Drawing;
using Model.SPCore.GameplayModelDefinition.SidePocketGame.Data;

namespace ViewOpenTK.SPCore.ViewProvider.ViewComponents.Binds.Game
{
  /// <summary>
  /// Константы, связанные с отображением игрового поля-стола
  /// </summary>
  public static class GameFieldViewConsts
  {
    /// <summary>
    /// Смещение по оси X
    /// </summary>
    public const int FIELD_MODEL_RENDER_LEFT_TOP_OFFSET_X = 26;
    /// <summary>
    /// Смещение по оси Y
    /// </summary>
    public const int FIELD_MODEL_RENDER_LEFT_TOP_OFFSET_Y = 60;

    /// <summary>
    /// Ширина поля
    /// </summary>
    public const int FIELD_WIDTH = 268;
    /// <summary>
    /// Высота поля
    /// </summary>
    public const int FIELD_HEIGHT = 160;

    /// <summary>
    /// Префикс названия спрайтов шаров
    /// </summary>
    private const string BALLS_ALL_PREFIX = "/balls_tex/spr_tex_balls_all.png/";

    /// <summary>
    /// Словарь соответствия типа шара и данных для рендеринга
    /// </summary>
    public static Dictionary<EBallType, BallGraphicsBind> BallGraphicsBinds =
      new Dictionary<EBallType, BallGraphicsBind>()
      {
        {EBallType.WhitePlayerBall, new BallGraphicsBind(Color.White, $"{BALLS_ALL_PREFIX}ball_player_rot_0")},
        {EBallType.Black, new BallGraphicsBind(Color.Black, $"{BALLS_ALL_PREFIX}ball_normal_rot_0")},
        {EBallType.Blue, new BallGraphicsBind(Color.Blue, $"{BALLS_ALL_PREFIX}ball_normal_rot_0")},
        {EBallType.Brown, new BallGraphicsBind(Color.SaddleBrown, $"{BALLS_ALL_PREFIX}ball_normal_rot_0")},
        {EBallType.Green, new BallGraphicsBind(Color.Green, $"{BALLS_ALL_PREFIX}ball_normal_rot_0")},
        {EBallType.Orange, new BallGraphicsBind(Color.Orange, $"{BALLS_ALL_PREFIX}ball_normal_rot_0")},
        {EBallType.Purple, new BallGraphicsBind(Color.Purple, $"{BALLS_ALL_PREFIX}ball_normal_rot_0")},
        {EBallType.Red, new BallGraphicsBind(Color.Red, $"{BALLS_ALL_PREFIX}ball_normal_rot_0")},
        {EBallType.Yellow, new BallGraphicsBind(Color.Yellow, $"{BALLS_ALL_PREFIX}ball_normal_rot_0")},
        {EBallType.Yellow9Ball, new BallGraphicsBind(Color.Yellow, $"{BALLS_ALL_PREFIX}ball_special_rot_0")},
        {EBallType.Blue10Ball, new BallGraphicsBind(Color.Blue, $"{BALLS_ALL_PREFIX}ball_special_rot_0")},
      };
  }
}