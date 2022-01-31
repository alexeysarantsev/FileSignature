using System;
using System.IO;

namespace FileSignature
{
    internal class StreamBlockReader
    {
        private object _lockObject = new object();
        private bool _isEndOfSreamReached;
        private int _blockNumber;
        private readonly int _blockSize;
        private readonly Stream _stream;
        private bool _isCrashed = false;

        public StreamBlockReader(Stream stream, int blockSize)
        {
            _stream = stream;
            _blockSize = blockSize;
        }

        /// <summary>
        /// reads the next part of the stream and fill the buffer
        /// </summary>
        /// <returns>StreamBlockReadResult</returns>
        public StreamBlockReadResult Read(byte[] buffer)
        {
            if (!_isEndOfSreamReached && !_isCrashed)
            {
                lock (_lockObject)
                {
                    if (!_isEndOfSreamReached && !_isCrashed)
                    {
                        try
                        {
                            var bytesRead = _stream.Read(buffer, 0, _blockSize);
                            _isEndOfSreamReached = bytesRead == 0;
                            if (!_isEndOfSreamReached)
                            {
                                var result = new StreamBlockReadResult(_blockNumber, bytesRead);
                                _blockNumber++;
                                return result;
                            }
                        }
                        catch (Exception e)
                        {
                            _isCrashed = true;
                            Console.WriteLine($"{e.Message}{Environment.NewLine}{e.StackTrace}");
                        }
                    }
                }
            }
            return new StreamBlockReadResult(_blockNumber, 0);
        }
    }
}
