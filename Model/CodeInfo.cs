using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GreenWhale.UpdateCore
{
    /// <summary>
    /// 源码信息
    /// </summary>
    [System.Serializable]
    public class CodeInfo : INotifyPropertyChanged
    {
        private string _hardwareVersion = "0001";
        private string _softwareVersion = "0001";
        private string _address = "00000000";
        private string _sourcePath;
        private string _savePath;
        private string _projectName="项目名称";
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName
        {
            get => _projectName; set
            {
                _projectName = value;Changed();
            }
        }
        /// <summary>
        /// 固件版本
        /// </summary>
        public string HardwareVersion
        {
            get => _hardwareVersion; set
            {
                _hardwareVersion = value; Changed();
            }
        }
        /// <summary>
        /// 软件版本
        /// </summary>
        public string SoftwareVersion
        {
            get => _softwareVersion; set
            {
                _softwareVersion = value; Changed();
            }
        }
        /// <summary>
        /// 偏移地址
        /// </summary>
        public string Address
        {
            get => _address; set
            {
                _address = value; Changed();
            }
        }
        /// <summary>
        /// 原陆军
        /// </summary>
        public string SourcePath
        {
            get => _sourcePath; set
            {
                _sourcePath = value; Changed();
            }
        }
        /// <summary>
        /// 保存位置
        /// </summary>
        public string SavePath
        {
            get => _savePath; set
            {
                _savePath = value; Changed();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 属性被修改
        /// </summary>
        /// <param name="name"></param>
        protected void Changed([CallerMemberName]string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
