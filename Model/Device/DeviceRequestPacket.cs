namespace GreenWhale.UpdateCore.Model
{
    using System.Linq;
    /// <summary>
    /// 设备端请求指定包
    /// </summary>
    public class DeviceRequestPacket : ReceiveFrameBase
    {
        /// <summary>
        /// 请求的包序号
        /// </summary>
        public int PacketIndex => DataArea[0] + DataArea[1] * 0x100;
        protected override int DataAreaLength => 2;

        public override void Create(byte[] create)
        {
            FullFrame = create;
            Analysis(create);
        }
    }
}
