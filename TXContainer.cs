using System.Collections.Generic;

namespace GreenWhale.UpdateCore
{
    public class TXContainer
    {

        public TXContainer(byte[] data)
        {
            Data = data;
        }

        public byte Header { get; set; } = 0xAA;
        public byte Footer { get; set; } = 0xFF;
        public byte[] Length
        {
            get
            {
                var len=  Data.Length;
                var l= len % 0x100;
                var h = len / 0x100;
                return new byte[] {(byte)l,(byte)h };
            }
        }
        public byte[] Data { get; set; }
        public byte[] FullFrame
        {
            get
            {
                List<byte> buffer = new List<byte>();
                buffer.Add(Header);
                buffer.AddRange(Length);
                buffer.AddRange(Data);
                buffer.Add(Footer);
                return buffer.ToArray();
            }
        }
    }
}
