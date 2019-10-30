using System;
using System.IO;
using System.Threading;

namespace FaceRecognition.DBStorage
{
    public class DB2OpenCVTempFileGenerator : IDisposable
    {
        private string filePath;
        private bool requestInitialOwnership = true;
        private bool mutexWasCreated;
        Mutex mFile;

        /// <summary>
        /// Generate temporary file for OpenCV usage 
        /// </summary>
        /// <param name="filePath">Should contain only ASC characters; max length is 260 characters</param>
        /// <param name="fileContent"></param>
        public DB2OpenCVTempFileGenerator(string filePath, byte[] fileContent)
        {
            if (string.IsNullOrEmpty(filePath) || filePath.Length > 260)
            {
                throw new Exception(string.Format("Parameter filePath = {0} is incorrect", filePath));
            }

            this.filePath = filePath;
            mFile = new Mutex(requestInitialOwnership,
                           filePath,
                           out mutexWasCreated);

            if (!(requestInitialOwnership && mutexWasCreated))
            {
                Console.WriteLine("Waiting for the named mutex.");
                mFile.WaitOne();
            }
            File.WriteAllBytes(filePath, fileContent);
        }
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    mFile.ReleaseMutex();
                }
                File.Delete(filePath);                
                // TODO: set large fields to null.
                disposedValue = true;
            }
        }

        ~DB2OpenCVTempFileGenerator()
        {
            // Do not change this code. CCleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
