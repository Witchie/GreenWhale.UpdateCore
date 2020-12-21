using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
    /// SerialPortVIew.xaml 的交互逻辑
    /// </summary>
    public partial class SerialPortView : UserControl
    {
        public SerialPortView()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            cmbname.ItemsSource = SerialPort.GetPortNames();
            var sp = Activator.SerialPort;

            if (sp.IsOpen)
            {
                spstate.Text = "串口已经打开";
            }
            else
            {
                spstate.Text = "串口已经关闭";
            }
        }

        private void SimpleButton_Click(object sender, RoutedEventArgs e)
        {
            var names=  SerialPort.GetPortNames();
            if (names.Contains(cmbname.Text)&&!string.IsNullOrEmpty(cmbbaudrate.Text))
            {
                var sp=  Activator.SerialPort;
                if (sp.IsOpen)
                {
                    sp.Close();
                }
                sp.PortName = cmbname.Text;
                sp.BaudRate = int.Parse(cmbbaudrate.Text);
                sp.Open();
                spstate.Text = "串口已经打开";
            }
            else
            {
                MessageBox.Show("请选择参数");
            }
        }

        private void SimpleButton_Click_1(object sender, RoutedEventArgs e)
        {
            var sp = Activator.SerialPort;
            if (sp.IsOpen)
            {
                sp.Close();
            }
            spstate.Text = "串口已经关闭";
        }
    }
}
