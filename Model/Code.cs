namespace GreenWhale.UpdateCore.Model
{
    /// <summary>
    /// 功能码
    /// </summary>
    [System.Serializable]
    public enum Code : byte
    {
        /// <summary>
        /// 服务端请求升级
        /// </summary>
        ServerRequestUpdate = 0x01,
        /// <summary>
        /// 设备端响应升级
        /// </summary>
        DeviceRespondUpdate = 0x81,
        /// <summary>
        /// 服务端通知包总数
        /// </summary>
        ServerRequestPacketCount = 0x02,
        /// <summary>
        /// 设备端请求指定包
        /// </summary>
        DeviceRequestPacket = 0x83,
        /// <summary>
        /// 服务端响应指定包
        /// </summary>
        ServerRespondPacket = 0x03,
        /// <summary>
        /// 设备端响应升级结果
        /// </summary>
        DeviceRespondUpdateResult = 0x84,
        /// <summary>
        /// 设备端请求升级
        /// </summary>
        DeviceRequestUpdate = 0x85,
        /// <summary>
        /// 获取设备端版本
        /// </summary>
        ServerRequestDeviceVersion = 0x06,
        /// <summary>
        /// 设备端响应版本
        /// </summary>
        DeviceRespondDeviceVersion = 0x86,
    }
}
