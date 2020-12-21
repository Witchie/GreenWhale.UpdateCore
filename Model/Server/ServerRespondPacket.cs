using System.Collections.Generic;
namespace GreenWhale.UpdateCore.Model
{
    /// <summary>
    /// 服务端响应指定包
    /// </summary>
    public class ServerRespondPacket : SendFrameBase
    {
        /// <summary>
        /// 服务端响应指定包
        /// </summary>
        /// <param name="md5"></param>
        /// <param name="index"></param>
        /// <param name="packetlength"></param>
        /// <param name="data"></param>
        public ServerRespondPacket(uint md5,ushort index,ushort packetlength,byte[] data) : base(Code.ServerRespondPacket, md5)
        {
            var l=  index % 0x100;
            var h = index / 0x100;
            Index = new byte[] { (byte)l, (byte)h };
            var l1 = packetlength % 0x100;
            var h1 = packetlength / 0x100;
            Length = new byte[] { (byte)l1, (byte)h1 };
            Data = data;
        }
        /// <summary>
        /// 包序号
        /// </summary>
        public byte[] Index { get;private set; }
        /// <summary>
        /// 包长
        /// </summary>
        public byte[] Length { get;private set; }
        /// <summary>
        /// 数据包
        /// </summary>
        public byte[] Data { get;private set; }
        /// <summary>
        /// 数据域
        /// </summary>
        public override byte[] DataArea
        {
            get
            {
                List<byte> vs = new List<byte>();
                vs.AddRange(Index);
                vs.AddRange(Length);
                vs.AddRange(Data);
                return vs.ToArray();
            }
        }
    }
}
