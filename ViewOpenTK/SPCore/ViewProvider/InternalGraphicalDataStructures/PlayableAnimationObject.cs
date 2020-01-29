using System;
using ViewOpenTK.AssetData.DataTypes.Subassets;

namespace ViewOpenTK.SPCore.ViewProvider.InternalGraphicalDataStructures
{
  /// <summary>
  /// Класс объекта, хранящего состояние о проигрываемой анимации
  /// </summary>
  public class PlayableAnimationObject
  {
    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parTargetAnimationSubasset">Производный ассет с данными анимации</param>
    /// <param name="parFramesPerSecondSpeed">Скорость: кадров анимации в секунду</param>
    public PlayableAnimationObject(SubassetDataAnimation parTargetAnimationSubasset, double parFramesPerSecondSpeed = 60.0)
    {
      TargetAnimationSubasset = parTargetAnimationSubasset;
      FramesPerSecondSpeed = parFramesPerSecondSpeed;
    }

    /// <summary>
    /// Событие окончания проигрывания анимации
    /// </summary>
    public event Action EventFullAnimCompleted;

    /// <summary>
    /// Расчитать текущий шаг анимации
    /// </summary>
    /// <param name="parDeltaTime">Время кадра</param>
    /// <param name="parIsForward">Направление анимации вперед?</param>
    public void ProceedAnimationStep(double parDeltaTime, bool parIsForward = true)
    {
      if (parIsForward)
      {
        CurrentFrame += FramesPerSecondSpeed * parDeltaTime;

        while (CurrentFrame > FrameCount)
        {
          CurrentFrame -= FrameCount;
          EventFullAnimCompleted?.Invoke();
        }
      }
      else
      {
        CurrentFrame -= FramesPerSecondSpeed * parDeltaTime;

        while (CurrentFrame < 0)
        {
          CurrentFrame += FrameCount;
          EventFullAnimCompleted?.Invoke();
        }
      }
    }

    /// <summary>
    /// Получить спрайт текущего кадра анимации
    /// </summary>
    /// <returns></returns>
    public SubassetDataSprite GetCurrentAnimSpriteFrame()
    {
      return TargetAnimationSubasset.SpriteFramesInAnimation
        [Math.Min((int) Math.Floor(CurrentFrame), FrameCount - 1)];
    }

    /// <summary>
    /// Производный ассет с данными анимации
    /// </summary>
    public SubassetDataAnimation TargetAnimationSubasset { get; private set; }

    /// <summary>
    /// Общее количество кадров анимации
    /// </summary>
    public int FrameCount => TargetAnimationSubasset.FramesCount;

    /// <summary>
    /// Номер текущего кадра
    /// </summary>
    public double CurrentFrame { get; set; }

    /// <summary>
    /// Скорость: кадров анимации в секунду
    /// </summary>
    public double FramesPerSecondSpeed { get; set; }
  }
}