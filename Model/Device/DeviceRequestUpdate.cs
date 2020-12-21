namespace GreenWhale.UpdateCore.Model
{
    using System.Linq;
    /// <summary>
    /// 设备端请求升级
    /// </summary>
    public class DeviceRequestUpdate : ReceiveFrameBase
    {
        /// <summary>
        /// 设备序列号
        /// </summary>
        public DeviceNumber DeviceNumber { get;private set; }
        protected override int DataAreaLength =>11;

        public override void Create(byte[] create)
        {
            Analysis(create);
            var n1 = DataArea.Take(7).ToArray();
            DeviceNumber = new DeviceNumber(n1);
        }
    }
}
