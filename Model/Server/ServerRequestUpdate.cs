using Newtonsoft.Json;
using System;
using System.Collections.Generic;
namespace GreenWhale.UpdateCore.Model
{
    /// <summary>
    /// 服务端发起请求升级
    /// </summary>
    [System.Serializable]
    public class ServerRequestUpdate : SendFrameBase
    {
        [Obsolete]
        public ServerRequestUpdate() : base() { }
        public ServerRequestUpdate(uint md5,HardwareVersion hardwareVersion,SoftwareVersion softwareVersion, byte[] Address) : base(Code.ServerRequestUpdate, md5)
        {
            this.Address = Address;
            this.HardwareVersion = hardwareVersion;
            this.SoftwareVersion = softwareVersion;
        }
        /// <summary>
        /// 固件地址
        /// </summary>
        public HardwareVersion HardwareVersion { get;  set; }
        /// <summary>
        /// 软件地址
        /// </summary>
        public SoftwareVersion SoftwareVersion { get;  set; }
        /// <summary>
        /// 偏移位置
        /// </summary>
        public byte[] Address { get;  set; }
        /// <summary>
        /// 数据
        /// </summary>
        [JsonIgnore]
        public override byte[] DataArea
        {
            get
            {
                List<byte> vs = new List<byte>();
                vs.AddRange(HardwareVersion.VersionData);
                vs.AddRange(SoftwareVersion.VersionData);
                var bit = Address;
                vs.AddRange(bit);
                vs.AddRange(new byte[] {0,0,0,0 });
                return vs.ToArray();
            }
        }
    }
}
