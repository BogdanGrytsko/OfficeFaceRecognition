using System;

namespace OfficeFaceRecognition.Video
{
    public class VideoSplitter
    {
        public event Action<IncomingStream> OnStreamReceived;

        public void PushStream(IncomingStream incStream)
        {
            OnStreamReceived?.Invoke(incStream);
        }
    }
}
