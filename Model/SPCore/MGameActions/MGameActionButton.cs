using System;

namespace Model.SPCore.MGameActions
{
  /// <summary>
  /// Игровое действие типа "кнопка"
  /// </summary>
  public class MGameActionButton
  {
    private readonly object _lockObj = new object();
    private readonly object _releasedFrameLockObj = new object();
    public event Action ButtonPressedFrame;
    public event Action ButtonReleasedFrame;
    public event Action ButtonIsHoldedFrame;

    /// <summary>
    /// Call this before every 'fixed' update frame from Contoller
    /// </summary>
    public void SetPressedStatus(bool parIsPressed)
    {
      //  lock (lockObj)
      //  {

      if (parIsPressed)
      {
        if (!IsHolding)
        {
          IsPressedFrame = IsHolding = true;
          ButtonPressedFrame?.Invoke();
        }
        else
        {
          IsPressedFrame = false;
        }

        ButtonIsHoldedFrame?.Invoke();
      }
      else
      {
        IsPressedFrame = false;
        if (IsHolding)
        {
          IsHolding = false;
          IsReleasedFrame = true;
          ButtonReleasedFrame?.Invoke();
        }
        else
        {
          IsReleasedFrame = false;
        }
      }

      //  }
    }

    public bool IsHolding { get; private set; }
    public bool IsReleasedFrame { get; private set; }
    public bool IsPressedFrame { get; private set; }
  }
}