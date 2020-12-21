namespace GreenWhale.UpdateCore.Model
{
    /// <summary>
    /// 获取设备端版本
    /// </summary>
    public class ServerRequestDeviceVersion : SendFrameBase
    {
        public ServerRequestDeviceVersion() : base(Code.ServerRequestDeviceVersion, uint.MaxValue)
        {
        }

        public override byte[] DataArea => new byte[] { 0, 0, 0, 0 };
    }
}
