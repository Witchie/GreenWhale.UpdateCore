using System.Linq;
namespace GreenWhale.UpdateCore.Model
{
    /// <summary>
    /// 设备端响应升级
    /// </summary>
    public class DeviceRespondUpdate : ReceiveFrameBase
    {
        public override void Create(byte[] create)
        {
            FullFrame = create;
            Analysis(create);
            var b1 = DataArea.Take(7).ToArray();
            DeviceNumber = new DeviceNumber(b1);
            var b2 = DataArea.Skip(7).Take(1).First();
            State = (State)(b2);
            var b3 = DataArea.Skip(8).Take(2).ToArray();
            HardwareVersion = new HardwareVersion(b3);
            var b4 = DataArea.Skip(10).Take(2).ToArray();
            SoftwareVersion = new SoftwareVersion(b4);
            var b5 = DataArea.Skip(12).Take(1).First();
            UpdateCount =b5;
            var b6 = DataArea.Skip(13).Take(2).ToArray();
            PacketSize = (ushort)(b6[0] + b6[1] * 0x100);
        }
        /// <summary>
        /// 单包最大长度
        /// </summary>
        public ushort PacketSize { get;private set; }
        /// <summary>
        /// 升级次数
        /// </summary>
        public byte UpdateCount { get;private set; }
        /// <summary>
        /// 软件版本
        /// </summary>
        public SoftwareVersion SoftwareVersion { get;private set; }
        /// <summary>
        /// 硬件版本
        /// </summary>
        public HardwareVersion HardwareVersion { get;private set; }
        /// <summary>
        /// 升级状态
        /// </summary>
        public State State { get;private set; }
        /// <summary>
        /// 设备编号
        /// </summary>
        public DeviceNumber DeviceNumber { get; private set; }
        /// <summary>
        /// 数据域长度
        /// </summary>
        protected override int DataAreaLength => 18;
    }
}
