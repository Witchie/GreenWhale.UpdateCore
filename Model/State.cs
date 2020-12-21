namespace GreenWhale.UpdateCore.Model
{
    using System.Linq;
    /// <summary>
    /// 状态
    /// </summary>
    public enum State
    {
        /// <summary>
        /// 检查正确
        /// </summary>
        CheckSuccess=0x00,
        /// <summary>
        /// 硬件版本错误
        /// </summary>
        HardareError=0xF1,
        /// <summary>
        /// 软件版本错误
        /// </summary>
        SoftwareError=0xF2,
        /// <summary>
        /// 写入地址错误
        /// </summary>
        AddressError=0xF3,
    }
}
