using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenWhale.UpdateCore.Model.Server
{
    public class ServerRequestPacketCount : SendFrameBase
    {
        /// <summary>
        /// 服务端通知包总数
        /// </summary>
        /// <param name="md5">MD5</param>
        /// <param name="binData">固件包文件</param>
        public ServerRequestPacketCount( uint md5,byte[] binData,ushort packetcount) : base(Code.ServerRequestPacketCount, md5)
        {
            BinData = binData;
            var bit = packetcount;
            var bit0 = bit % 0x100;
            var bit1 = bit / 0x100 ;
            PacketCount = new byte[] { (byte)bit0, (byte)bit1 };
        }
        /// <summary>
        /// 固件包信息
        /// </summary>
        public byte[] BinData { get;private set; }
        /// <summary>
        /// CRC校验
        /// </summary>
        public byte[] BinCRC => new CRCHelper().GetCRC16(BinData);
        /// <summary>
        /// 固件包长度
        /// </summary>
        public byte[] BinDataLength
        {
            get
            {
                var len=   BinData.Length;
                var bit = len;
                var bit0 = bit % 0x100;
                var bit1 = bit / 0x100 % 0x100;
                var bit2 = bit / 0x10000 % 0x100;
                var bit3 = bit / 0x1000000 % 0x100;
                var res = new byte[] { (byte)bit0, (byte)bit1, (byte)bit2, (byte)bit3 };
                return res ;
            }
        }
        /// <summary>
        /// 分包总数
        /// </summary>
        public byte[] PacketCount { get;private set; }
        /// <summary>
        /// 数据域数据
        /// </summary>
        public override byte[] DataArea
        {
            get
            {
                List<byte> vs = new List<byte>();
                vs.AddRange(BinCRC);
                vs.AddRange(BinDataLength);
                vs.AddRange(PacketCount);
                vs.AddRange(new byte[] {0,0,0,0 });
                return vs.ToArray();
            }
        }
    }
}
