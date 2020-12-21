using Newtonsoft.Json;
using System;
using System.Collections.Generic;
namespace GreenWhale.UpdateCore.Model
{
    /// <summary>
    /// 发送帧基类
    /// </summary>
    [System.Serializable]
    public abstract class SendFrameBase:MD5Base
    {
        [Obsolete]
        public SendFrameBase() { }
        /// <summary>
        /// 发送帧基类
        /// </summary>
        /// <param name="code"></param>
        public SendFrameBase(Code code,uint md5)
        {
            Code = code;
            MD5Number = md5;
        }
        /// <summary>
        /// 功能码
        /// </summary>
        public Code Code { get; set; }
        /// <summary>
        /// CRC校验区域
        /// </summary>
        public abstract byte[] DataArea { get; }
       /// <summary>
       /// 完整帧
       /// </summary>
       [JsonIgnore]
       public byte[] CrcArea
        {
            get
            {
                List<byte> vs = new List<byte>();
                vs.Add((byte)Code);
                vs.AddRange(MD5);
                vs.AddRange(DataArea);
                return vs.ToArray();
            }
        }
        /// <summary>
        /// 完整数据包
        /// </summary>
        [JsonIgnore]
        public byte[] FullFrame
        {
            get
            {
                List<byte> vs = new List<byte>();
                vs.AddRange(CrcArea);
                vs.AddRange(CRC16);
                return vs.ToArray();
            }
        }
        /// <summary>
        /// 获取CRC结果
        /// </summary>
        [JsonIgnore]
        public byte[] CRC16
        {
            get
            {
                CRCHelper cRCHelper = new CRCHelper();
                return cRCHelper.GetCRC16(CrcArea);
            }
        }
    }
}
