namespace GreenWhale.UpdateCore.Model
{
    using System.Linq;
    /// <summary>
    /// 设备端响应版本(0x86)
    /// </summary>
    public class DeviceRespondDeviceVersion : ReceiveFrameBase
    {
        /// <summary>
        /// 软件版本
        /// </summary>
        public SoftwareVersion SoftwareVersion { get;private set; }
        /// <summary>
        /// 硬件版本
        /// </summary>
        public HardwareVersion HardwareVersion { get;private set; }
        protected override int DataAreaLength => 8;

        public override void Create(byte[] create)
        {
            FullFrame = create;

            Analysis(create);
            var version = DataArea.Take(2).ToArray();
            HardwareVersion = new HardwareVersion(version);
            var version1 = DataArea.Skip(2).Take(2).ToArray();
            SoftwareVersion = new SoftwareVersion(version1);
        }
    }
}
