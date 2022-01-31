using System;

namespace FileSignature
{
    /// <summary>
    /// Represents result of reading of a stream
    /// </summary>
    internal class StreamBlockReadResult
    {
        public int BlockNumber { get; }
        public int BlockSize { get; }

        public StreamBlockReadResult(int blockNumber, int blockSize)
        {
            BlockNumber = blockNumber;
            BlockSize = blockSize;
        }
    }
}
