using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading;

namespace FileSignature
{
    class Program
    {
        static void Main(string[] args)
        {
            const int defaultBlockSize = 10000; // default if block size is not passed
            string filePath;
            int blockSize = defaultBlockSize;

            if (args.Length < 1)
            {
                Console.WriteLine("A path to a file not specified.");
                return;
            }

            if (args.Length >= 2 && !int.TryParse(args[1], out blockSize))
            {
                Console.WriteLine("The size of the block must be the whole number.");
                return;
            }

            if (blockSize <= 0)
            {
                Console.WriteLine("The size of the block must be the positive whole number.");
                return;
            }
            filePath = args[0];

            var fileInfo = new FileInfo(filePath);

            if (fileInfo.Length == 0) //empty file, nothing to do.
            {
                return;
            }

            //if number of blocks is less than number of processors no reason to create extra threads 
            //thats't the question how many threads is optional
            //we have the file read operations and it could be slow
            //compution of hash depends on the block size (bigger is slower)
            //lets be ready for big blocks and utilize all cores
            int threadCount = (int) Math.Min(Environment.ProcessorCount, fileInfo.Length / blockSize + (fileInfo.Length % blockSize == 0 ? 0 : 1));

            using var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            using var sha256 = SHA256.Create();
            var reader = new StreamBlockReader(stream, blockSize);
            var calculator = new HashCalculator(sha256);

            var threads = new List<Thread>(); 
            for (int i = 0; i < threadCount; i++)
            {
                var blockProcessor = new BlockProcessor(reader, calculator, blockSize);
                var thread = new Thread(new ThreadStart(blockProcessor.Process));
                threads.Add(thread);
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }
        }
    }
}
