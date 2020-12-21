using DevExpress.Xpf.Core;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GreenWhale.UpdateCore
{
    /// <summary>
    /// UpdatePkgManage.xaml 的交互逻辑
    /// </summary>
    public partial class UpdatePkgManage : UserControl
    {
        public UpdatePkgManage()
        {
            InitializeComponent();
            this.DataContext = pudateCreate.CodeInfo;
        }
        public PudateCreate pudateCreate { get; private set; } = new PudateCreate();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "未处理的固件包|*.bin";
            openFileDialog.Title = "请选择未处理的固件包";
            if (openFileDialog.ShowDialog() == true)
            {
                pudateCreate.CreateUpdateFile(openFileDialog.FileName);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(pudateCreate.CodeInfo.ProjectName))
            {
                DXMessageBox.Show("请填入项目名称");
                return;
            }
            if (string.IsNullOrEmpty(pudateCreate.CodeInfo.HardwareVersion))
            {
                DXMessageBox.Show("请填入软件版本");
                return;
            }
            if (string.IsNullOrEmpty(pudateCreate.CodeInfo.ProjectName))
            {
                DXMessageBox.Show("请填入硬件版本");
                return;
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "已处理好的固件包|*.binpkg";
            saveFileDialog.Title = "请选择要保存的文件位置";
            saveFileDialog.DefaultExt = "*.binpkg";
            saveFileDialog.AddExtension = true;
            saveFileDialog.FileName = $"{pudateCreate.CodeInfo.ProjectName}_V{pudateCreate.CodeInfo.HardwareVersion}{pudateCreate.CodeInfo.SoftwareVersion}_{pudateCreate.CodeInfo.Address}";
            if (saveFileDialog.ShowDialog() == true)
            {
                pudateCreate.CodeInfo.SavePath = saveFileDialog.FileName;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(pudateCreate.CodeInfo.ProjectName))
            {
                DXMessageBox.Show("请填入项目名称");
                return;
            }
            if (string.IsNullOrEmpty(pudateCreate.CodeInfo.HardwareVersion))
            {
                DXMessageBox.Show("请填入软件版本");
                return;
            }
            if (string.IsNullOrEmpty(pudateCreate.CodeInfo.ProjectName))
            {
                DXMessageBox.Show("请填入硬件版本");
                return;
            }
            if (string.IsNullOrEmpty(pudateCreate.CodeInfo.SourcePath))
            {
                DXMessageBox.Show("请选择要处理的固件包");
                return;
            }
            if (string.IsNullOrEmpty(pudateCreate.CodeInfo.SavePath))
            {
                DXMessageBox.Show("请选择要保存的路径");
                return;
            }
            var res = pudateCreate.CreatePkg();
            if (res)
            {
                DXMessageBox.Show("创建成功");
            }
            else
            {
                DXMessageBox.Show("创建失败");
            }
        }
    }
}
