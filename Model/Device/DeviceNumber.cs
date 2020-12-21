using System;
namespace GreenWhale.UpdateCore.Model
{
    /// <summary>
    /// 设备编号
    /// </summary>
    public class DeviceNumber
    {
        public DeviceNumber(byte[] data)
        {
            Data = data;
        }

        public byte[] Data { get; set; }
        /// <summary>
        /// 设备ID
        /// </summary>
        public string DeviceID => BitConverter.ToString(Data);
    }
}
