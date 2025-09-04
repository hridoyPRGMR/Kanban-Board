using System.Security.Cryptography;

namespace KanbanBoard.Domain.Common
{
    public class SequentialGuid
    {
        public static Guid Create()
        {
            //generate random bytes
            var randomBytes = new Byte[10];
            RandomNumberGenerator.Fill(randomBytes);

            //current timestamp (ticks = 100-ns intervals)
            long timestamp = DateTime.UtcNow.Ticks / 10000L; //ms precision

            byte[] timestampBytes = BitConverter.GetBytes(timestamp);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(timestampBytes);

            byte[] guidBytes = new byte[16];
            Array.Copy(timestampBytes, 2, guidBytes, 0, 6);   // first 6 bytes = timestamp
            Array.Copy(randomBytes, 0, guidBytes, 6, 10);    // last 10 bytes = randomness

            return new Guid(guidBytes);
        }
    }
}