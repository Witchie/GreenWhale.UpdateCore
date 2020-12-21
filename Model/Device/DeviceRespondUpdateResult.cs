using System.Linq;

namespace GreenWhale.UpdateCore.Model
{
    /// <summary>
    /// 设备端响应升级结果
    /// </summary>
    public class DeviceRespondUpdateResult : ReceiveFrameBase
    {
        /// <summary>
        /// 执行状态
        /// </summary>
        public State State => (State)DataArea.First();
        protected override int DataAreaLength => 1;

        public override void Create(byte[] create)
        {
            FullFrame = create;
            Analysis(create);
        }
    }
}
