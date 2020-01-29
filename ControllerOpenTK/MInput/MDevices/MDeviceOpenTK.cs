using Controller.MInput.MDevices;
using OpenTK.Input;
using ViewOpenTK.OpenTKInput;
using ViewOpenTK.SPCore;

namespace ControllerOpenTK.MInput.MDevices
{
  /// <summary>
  /// Физическое устройство OpenTK
  /// </summary>
  public class MDeviceOpenTk : MDevice
  {
    /// <summary>
    /// ID для специальных устройств по умолчанию
    /// </summary>
    private const int DEFAULT_NONE_ID = 0;

    /// <summary>
    /// Ось X левого стика геймпада
    /// </summary>
    public const string THUMBSTICK_LEFT_X_ID = "THUBMSTICK_LEFT_X";

    /// <summary>
    /// Ось Y левого стика геймпада
    /// </summary>
    public const string THUMBSTICK_LEFT_Y_ID = "THUBMSTICK_LEFT_Y";

    /// <summary>
    /// Ось X правого стика геймпада
    /// </summary>
    public const string THUMBSTICK_RIGHT_X_ID = "THUBMSTICK_RIGHT_X";

    /// <summary>
    /// Ось Y правого стика геймпада
    /// </summary>
    public const string THUMBSTICK_RIGHT_Y_ID = "THUBMSTICK_RIGHT_Y";

    /// <summary>
    /// Ось мыши X
    /// </summary>
    public const string MOUSE_X_ID = "MOUSE_X";

    /// <summary>
    /// Ось мыши Y
    /// </summary>
    public const string MOUSE_Y_ID = "MOUSE_Y";

    /// <summary>
    /// Нижняя кнопка геймпада 1
    /// </summary>
    public const string GAMEPAD_BUTTON_BOTTOM_1_ID = "GAMEPAD_BUTTON_BOTTOM_1";

    /// <summary>
    /// Нижняя кнопка геймпада 2
    /// </summary>
    public const string GAMEPAD_BUTTON_BOTTOM_2_ID = "GAMEPAD_BUTTON_BOTTOM_2";

    /// <summary>
    /// Верхняя кнопка геймпада 1
    /// </summary>
    public const string GAMEPAD_BUTTON_TOP_1_ID = "GAMEPAD_BUTTON_TOP_1";

    /// <summary>
    /// Верхняя кнопка геймпада 2
    /// </summary>
    public const string GAMEPAD_BUTTON_TOP_2_ID = "GAMEPAD_BUTTON_TOP_2";

    /// <summary>
    /// Крестовина геймпада вверх
    /// </summary>
    public const string GAMEPAD_BUTTON_DPAD_UP_ID = "GAMEPAD_BUTTON_DPAD_UP";

    /// <summary>
    /// Крестовина геймпада вниз
    /// </summary>
    public const string GAMEPAD_BUTTON_DPAD_DOWN_ID = "GAMEPAD_BUTTON_DPAD_DOWN";

    /// <summary>
    /// Крестовина геймпада влево
    /// </summary>
    public const string GAMEPAD_BUTTON_DPAD_LEFT_ID = "GAMEPAD_BUTTON_DPAD_LEFT";

    /// <summary>
    /// Крестовина геймпада вправо
    /// </summary>
    public const string GAMEPAD_BUTTON_DPAD_RIGHT_ID = "GAMEPAD_BUTTON_DPAD_RIGHT";

    /// <summary>
    /// Центральная кнопка геймпада 1
    /// </summary>
    public const string GAMEPAD_BUTTON_CENTER_1_ID = "GAMEPAD_BUTTON_CENTER_1";

    /// <summary>
    /// Текущее состояние устройства геймпада
    /// </summary>
    private GamePadState _currentGamepadState;

    /// <summary>
    /// Текущее состояние устройства клавиатуры
    /// </summary>
    private KeyboardState _currentKeyboardState;

    /// <summary>
    /// Текущее состояние устройства мыши
    /// </summary>
    private MouseState _currentMouseState;

    /// <summary>
    /// Конструктор по умолчанию
    /// </summary>
    /// <param name="parDeviceNumId">Идентификатор физического устройства</param>
    /// <param name="parDeviceStrId">Дополнительный идентификатор физического устройства</param>
    /// <param name="parDeviceDescriptor">Дескриптор-описание физического устройства</param>
    /// <param name="parOpenTkView">Вид OpenTK</param>
    public MDeviceOpenTk(long parDeviceNumId, string parDeviceStrId, string parDeviceDescriptor,
      AppViewOpenTk parOpenTkView) :
      base(parDeviceNumId, parDeviceStrId, parDeviceDescriptor)
    {
      this.OpenTkView = parOpenTkView;
    }

    /// <summary>
    /// Связанный компонент вида OpenTK
    /// </summary>
    private AppViewOpenTk OpenTkView { get; set; }

    /// <summary>
    /// Обработчик обновления состояния геймпада
    /// </summary>
    /// <param name="parState">Объект-состояние геймпада</param>
    private void UpdateGamepadState(object parState)
    {
      _currentGamepadState = (GamePadState) parState;
    }

    /// <summary>
    /// Запрос состояния физического устройства
    /// </summary>
    public void RequestDeviceState()
    {
      if (IsDeviceKeyboardAndMouse())
      {
        OpenTkView.RequestDeviceState(new StateRequest(DeviceType.Keyboard,
          parO => _currentKeyboardState = (KeyboardState) parO, DEFAULT_NONE_ID));
        OpenTkView.RequestDeviceState(new StateRequest(DeviceType.Mouse,
          parO => _currentMouseState = (MouseState) parO, DEFAULT_NONE_ID));
      }
      else
      {
        OpenTkView.RequestDeviceState(new StateRequest(DeviceType.Gamepad,
          UpdateGamepadState, (int) DeviceNumId));
      }
    }

    /// <summary>
    /// Является ли устройство клавиатурой или мышью
    /// </summary>
    /// <returns>True, если является клавиатурой или мышью</returns>
    public bool IsDeviceKeyboardAndMouse()
    {
      return DeviceNumId == MDevicesManagerOpenTk.KEYBOARD_DEVICE_SPECIAL_ID;
    }

    /// <summary>
    /// Получение 'сырого' значения оси устройства
    /// </summary>
    /// <param name="parId">Идентификатор оси</param>
    /// <returns></returns>
    public override double GetAxisValue(string parId)
    {
      if (!IsDeviceKeyboardAndMouse())
      {
        switch (parId)
        {
          case THUMBSTICK_LEFT_X_ID:
          {
            return _currentGamepadState.ThumbSticks.Left.X;
          }
          case THUMBSTICK_LEFT_Y_ID:
          {
            return _currentGamepadState.ThumbSticks.Left.Y;
          }
          case THUMBSTICK_RIGHT_X_ID:
          {
            return _currentGamepadState.ThumbSticks.Right.X;
          }
          case THUMBSTICK_RIGHT_Y_ID:
          {
            return _currentGamepadState.ThumbSticks.Right.Y;
          }
          default:
          {
            return 0.0;
          }
        }
      }
      else
      {
        switch (parId)
        {
          case MOUSE_X_ID:
          {
            return _currentMouseState.X;
          }
          case MOUSE_Y_ID:
          {
            return _currentMouseState.Y;
          }
          default:
          {
            return 0.0;
          }
        }
      }
    }

    /// <summary>
    /// Получение 'сырого' значения кнопки устройства
    /// </summary>
    /// <param name="parId">Идентификатор кнопки</param>
    /// <returns></returns>
    public override bool GetButtonValue(string parId)
    {
      if (!IsDeviceKeyboardAndMouse())
      {
        switch (parId)
        {
          case GAMEPAD_BUTTON_BOTTOM_1_ID:
          {
            return _currentGamepadState.Buttons.A == ButtonState.Pressed;
          }
          case GAMEPAD_BUTTON_BOTTOM_2_ID:
          {
            return _currentGamepadState.Buttons.B == ButtonState.Pressed;
          }
          case GAMEPAD_BUTTON_TOP_1_ID:
          {
            return _currentGamepadState.Buttons.X == ButtonState.Pressed;
          }
          case GAMEPAD_BUTTON_TOP_2_ID:
          {
            return _currentGamepadState.Buttons.Y == ButtonState.Pressed;
          }
          case GAMEPAD_BUTTON_DPAD_UP_ID:
          {
            return _currentGamepadState.DPad.Up == ButtonState.Pressed;
          }
          case GAMEPAD_BUTTON_DPAD_DOWN_ID:
          {
            return _currentGamepadState.DPad.Down == ButtonState.Pressed;
          }
          case GAMEPAD_BUTTON_DPAD_LEFT_ID:
          {
            return _currentGamepadState.DPad.Left == ButtonState.Pressed;
          }
          case GAMEPAD_BUTTON_DPAD_RIGHT_ID:
          {
            return _currentGamepadState.DPad.Right == ButtonState.Pressed;
          }
          case GAMEPAD_BUTTON_CENTER_1_ID:
          {
            return _currentGamepadState.Buttons.Start == ButtonState.Pressed;
          }
          default:
          {
            return false;
          }
        }
      }
      else
      {
        return _currentKeyboardState.IsKeyDown((Key) int.Parse(parId));
      }
    }

    /// <summary>
    /// Является ли геймпад доступным и корректно настроенным
    /// </summary>
    /// <returns>True, если доступен</returns>
    public virtual bool IsJoystickValidAndAvailable()
    {
      var openTkState = _currentGamepadState;
      if (!openTkState.IsConnected)
      {
        return false;
      }

      var description = GamePad.GetName((int) DeviceNumId);
      if (description != DeviceDescriptor)
      {
        return false;
      }

      return true;
    }
  }
}