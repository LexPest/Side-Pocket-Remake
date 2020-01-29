using System;
using System.Collections.Generic;
using System.Drawing;
using Model.SPCore.MPlayers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using ViewOpenTK.OpenGL;
using ViewOpenTK.OpenTKInput;
using ViewOpenTK.SPCore.ViewProvider;
using ViewOpenTK.SPCore.ViewProvider.RenderingData;

namespace ViewOpenTK.SPCore
{
  /// <summary>
  /// Реализация окна приложения OpenTK
  /// </summary>
  public sealed class GlAppWindow : GameWindow
  {
    /// <summary>
    /// Следующие задачи для рендеринга
    /// </summary>
    private LinkedList<RenderingTask> _nextRenderingTasks;

    /// <summary>
    /// Связанный объект вида MVC
    /// </summary>
    private AppViewOpenTk _viewApp;

    /// <summary>
    /// Флаг: по крайней мере одна операция рендеринга успешно произведена (необходимо для инициализации)
    /// </summary>
    public bool AtLeastOneRenderOpPerformed { get; private set; } = false;

    /// <summary>
    /// Флаг ожидания обновления
    /// </summary>
    public bool WaitingForUpdateFlag = false;

    /// <summary>
    /// Добавить задачу для рендеринга
    /// </summary>
    /// <param name="parTask">Объект задачи для рендеринга</param>
    public void AddTask(RenderingTask parTask)
    {
      if (_nextRenderingTasks == null)
      {
        _nextRenderingTasks = new LinkedList<RenderingTask>();
      }

      _nextRenderingTasks.AddLast(parTask);

      RenderReady = false;
      _renderNeed = true;
    }

    /// <summary>
    /// Объект блокировки для синхронизации потоков при рендеринге
    /// </summary>
    public object NextRenderTaskLock = new object();

    /// <summary>
    /// Флаг необходимости перерисовки
    /// </summary>
    private bool _renderNeed = false;

    /// <summary>
    /// Флаг успошного выполнения операций рендеринга
    /// </summary>
    public bool RenderReady { get; private set; } = true;

    /// <summary>
    /// Идентификатор текстуры для кадрового буфера OpenGL
    /// </summary>
    private int GlSurfaceTextureId { get; set; } = -1;

    /// <summary>
    /// Идентификатор кадрового буфера OpenGL
    /// </summary>
    private int GlSurfaceFramebuffer { get; set; } = -1;

    /// <summary>
    /// Режим отрисовки OpenGL
    /// </summary>
    private DrawBufferMode _glSurfaceDrawBuffers;

    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parWidth">Ширина окна</param>
    /// <param name="parHeight">Высота окна</param>
    /// <param name="parTitle">Заголовок окна</param>
    /// <param name="parOptions">Настройки окна</param>
    /// <param name="parViewApp">Представление MVC</param>
    public GlAppWindow(int parWidth, int parHeight, string parTitle, GameWindowFlags parOptions,
      AppViewOpenTk parViewApp)
      : base(parWidth, parHeight, GraphicsMode.Default, parTitle, parOptions, DisplayDevice.Default, 2, 1,
        GraphicsContextFlags.ForwardCompatible)
    {
      _viewApp = parViewApp;
      OpenGlWindowDisplay.Instance.UpdateDisplay(parWidth, parHeight);
      GL.Enable(EnableCap.Texture2D);
      GL.Enable(EnableCap.Blend);
      GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

      Console.WriteLine(
        $"GL: OpenGL Initialized, using renderer: {GL.GetString(StringName.Version)} - {GL.GetError()}");
    }

    /// <summary>
    /// Проверить текстуру кадрового буфера
    /// </summary>
    private void CheckSurfaceTexture()
    {
      if (GlSurfaceTextureId <= 0 || GlSurfaceFramebuffer <= 0)
      {
        GlSurfaceFramebuffer = GL.GenFramebuffer();
        GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, GlSurfaceFramebuffer);

        GlSurfaceTextureId = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, GlSurfaceTextureId);

        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb,
          ViewBehaviourConsts.BASE_SURFACE_WIDTH, ViewBehaviourConsts.BASE_SURFACE_HEIGHT, 0, PixelFormat.Rgb,
          PixelType.UnsignedByte, IntPtr.Zero);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
          (int) TextureMinFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
          (int) TextureMinFilter.Nearest);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS,
          (int) TextureWrapMode.Clamp);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,
          (int) TextureWrapMode.Clamp);

        GL.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer, FramebufferAttachment.ColorAttachment0,
          TextureTarget.Texture2D, GlSurfaceTextureId, 0);

        _glSurfaceDrawBuffers = DrawBufferMode.ColorAttachment0;
        GL.DrawBuffer(_glSurfaceDrawBuffers);
      }
    }

    /// <summary>
    /// Действия при рендеринге кадра
    /// </summary>
    /// <param name="parE">Аргументы события рендеринга кадра</param>
    /// <exception cref="ArgumentOutOfRangeException">Режим рендеринга не поддерживается</exception>
    protected override void OnRenderFrame(FrameEventArgs parE)
    {
      base.OnRenderFrame(parE);

      ExecuteInternalGlActions();

      lock (NextRenderTaskLock)
      {
        if (_renderNeed)
        {
          _renderNeed = false;

          switch (OpenGlWindowDisplay.Instance.AppRenderingMode)
          {
            case EOpenGlAppRenderingMode.UsingFramebuffer:
            {
              //render to texture
              CheckSurfaceTexture();

              GL.BindTexture(TextureTarget.Texture2D, 0);
              GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, GlSurfaceFramebuffer);

              GL.Viewport(0, 0, (int) OpenGlWindowDisplay.Instance.ViewportWidth,
                (int) OpenGlWindowDisplay.Instance.ViewportHeight);


              RenderTasks();

              GL.DrawBuffer(_glSurfaceDrawBuffers);


              GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

              GL.Viewport(0, 0, Width, Height);

              GL.BindTexture(TextureTarget.Texture2D, GlSurfaceTextureId);

              GL.Clear(ClearBufferMask.ColorBufferBit);
              GL.ClearColor(Color.Black);

              GL.Begin(PrimitiveType.Quads);
              GL.Color3(Color.White);


              GL.TexCoord2(0, 1);
              GL.Vertex2(OpenGlWindowDisplay.Instance.DisplayViewportGlUnitsX1,
                OpenGlWindowDisplay.Instance.DisplayViewportGlUnitsY1);

              GL.TexCoord2(1, 1);
              GL.Vertex2(OpenGlWindowDisplay.Instance.DisplayViewportGlUnitsX2,
                OpenGlWindowDisplay.Instance.DisplayViewportGlUnitsY1);

              GL.TexCoord2(1, 0);
              GL.Vertex2(OpenGlWindowDisplay.Instance.DisplayViewportGlUnitsX2,
                OpenGlWindowDisplay.Instance.DisplayViewportGlUnitsY2);

              GL.TexCoord2(0, 0);
              GL.Vertex2(OpenGlWindowDisplay.Instance.DisplayViewportGlUnitsX1,
                OpenGlWindowDisplay.Instance.DisplayViewportGlUnitsY2);


              GL.End();

              break;
            }

            case EOpenGlAppRenderingMode.UsingGlobalRescale:
            {
              GL.Clear(ClearBufferMask.ColorBufferBit);
              GL.ClearColor(Color.Black);

              GL.BindTexture(TextureTarget.Texture2D, 0);

              RenderTasks();

              break;
            }

            default:
              throw new ArgumentOutOfRangeException();
          }


          GL.Finish();
          GL.Flush();
          RenderReady = true;
          this.SwapBuffers();
        }
      }

      AtLeastOneRenderOpPerformed = true;
    }

    /// <summary>
    /// Выполнить задачи рендеринга
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Тип задачи не поддерживается</exception>
    private void RenderTasks()
    {
      foreach (var nextRenderingTask in _nextRenderingTasks)
      {
        switch (nextRenderingTask.RenderingDataType)
        {
          case ERenderingTaskType.Primitive:
          {
            GL.BindTexture(TextureTarget.Texture2D, 0);

            GL.Color4(nextRenderingTask.BlendColor);
            GL.Begin(PrimitiveType.Quads);

            for (int i = 0; i < 4; i++)
            {
              GL.Vertex2(nextRenderingTask.Vertices[i].X, nextRenderingTask.Vertices[i].Y);
            }

            GL.End();
            break;
          }

          case ERenderingTaskType.Sprite:
          {
            GL.BindTexture(TextureTarget.Texture2D, nextRenderingTask.GlTexture);
            GL.Begin(PrimitiveType.Quads);

            GL.Color3(nextRenderingTask.BlendColor);

            for (int i = 0; i < 4; i++)
            {
              GL.TexCoord2(nextRenderingTask.TexVertices[i].X,
                nextRenderingTask.TexVertices[i].Y);
              GL.Vertex2(nextRenderingTask.Vertices[i].X, nextRenderingTask.Vertices[i].Y);
            }

            GL.End();

            break;
          }

          default:
            throw new ArgumentOutOfRangeException();
        }
      }

      _nextRenderingTasks.Clear();
    }


    /// <summary>
    /// Исполнить служебные и системные команды из очереди OpenGL
    /// </summary>
    private void ExecuteInternalGlActions()
    {
      Action nextGlCommand;
      while ((nextGlCommand = OpenGlCommandsInternalHandler.GetNextGlAction()) != null)
      {
        nextGlCommand();
      }
    }

    /// <summary>
    /// Действия при изменении размеров окна
    /// </summary>
    /// <param name="parE">Аргументы события изменения размеров окна</param>
    protected override void OnResize(EventArgs parE)
    {
      base.OnResize(parE);
      GL.Viewport(0, 0, Width, Height);
      OpenGlWindowDisplay.Instance.UpdateDisplay(Width, Height);
    }

    /// <summary>
    /// Действия при обновлении состояния окна OpenTK
    /// </summary>
    /// <param name="parE">Аргументы события обновления состояния окна OpenTK</param>
    /// <exception cref="ArgumentOutOfRangeException">Тип устройства не поддерживается</exception>
    protected override void OnUpdateFrame(FrameEventArgs parE)
    {
      lock (_viewApp.InputSyncObj)
      {
        base.OnUpdateFrame(parE);
        //_viewApp.CurrentKBState = Keyboard.GetState();
        while (_viewApp.StateRequests.TryDequeue(out StateRequest sr))
        {
          switch (sr.RequestedDeviceType)
          {
            case DeviceType.Keyboard:
              sr.OnStateArrived(Keyboard.GetState());
              break;
            case DeviceType.Mouse:
              sr.OnStateArrived(Mouse.GetState());
              break;
            case DeviceType.Gamepad:
              sr.OnStateArrived(GamePad.GetState(sr.DeviceNumId));
              break;
            default:
              throw new ArgumentOutOfRangeException();
          }
        }
      }

      WaitingForUpdateFlag = true;
    }

    /// <summary>
    /// Действия при событии нажатия клавиши
    /// </summary>
    /// <param name="parE">Аргументы события нажатия клавиши</param>
    protected override void OnKeyDown(KeyboardKeyEventArgs parE)
    {
      base.OnKeyDown(parE);
      if (parE.Alt && parE.Key == Key.F4)
      {
        _viewApp.App.ExitApp();
      }
      else
      {
        string keyString = parE.Key.ToString();


        bool shouldSend = false;

        if (keyString == MPlayersManager.SPECIAL_BACKSPACE_SIGNATURE)
        {
          shouldSend = true;
        }
        else if (keyString.Length == 1)
        {
          if (parE.Shift)
          {
            keyString = keyString.ToUpper();
          }
          else
          {
            keyString = keyString.ToLower();
          }

          shouldSend = true;
        }

        if (shouldSend)
        {
          _viewApp.InvokeLetterKeyPressed(keyString);
        }
      }
    }

    /// <summary>
    /// Действия при закрытии окна
    /// </summary>
    /// <param name="parE">Аргументы события при закрытии окна</param>
    protected override void OnClosed(EventArgs parE)
    {
      base.OnClosed(parE);
      _viewApp.App.ExitApp();
    }

    /// <summary>
    /// Действия после успешной загрузки окна
    /// </summary>
    /// <param name="parE">Аргументы события после успешной загрузки окна</param>
    protected override void OnLoad(EventArgs parE)
    {
      base.OnLoad(parE);
      CursorVisible = false;
      _viewApp.ViewAvailable = true;
    }
  }
}