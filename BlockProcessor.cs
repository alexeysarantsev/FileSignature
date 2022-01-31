using System;

namespace FileSignature
{
    internal class BlockProcessor
    {
        private readonly StreamBlockReader _reader;
        private readonly HashCalculator _calculator;
        private readonly int _blockSize;
        private readonly byte[] _buffer;

        public BlockProcessor(StreamBlockReader reader, HashCalculator calculator, int blockSize)
        {
            _reader = reader;
            _calculator = calculator;
            _blockSize = blockSize;
            _buffer = new byte[_blockSize];
        }

        public void Process()
        {
            bool isEndOfFile;
            do
            {
                var readResult = _reader.Read(_buffer);
                isEndOfFile = readResult.BlockSize == 0;
                if (!isEndOfFile)
                {
                    var hash = _calculator.GetHashString(_buffer, readResult.BlockSize);
                    Console.WriteLine($"{readResult.BlockNumber}\t{hash}");
                }
            }
            while (!isEndOfFile);
        }
    }
}
