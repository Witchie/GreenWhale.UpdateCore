
using System;
using System.Linq;
namespace GreenWhale.UpdateCore.Model
{
    /// <summary>
    /// 通讯帧类
    /// </summary>
    public  class ReceiveFrameBase: MD5Base
    {
        public ReceiveFrameBase()
        {

        }
        protected virtual void Analysis(byte[] receivedata)
        {
            ReceiveData = receivedata;
            Code = (Code)receivedata.Take(1).First();
            MD5 = receivedata.Skip(1).Take(4).ToArray();
            DataArea = receivedata.Skip(5).Take(DataAreaLength).ToArray();
            CRC16 = receivedata.Skip(5 + DataAreaLength).Take(2).ToArray();
        }
        /// <summary>
        /// 创建
        /// </summary>
        public virtual void Create(byte[] create)
        {
            FullFrame=create;
            Analysis(create);
        }
        /// <summary>
        /// 完整数据包
        /// </summary>
        public byte[] FullFrame { get; set; }
        protected virtual int DataAreaLength { get; }
        /// <summary>
        /// 数据域数据(MD5=》CRC前)
        /// </summary>
        public byte[] DataArea { get;private set; }
        protected byte[] ReceiveData { get;private set; }
        private byte[] _crc16;
        private byte[] _mD5;

        /// <summary>
        /// 校验码
        /// </summary>
        public  Code Code { get; set; }
        /// <summary>
        /// CRC校验
        /// </summary>
        public byte[] CRC16
        {
            get => _crc16;
            set
            {
                if (value != null && value.Length == 2)
                {
                    _crc16 = value;
                }

            }
        }
    }
}
