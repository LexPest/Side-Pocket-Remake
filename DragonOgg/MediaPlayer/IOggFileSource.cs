using DragonOgg.csvorbis;
using DragonOgg.TagLibSharp;

namespace DragonOgg.MediaPlayer {

    public interface IOggFileSource {
        string FileName { get; }
        VorbisFile VorbisFile { get; }
        File TagLibFile { get; }
    }
}

