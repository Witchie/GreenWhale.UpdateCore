using GreenWhale.UpdateCore.Model;
using System;
using System.Linq;

namespace GreenWhale.UpdateCore
{
    public class RXContainer
    {
        public RXContainer(byte[] data)
        {
            Data = data;
        }
        public Tuple<T, bool> GetFrame<T>() where T : ReceiveFrameBase, new ()
        {
            if (Data!=null&&Data.Length>4)
            {
                Byte[] receive=null;
                for (int i = 0; i < Data.Length; i++)
                {
                    if (Data[i]==Header)
                    {
                        receive= Data.Skip(i).ToArray();
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                if (receive!=null)
                {
                    var len = receive.Skip(1).Take(2).ToArray();
                    var length = len[0] + len[1] * 0x100;
                    if (receive.Length>length)
                    {
                        var frame=  receive.Skip(3).Take(length).ToArray();
                        T data = new T();
                        data.Create(frame);
                        return new Tuple<T, bool>(data, true);
                    }
                }
            }
            return new Tuple<T, bool>(default(T), false);
        }
        public byte[] Data { get; set; }
        public byte Header { get; set; } = 0xAA;
        public byte Footer { get; set; } = 0xFF;
    }
}
