using Lierda.Formatter;
using System;
using System.Linq;
namespace GreenWhale.UpdateCore.Model
{
    /// <summary>
    /// 软件版本
    /// </summary>
    [System.Serializable]
    public class SoftwareVersion
    {
        [Obsolete]
        public SoftwareVersion() { }
        /// <summary>
        /// 软件版本
        /// </summary>
        /// <param name="version"></param>
        public SoftwareVersion(byte[] version)
        {
            if (version!=null&&version.Length==2)
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
        public byte[] VersionData { get; set; }
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
                VersionData = ByteAction.HexStringToByte(value).Reverse().ToArray();
            }
        }
    }
}
