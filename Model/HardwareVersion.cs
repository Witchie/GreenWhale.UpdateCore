using Lierda.Formatter;
using Newtonsoft.Json;
using System;
using System.Linq;
namespace GreenWhale.UpdateCore.Model
{
    /// <summary>
    /// 硬件版本
    /// </summary>
    [System.Serializable]
    public class HardwareVersion
    {
        [Obsolete]
        public HardwareVersion() { }
        /// <summary>
        /// 硬件版本
        /// </summary>
        /// <param name="version"></param>
        public HardwareVersion(byte[] version)
        {
            if (version != null && version.Length == 2)
            {
                VersionData = version;
            }
            else
            {
                throw new ArgumentException("版本必须俩个字节");
            }
        }

        /// <summary>
        /// 软件版本
        /// </summary>
        [JsonIgnore]
        public byte[] VersionData { get;  set; }
        /// <summary>
        /// 版本
        /// </summary>
        public string Version
        {
            get
            {
                return VersionData.Reverse().ToArray().ToHex();
            }
            set
            {
                VersionData= ByteAction.HexStringToByte(value).Reverse().ToArray();
            }
        }
    }
}
