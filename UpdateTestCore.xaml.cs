using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using GreenWhale.UpdateCore.Model;
using Lierda.Formatter;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
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
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UpdateTestCore : UserControl
    {
        public UpdateTestCore()
        {
            InitializeComponent();
            this.DataContext = this;
            dgv.SelectedItemChanged += Dgv_SelectedItemChanged;  
            updateHelper.GetPacket += UpdateHelper_GetPacket1;
            updateHelper.ReadData += UpdateHelper_ReadData;
        }

        private void Dgv_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            dgv.SelectedItem = null;
        }

        private void UpdateHelper_ReadData(byte[] obj,string txt)
        {
            Log("=>>>"+ obj .ToHex()+ "完整包数据:"+txt);
        }

        private Model.ServerRespondPacket UpdateHelper_GetPacket1(int packetidnex, uint md5number, DeviceRequestPacket deviceRequestPacket)
        {
            Log(deviceRequestPacket);
            var res =  BinPkgFiles.Where(p => p.MD5 == md5number).FirstOrDefault();
            if (res!=null)
            {
                if (res.ProcessValue!=null)
                {
                    res.ProcessValue.CurrentValue = packetidnex;
                }
                var pkg=   res.GetPacket(packetidnex, UpdateInfo.PacketSize);
                if (pkg!=null)
                {
                    var pkg2 = new ServerRespondPacket(md5number, Convert.ToUInt16(packetidnex), Convert.ToUInt16(pkg.Length), pkg);
                    Log(pkg2);
                    return pkg2;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                Log("指定MD5不存在");
                return null;
            }
        }


        public ObservableCollection<BinPkgFile> BinPkgFiles { get; set; } = new ObservableCollection<BinPkgFile>();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog() ;
            openFile.Filter = "固件升级包|*.binpkg";
            openFile.Title = "请选择打包过的固件升级包";
            if (openFile.ShowDialog()==true)
            {
                try
                {
                    PudateCreate pudateCreate = new PudateCreate();
                    var bindata = pudateCreate.GetBinPkgFile(openFile.FileName);
                    var dta = BinPkgFiles.Where(p => p.MD5 == bindata.MD5).FirstOrDefault();
                    if (dta == null)
                    {
                        bindata.ProcessValue = new ProcessValue();
                        BinPkgFiles.Add(bindata);
                    }
                    else
                    {
                        DXMessageBox.Show($"指定固件MD5已经存在，请更换一个再次尝试,MD5=>{dta.MD5}");
                    }
                }
                catch (Exception err)
                {
                    DXMessageBox.Show($"读取BinPKG包错误，{err.Message}");
                }

            }
        }

        private void ButtonInfo_Click(object sender, RoutedEventArgs e)
        {
            var info = sender as SimpleButton;
            if (info!=null && dgv.GetFocusedRow() is BinPkgFile binPkgFile)
            {
                dgv.SelectedItem = null;
                var dta = BinPkgFiles.Where(p => p.MD5 == binPkgFile.MD5).FirstOrDefault();
                if (dta!=null)
                {
                    BinPkgFiles.Remove(dta);
                }
                else
                {
                    DXMessageBox.Show("无法找到升级文件");
                }
            }
        }
        /// <summary>
        /// 单包大小
        /// </summary>
        public DeviceRespondUpdate UpdateInfo { get; set; }
        UpdateHelper updateHelper = new UpdateHelper(Activator.SerialPort);
        private async void ButtonInfo_Click_1(object sender, RoutedEventArgs e)
        {
            if (!updateHelper.SerialPort.IsOpen)
            {
                DXMessageBox.Show("请先配置串口");
                return;
            }
            UpdateInfo = null;
            var info = sender as SimpleButton;
            if (info != null  && dgv.GetFocusedRow() is BinPkgFile binPkgFile)
            {
                dgv.SelectedItem = null;
                info.IsEnabled = false;
                Log(binPkgFile.ServerRequestUpdate);
                var result=await  updateHelper.BeginUpdate(binPkgFile.ServerRequestUpdate);
                if (result!=null)
                {
                    Log(result);
                    UpdateInfo = result;
                    Log($"硬件版本:{result.HardwareVersion.Version},软件版本:{result.SoftwareVersion.Version},升级次数:{result.UpdateCount},单包长度:{result.PacketSize},状态:{result.State.ToString()}");
                    if (result.State==State.CheckSuccess)
                    {
                        var h=  binPkgFile.BinData.Length / result.PacketSize;
                        var l = binPkgFile.BinData.Length % result.PacketSize;
                        if (l!=0)
                        {
                            h++;
                        }
                        con.MaxValue = h;
                        binPkgFile.ProcessValue = new ProcessValue() { MaxValue = h, CurrentValue = 0 };
                        updateHelper.SysncHardInfo(new Model.Server.ServerRequestPacketCount(result.MD5Number, binPkgFile.BinData, Convert.ToUInt16(h)));
                    }
                }
                else
                {
                    Log("设备未响应，或设备未连接");
                }
                info.IsEnabled = true;
            }
        }
        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="log"></param>
        private void Log(string log)
        {
            this.Dispatcher.Invoke(new Action(delegate {
                logbox.AppendText($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss ms")}:{log}{Environment.NewLine}");
                logbox.ScrollToEnd();
            }));
        }
        private void Log(SendFrameBase context)
        {
            Log($"TX:{context.FullFrame.ToHex()}");
        }
        private void Log(ReceiveFrameBase receiveFrameBase)
        {
            Log($"RX:{receiveFrameBase.FullFrame.ToHex()}");
        }
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!updateHelper.SerialPort.IsOpen)
            {
                DXMessageBox.Show("请先配置串口");
                return;
            }
            ServerRequestDeviceVersion serverRequestDeviceVersion = new ServerRequestDeviceVersion();
            Log(serverRequestDeviceVersion);
            var version=await updateHelper.GetDeviceVersion(serverRequestDeviceVersion);
            if (version!=null)
            {
                Log($"硬件版本:{version.HardwareVersion.Version},软件版本:{version.SoftwareVersion.Version}");
            }
            else
            {
                Log("设备未响应，或设备未连接");
            }
        }

        private void logbox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            logbox.Clear();
        }
    }
}
