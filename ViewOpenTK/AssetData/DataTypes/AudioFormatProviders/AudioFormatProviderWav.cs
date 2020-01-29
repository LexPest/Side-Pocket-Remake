using System;
using System.IO;
using OpenTK.Audio.OpenAL;
using ViewOpenTK.OpenAL.BasicManager;

namespace ViewOpenTK.AssetData.DataTypes.AudioFormatProviders
{
  /// <summary>
  /// Реализация работы с форматом аудио WAVE
  /// </summary>
  public class AudioFormatProviderWav : IAudioFormatProvider
  {
    /// <summary>
    /// Связанные данные аудио ассета OpenTK
    /// </summary>
    private AssetDataOpenTkWaveSound _linkedAssetData;

    /// <summary>
    /// Конструктор по умолчанию
    /// </summary>
    /// <param name="parAudioBinaryData">Бинарные данные аудио</param>
    public AudioFormatProviderWav(byte[] parAudioBinaryData)
    {
      AudioBinaryData = parAudioBinaryData;
    }

    /// <summary>
    /// Инициализация и первоначальная загрузка аудио данных
    /// </summary>
    /// <param name="parLinkedOpenTkSound">Аудио данные OpenTK</param>
    public void InitAudioData(AssetDataOpenTkWaveSound parOpenTkWaveSound)
    {
      _linkedAssetData = parOpenTkWaveSound;

      AlBuffer = AL.GenBuffer();
      AlSource = AL.GenSource();

      OpenAlManager.CheckForError("AL loading not ready...");

      using (MemoryStream ms = new MemoryStream(AudioBinaryData))
      {
        using (BinaryReader br = new BinaryReader(ms))
        {
          // проверка заголовка RIFF
          string signature = new string(br.ReadChars(4));
          if (signature != "RIFF")
            throw new NotSupportedException("Specified stream is not a wave file.");

          int riffChunckSize = br.ReadInt32();

          string format = new string(br.ReadChars(4));
          if (format != "WAVE")
            throw new NotSupportedException("Specified stream is not a wave file.");

          // проверка заголовка WAVE
          string formatSignature = new string(br.ReadChars(4));
          if (formatSignature != "fmt ")
            throw new NotSupportedException("Specified wave file is not supported.");

          int formatChunkSize = br.ReadInt32();
          int audioFormat = br.ReadInt16();
          int numChannels = br.ReadInt16();
          int sampleRate = br.ReadInt32();
          int byteRate = br.ReadInt32();
          int blockAlign = br.ReadInt16();
          int bitsPerSample = br.ReadInt16();

          string dataSignature = new string(br.ReadChars(4));
          if (dataSignature != "data")
            throw new NotSupportedException("Specified wave file is not supported.");

          int dataChunkSize = br.ReadInt32();

          Channels = numChannels;
          BitsPerSample = bitsPerSample;
          SampleRate = sampleRate;

          AudioBinaryData = br.ReadBytes((int) dataChunkSize);
        }
      }

      Console.WriteLine(
        $"Binary data length: {AudioBinaryData.Length}, Sample rate: {SampleRate}, {Environment.NewLine}BitsPerSample: {BitsPerSample}");
      AL.BufferData(AlBuffer, GetSoundFormat(Channels, BitsPerSample), AudioBinaryData, AudioBinaryData.Length,
        SampleRate);

      if (!OpenAlManager.CheckForError($"Loading error: {_linkedAssetData.ActualAssetMetadata.FilePath}"))
      {
      }

      AL.Source(AlSource, ALSourcei.Buffer, AlBuffer);

      if (!OpenAlManager.CheckForError($"Loading error: {_linkedAssetData.ActualAssetMetadata.FilePath}"))
      {
        Console.WriteLine($"AL: Audio data {AlSource}:{AlBuffer} created in memory");
      }
    }

    /// <summary>
    /// Воспроизведение
    /// </summary>
    /// <param name="parIsLooped">Зациклить воспроизведение?</param>
    public void Play(bool parIsLooped)
    {
      AL.Source(AlSource, ALSourceb.Looping, parIsLooped);
      AL.SourcePlay(AlSource);

      OpenAlManager.CheckForError(
        $"Cannot start playback: {AlSource} - {_linkedAssetData.ActualAssetMetadata.FilePath}");
    }

    /// <summary>
    /// Приостановить воспроизведение
    /// </summary>
    public void Pause()
    {
      AL.SourcePause(AlSource);
      OpenAlManager.CheckForError(
        $"Cannot pause playback: {AlSource} - {_linkedAssetData.ActualAssetMetadata.FilePath}");
    }

    /// <summary>
    /// Остановить воспроизведение
    /// </summary>
    public void Stop()
    {
      AL.SourceStop(AlSource);
      OpenAlManager.CheckForError(
        $"Cannot stop playback: {AlSource} - {_linkedAssetData.ActualAssetMetadata.FilePath}");
    }

    /// <summary>
    /// Сбросить воспроизведение
    /// </summary>
    public void Reset()
    {
      AL.SourceRewind(AlSource);
      OpenAlManager.CheckForError(
        $"Cannot reset playback: {AlSource} - {_linkedAssetData.ActualAssetMetadata.FilePath}");
    }

    /// <summary>
    /// Воспроизводится ли сейчас?
    /// </summary>
    /// <returns>True, если воспроизводится</returns>
    public bool IsPlaying()
    {
      ALSourceState sourceState = AL.GetSourceState(AlSource);
      return sourceState == ALSourceState.Playing;
    }

    /// <summary>
    /// На паузе ли сейчас?
    /// </summary>
    /// <returns>True, если на паузе</returns>
    public bool IsPaused()
    {
      ALSourceState sourceState = AL.GetSourceState(AlSource);
      return sourceState == ALSourceState.Paused;
    }

    /// <summary>
    /// Установка параметра зацикленности воспроизведения
    /// </summary>
    /// <param name="parIsLooped">Значение параметра зацикленности воспроизведения</param>
    public void SetIsLooped(bool parIsLooped)
    {
      AL.Source(AlSource, ALSourceb.Looping, parIsLooped);
    }

    /// <summary>
    /// Деструктор
    /// </summary>
    public void Dispose()
    {
      AL.DeleteSource(AlSource);
      AL.DeleteBuffer(AlBuffer);

      Console.WriteLine($"AL: Audio data {AlSource}:{AlBuffer} removed from memory");
    }

    /// <summary>
    /// Получить данные о формате аудио OpenAL контейнера WAVE
    /// </summary>
    /// <param name="parChannels">Количество каналов</param>
    /// <param name="parBits">Битность</param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException">Формат не поддерживается</exception>
    public static ALFormat GetSoundFormat(int parChannels, int parBits)
    {
      switch (parChannels)
      {
        case 1: return parBits == 8 ? ALFormat.Mono8 : ALFormat.Mono16;
        case 2: return parBits == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16;
        default: throw new NotSupportedException("The specified sound format is not supported.");
      }
    }

    /// <summary>
    /// Буфер OpenAL
    /// </summary>
    public int AlBuffer { get; private set; }

    /// <summary>
    /// Источник OpenAL
    /// </summary>
    public int AlSource { get; private set; }

    /// <summary>
    /// Количество аудио каналов
    /// </summary>
    public int Channels { get; private set; }

    /// <summary>
    /// Битность
    /// </summary>
    public int BitsPerSample { get; private set; }

    /// <summary>
    /// Частота дискретизации
    /// </summary>
    public int SampleRate { get; private set; }

    /// <summary>
    /// Аудио данные в бинарном виде
    /// </summary>
    public byte[] AudioBinaryData { get; private set; }
  }
}