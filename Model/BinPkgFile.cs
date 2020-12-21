using DevExpress.Mvvm;
using GreenWhale.UpdateCore.Model;
using Lierda.Formatter;
using Newtonsoft.Json;
using System;
using System.Linq;
namespace GreenWhale.UpdateCore
{
    /// <summary>
    /// 固件包信息
    /// </summary>
    [System.Serializable]
    [JsonObject]
    public class BinPkgFile : ViewModelBase
    {
        private ProcessValue _processValue;
        private byte[] _binData;

        [Obsolete]
        public BinPkgFile()
        {
            _processValue = new ProcessValue();
        }
        public BinPkgFile(byte[] bindata, CodeInfo codeInfo)
        {
            CodeInfo = codeInfo;
            BinData = bindata;
            var hd = codeInfo.HardwareVersion;
            var dhh = ByteAction.HexStringToByte(hd.PadLeft(4, '0')).Reverse().ToArray();
            var sd = codeInfo.SoftwareVersion;
            var shh = ByteAction.HexStringToByte(sd.PadLeft(4, '0')).Reverse().ToArray();
            var add = codeInfo.Address;
            var addd = ByteAction.HexStringToByte(add.PadLeft(8, '0')).Reverse().ToArray();
            ServerRequestUpdate = new ServerRequestUpdate(MD5, new HardwareVersion(dhh), new SoftwareVersion(shh), addd);
        }
        /// <summary>
        /// 获取指定的包
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public byte[] GetPacket(int index, int size)
        {
            var db = BinData.Skip(index * size).Take(size).ToArray();
            return db;
        }
        /// <summary>
        /// 代码信息
        /// </summary>
        public CodeInfo CodeInfo { get; set; }
        /// <summary>
        /// MD5码
        /// </summary>
        public uint MD5 { get; set; } = 0;
        /// <summary>
        /// 升级进度
        /// </summary>
        public ProcessValue ProcessValue
        {
            get => _processValue; set
            {
                _processValue = value; RaisePropertyChanged();
            }
        }
        /// <summary>
        /// 固件信息
        /// </summary>
        [JsonIgnore]
        public byte[] BinData { get => _binData; private set => _binData = value; }
        public string Data
        {
            get
            {
                if (BinData == null)
                {
                    return null;
                }
                else
                {
                    var tmp = Convert.ToBase64String(BinData);
                    return tmp;
                }
            }
            set
            {
                var buffer = Convert.FromBase64String(value);
                BinData = buffer;
            }
        }
        /// <summary>
        /// 服务端发起请求包
        /// </summary>
        public ServerRequestUpdate ServerRequestUpdate { get; set; }
    }
    /// <summary>
    /// 进度条
    /// </summary>
    [System.Serializable]
    [JsonObject]
    public class ProcessValue : ViewModelBase
    {
        private int _currentValue=0;
        private int _maxValue=0;
        /// <summary>
        /// 当前值
        /// </summary>
        public int CurrentValue
        {
            get => _currentValue; set
            {
                _currentValue = value; RaisePropertyChanged(); RaisePropertyChanged(nameof(Percent));
            }
        }
        [JsonIgnore]
        public double Percent
        {
            get
            {
                if (MaxValue==0)
                {
                    return 0;
                }
                return (CurrentValue * 100.0/ MaxValue);
            }
        }

        /// <summary>
        /// 最大值
        /// </summary>
        public int MaxValue
        {
            get => _maxValue; set
            {
                _maxValue = value; RaisePropertyChanged(); RaisePropertyChanged(nameof(Percent));
            }
        }
    }
}
