using System;

namespace OfficeFaceRecognition
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
