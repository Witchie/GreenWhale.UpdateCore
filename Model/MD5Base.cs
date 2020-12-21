
using System;
namespace GreenWhale.UpdateCore.Model
{
    /// <summary>
    /// MD5设置类
    /// </summary>
    [System.Serializable]
    public abstract class MD5Base
    {
        private byte[] _mD5;
        /// <summary>
        /// MD5校验
        /// </summary>
        public byte[] MD5
        {
            get => _mD5; set
            {
                if (value != null && value.Length == 4)
                {
                    _mD5 = value;
                }
                else
                {
                    throw new ArgumentException("MD5必须4位");
                }
            }
        }
        /// <summary>
        /// MD5值
        /// </summary>
        public uint MD5Number
        {
            get
            {
                var rres=   MD5[0] + MD5[1] * 0x100 + MD5[2] * 0x10000U + MD5[3] * 0x1000000U;
                return Convert.ToUInt32(rres);
            }
            set
            {
                var bit = value;
                var bit0 = bit % 0x100;
                var bit1 = bit/0x100 % 0x100;
                var bit2 = bit/0x10000 % 0x100;
                var bit3 = bit/0x1000000 % 0x100;
                MD5 = new byte[] { (byte)bit0, (byte)bit1, (byte)bit2, (byte)bit3 };
            }
        }
    }
}
