using System;
using DragonOgg.csvorbis;
using DragonOgg.TagLibSharp;

namespace DragonOgg.MediaPlayer {

    sealed class LocalOggFile : IOggFileSource {

        public string FileName {
            get;
            private set;
        }

        public VorbisFile VorbisFile {
            get;
            private set;
        }

        public File TagLibFile {
            get {
                try
                {
                    return File.Create (FileName);
                }
                catch (UnsupportedFormatException ex)
                {
                    throw new OggFileReadException ("Unsupported format (not an ogg?)\n" + ex.Message, FileName);
                }
                catch (CorruptFileException ex)
                {
                    throw new OggFileCorruptException (ex.Message, FileName, "Tags");
                }
            }
        }

        public LocalOggFile (string path) {

            FileName = path;

            try
            {
                VorbisFile = new VorbisFile (path);
            }
            catch (Exception ex)
            {
                throw new OggFileReadException ("Unable to open file for data reading\n" + ex.Message, path);   
            }

        }
    }
}

